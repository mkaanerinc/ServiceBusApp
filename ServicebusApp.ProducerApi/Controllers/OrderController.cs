﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicebusApp.Common;
using ServicebusApp.Common.Dto;
using ServicebusApp.Common.Events;
using ServicebusApp.ProducerApi.Services;

namespace ServicebusApp.ProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AzureService _azureService;

        public OrderController(AzureService azureService)
        {
            _azureService = azureService;
        }

        [HttpPost]
        public async Task CreateOrder(OrderDto order)
        {
            // insert order into database

            var orderCreatedEvent = new OrderCreatedEvent()
            {
                Id = order.Id,
                ProductName = order.ProductName,
                CreatedOn = DateTime.Now
            };

            // Queue
            //await _azureService.CreateQueueIfNotExists(Constants.OrderCreatedQueueName);

            //await _azureService.SendMessageToQueue(Constants.OrderCreatedQueueName,orderCreatedEvent);

            // Topic
            await _azureService.CreateTopicIfNotExists(Constants.OrderTopic);
            await _azureService.CreateSubscriptionIfNotExists(Constants.OrderTopic,Constants.OrderCreatedSubName, "OrderCreated", "OrderCreatedOnly");

            await _azureService.SendMessageToTopic(Constants.OrderTopic, orderCreatedEvent, "OrderCreated");
        }

        [HttpDelete("{id}")]
        public async Task DeleteOrder(int id)
        {
            // insert order into database

            var orderDeletedEvent = new OrderDeletedEvent()
            {
                Id = id,
                CreatedOn = DateTime.Now
            };

            //Queue
            //await _azureService.CreateQueueIfNotExists(Constants.OrderDeletedQueueName);

            //await _azureService.SendMessageToQueue(Constants.OrderDeletedQueueName, orderDeletedEvent);

            // Topic
            await _azureService.CreateTopicIfNotExists(Constants.OrderTopic);
            await _azureService.CreateSubscriptionIfNotExists(Constants.OrderTopic, Constants.OrderDeletedSubName, "OrderDeleted", "OrderDeletedOnly");

            await _azureService.SendMessageToTopic(Constants.OrderTopic, orderDeletedEvent, "OrderDeleted");
        }
    }
}
