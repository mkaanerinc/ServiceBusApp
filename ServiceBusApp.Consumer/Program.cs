using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServicebusApp.Common;
using ServicebusApp.Common.Events;
using System;
using System.Text;

namespace ServicebusApp.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsumeQueue<OrderCreatedEvent>(Constants.OrderCreatedQueueName, i =>
            //{
            //    Console.WriteLine($"OrderCreatedEvent ReceivedMessage with id: {i.Id}, Name: {i.ProductName}");
            //}).Wait();

            //ConsumeQueue<OrderDeletedEvent>(Constants.OrderDeletedQueueName, i =>
            //{
            //    Console.WriteLine($"OrderDeletedEvent ReceivedMessage with id: {i.Id}");
            //}).Wait();

            ConsumeSub<OrderCreatedEvent>(Constants.OrderTopic,Constants.OrderCreatedSubName, i =>
            {
                Console.WriteLine($"OrderCreatedEvent ReceivedMessage with id: {i.Id}, Name: {i.ProductName}");
            }).Wait();

            ConsumeSub<OrderDeletedEvent>(Constants.OrderTopic, Constants.OrderDeletedSubName, i =>
            {
                Console.WriteLine($"OrderDeletedEvent ReceivedMessage with id: {i.Id}");
            }).Wait();

            Console.ReadLine();
        }

        //private static async Task ConsumeQueue<T>(string queueName, Action<T> receivedAction)
        //{
        //    IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);

        //    client.RegisterMessageHandler(async (message, ct) =>
        //    {
        //        var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));

        //        receivedAction(model);

        //        await Task.CompletedTask;
        //    },
        //    new MessageHandlerOptions(i => Task.CompletedTask));

        //    Console.WriteLine($"{typeof(T).Name} is listening...");
        //}

        private static async Task ConsumeSub<T>(string topicName, string subName, Action<T> receivedAction)
        {
            ISubscriptionClient client = new SubscriptionClient(Constants.ConnectionString, topicName, subName);

            client.RegisterMessageHandler(async (message, ct) =>
            {
                var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));

                receivedAction(model);

                await Task.CompletedTask;
            },
            new MessageHandlerOptions(i => Task.CompletedTask));

            Console.WriteLine($"{typeof(T).Name} is listening...");
        }
    }
}