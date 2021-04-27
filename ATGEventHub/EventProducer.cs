using ATGEventHub.Library.Interface;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATGEventHub
{
    public class EventProducer
    {

        string connectionString = "Endpoint=sb://atgeventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xW8YxtD/ZFfh+zBwZyttnu0O0utzH+Ginq0fBufDcfo=";
        string eventHubName = "atgeh";
        EventDataBatch generateData;
        List<AuditLogModel> users = new List<AuditLogModel>();
        EventHubProducerClient producerClient;
        public void Init()
        {
            producerClient = new EventHubProducerClient(connectionString, eventHubName);
            users.Add(new AuditLogModel() { UserId = "4", Region = "India" });
            users.Add(new AuditLogModel() { UserId = "5", Region = "USA" });
            users.Add(new AuditLogModel() { UserId = "6", Region = "Australia" });
            //users.Add("1100");
            //users.Add("1200");
        }
        public async Task GenerateEvent()
        {
            try
            {
                // send in batch
                int partitionId = 0;
                var batchOptions = new CreateBatchOptions()
                {
                    //PartitionId = partitionId.ToString(),
                    //MaximumSizeInBytes = 150

                };
                generateData = producerClient.CreateBatchAsync(batchOptions).Result;
                foreach (var eachUser in users)
                {
                    //StringBuilder strBuilder = new StringBuilder();


                    //strBuilder.AppendFormat("User Triggered {0} ", eachUser);
                    var dataAsJson = JsonConvert.SerializeObject(eachUser);

                    var eveData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
                    // All value should be dynamic
                    eveData.Properties.Add("UserId", eachUser.UserId);
                    eveData.Properties.Add("Location", eachUser.Region);
                    //eveData.Properties.Add("DeviceType", eachUser);
                    if (!generateData.TryAdd(eveData))
                    {
                        producerClient.SendAsync(generateData).Wait();
                        batchOptions = new CreateBatchOptions();
                        //{
                        //    PartitionId = partitionId.ToString(),
                        //    MaximumSizeInBytes = 150

                        //};
                        generateData = producerClient.CreateBatchAsync(batchOptions).Result;
                        generateData.TryAdd(eveData);
                    }


                    //Reset partitionId as it can be 0 or 1 as we have define in azure event hub  
                    //partitionId++;
                    //if (partitionId > 1)
                    //    partitionId = 0;
                }
                if (generateData.Count > 0)
                    producerClient.SendAsync(generateData).Wait();
                await Task.CompletedTask;
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error occruied {0}. Try again later", exp.Message);
            }
        }

        public async Task GenerateEvent2()
        {
            IEventSender<AuditLogModel> _eventSender = new Library.Implementation.EventSender<AuditLogModel>(connectionString, eventHubName);
            await _eventSender.GenerateEvent(users);
        }
    }
}
