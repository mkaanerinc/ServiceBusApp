using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using ServicebusApp.Common;
using System.Text;

namespace ServicebusApp.ProducerApi.Services;

public class AzureService
{
    private readonly ManagementClient _managementClient;

    public AzureService(ManagementClient managementClient)
    {
        _managementClient = managementClient;
    }

    public async Task SendMessageToQueue(string queueName, object messageContent, string messageType = null)
    {
        IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);

        await SendMessage(client, messageContent, messageType);
    }

    public async Task CreateQueueIfNotExists(string queueName)
    {
        if(!await _managementClient.QueueExistsAsync(queueName))
            await _managementClient.CreateQueueAsync(queueName);
    }

    public async Task SendMessageToTopic(string topicName, object messageContent, string messageType = null)
    {
        ITopicClient client = new TopicClient(Constants.ConnectionString, topicName);

        await SendMessage(client, messageContent, messageType);
    }

    public async Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName, string messageType = null, string ruleName = null)
    {
        if (!await _managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
            return;

        if(messageType != null)
        {
            SubscriptionDescription sd = new(topicName, subscriptionName);

            CorrelationFilter filter = new();
            filter.Properties["MessageType"] = messageType;

            RuleDescription rd = new(ruleName ?? messageType + "Rule", filter);

            await _managementClient.CreateSubscriptionAsync(sd,rd);
        }else
        {
            await _managementClient.CreateSubscriptionAsync(topicName, subscriptionName);

        }
    }

    public async Task CreateTopicIfNotExists(string topicName)
    {
        if (!await _managementClient.TopicExistsAsync(topicName))
            await _managementClient.CreateTopicAsync(topicName);
    }

    private async Task SendMessage(ISenderClient client, object messageContent, string messageType = null)
    {
        var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

        var message = new Message(byteArray);
        message.UserProperties["MessageType"] = messageType;

        await client.SendAsync(message);
    }
}
