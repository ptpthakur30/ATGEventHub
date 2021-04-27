using ATGEventHub.Library.Interface;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATGEventHub.Library.Implementation
{
    public class EventSender<T> : IEventSender<T> where T : class
    {
        private readonly string _connectionString;
        private readonly string _eventHubName;
        private EventDataBatch generateData;
        private EventHubProducerClient _producerClient;

        public EventSender(string connectionString, string eventHubName)
        {
            _connectionString = connectionString;
            _eventHubName = eventHubName;
        }
        public async Task GenerateEvent(IEnumerable<T> dataList)
        {
            try
            {
                await using (_producerClient = new EventHubProducerClient(_connectionString, _eventHubName))
                {
                    // send in batch
                    var batchOptions = new CreateBatchOptions();
                    generateData = _producerClient.CreateBatchAsync(batchOptions).Result;
                    foreach (var data in dataList)
                    {
                        var dataAsJson = JsonConvert.SerializeObject(data);
                        var eveData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
                        // All value should be dynamic
                        eveData.Properties.Add("UserId", data.GetType().GetProperty("UserId").GetValue(data)?.ToString());
                        eveData.Properties.Add("Location", data.GetType().GetProperty("Region").GetValue(data)?.ToString());
                        if (!generateData.TryAdd(eveData))
                        {
                            _producerClient.SendAsync(generateData).Wait();
                            batchOptions = new CreateBatchOptions();
                            generateData = _producerClient.CreateBatchAsync(batchOptions).Result;
                            generateData.TryAdd(eveData);
                        }
                    }
                    if (generateData?.Count > 0)
                        _producerClient.SendAsync(generateData).Wait();
                    await Task.CompletedTask;
                }
            }
            catch (Exception exp)
            {
                //Error handling
            }
        }
    }
}
