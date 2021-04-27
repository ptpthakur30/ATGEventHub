using Azure.Messaging.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATGEventHub.Library
{
    public class EventSender
    {
        //private EventHubClient _eventClient;

        //public EventSender(string connectionString)
        //{

        //}

        //public async Task SendDataAsync(string data)
        //{
        //    var eventData = CreateEventData(data);
        //    await _eventClient.SendAsync(eventData);

        //}


        //public async Task SendDataAsync(IEnumerable<string> datas)
        //{
        //    var eventDatas = datas.Select(auditdata => CreateEventData(auditdata));
        //    await _eventClient.SendAsync(eventDatas);

        //}

        //private static EventData CreateEventData(string auditdata)
        //{
        //    var dataAsJson = JsonConvert.SerializeObject(auditdata);
        //    var eventData = Encoding.UTF8.GetBytes(dataAsJson);
        //    return eventData;
        //}
    }
}
