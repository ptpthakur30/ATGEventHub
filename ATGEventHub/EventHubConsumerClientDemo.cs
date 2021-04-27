using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATGEventHub
{
    public class EventHubConsumerClientDemo
    {
        string connectionString = "Endpoint=sb://atgeventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xW8YxtD/ZFfh+zBwZyttnu0O0utzH+Ginq0fBufDcfo=";
        string eventHubName = "atgeh";
        //Read all events. No blol container needed directly consumer can use the method to procees event
        public async Task ConsumerReadEvent(string consumerGroup)
        {
            try
            {
                //CancellationTokenSource cancellationSource = new CancellationTokenSource();
                //cancellationSource.CancelAfter(TimeSpan.FromSeconds(300));
                //EventHubConsumerClient eventConsumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName);

                //await foreach (PartitionEvent partitionEvent in eventConsumer.ReadEventsAsync(cancellationSource.Token))
                //{
                //    Console.WriteLine("---Execution from ConsumerReadEvent method---");
                //    Console.WriteLine("------");
                //    Console.WriteLine("Event Data recieved {0} ", Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()));
                //    if (partitionEvent.Data != null)
                //    {
                //        Console.WriteLine("Event Data {0} ", Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()));
                //        if (partitionEvent.Data.Properties != null)
                //        {
                //            foreach (var keyValue in partitionEvent.Data.Properties)
                //            {
                //                Console.WriteLine("Event data key = {0}, Event data value = {1}", keyValue.Key, keyValue.Value);
                //            }
                //        }
                //    }

                //}

                Console.WriteLine("ConsumerReadEvent end");
                await Task.CompletedTask;
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error occruied {0}. Try again later", exp.Message);
            }
        }

        //Read all events based on partitionId
        public async Task ConsumerReadEventPartitionEvent(string consumerGroup, string partitionId)
        {
            try
            {
                CancellationTokenSource cancellationSource = new CancellationTokenSource();
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(30));
                EventHubConsumerClient eventConsumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName);

                ReadEventOptions readEventOptions = new ReadEventOptions()
                {
                    MaximumWaitTime = TimeSpan.FromSeconds(30)
                };

                await foreach (PartitionEvent partitionEvent in eventConsumer.ReadEventsFromPartitionAsync(partitionId, EventPosition.Latest, readEventOptions, cancellationSource.Token))
                {
                    Console.WriteLine("---Execution from ConsumerReadEventPartitionEvent method---");
                    Console.WriteLine("------");
                    if (partitionEvent.Data != null)
                    {
                        Console.WriteLine("Event Data recieved {0} ", Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()));
                        if (partitionEvent.Data.Properties != null)
                        {
                            foreach (var keyValue in partitionEvent.Data.Properties)
                            {
                                Console.WriteLine("Event data key = {0}, Event data value = {1}", keyValue.Key, keyValue.Value);
                            }
                        }
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error occruied {0}. Try again later", exp.Message);
            }

        }
    }
}
