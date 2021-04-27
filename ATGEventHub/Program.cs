using System;
using System.Threading;
using System.Threading.Tasks;

namespace ATGEventHub
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            Task[] tasks = new Task[1];
            tasks[0] = Task.Run(() => {
                Thread.Sleep(1000);
                //Run the producer
                program.RunProducer();
            });
            //tasks[1] = Task.Run(() =>
            //{
            //    Thread.Sleep(1000);
            //    //Run the event consumer
            //    program.RunEventHubConsumerReadEvent();
            //});
            //tasks[2] = Task.Run(() => {
            //    Thread.Sleep(1000);
            //    //Run the event consumer
            //    program.RunEventHubConsumerReadEventPartitionEvent();
            //});

            Task.WaitAll(tasks);

            Console.WriteLine("Press any any to end program");
            Console.ReadKey();
        }

        public void RunProducer()
        {
            //Run the producer
            EventProducer eventProducer = new EventProducer();
            eventProducer.Init();
            eventProducer.GenerateEvent2().Wait();
            //eventProducer.GenerateEvent().Wait();
        }

        public void RunEventHubConsumerReadEvent()
        {
            //Run the EventHubConsumerClientDemo with different group
            EventHubConsumerClientDemo eventHubConsumer = new EventHubConsumerClientDemo();
            eventHubConsumer.ConsumerReadEvent("$Default").Wait();
        }

        public void RunEventHubConsumerReadEventPartitionEvent()
        {
            //Run the EventHubConsumerClientDemo with different group
            EventHubConsumerClientDemo eventHubConsumer = new EventHubConsumerClientDemo();
            eventHubConsumer.ConsumerReadEventPartitionEvent("$Default", "0").Wait();
        }
    }
}
