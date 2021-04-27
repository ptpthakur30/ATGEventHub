using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATGEventHub.Library.Interface
{
    public interface IEventSender<T> where T : class
    {
        Task GenerateEvent(IEnumerable<T> dataList);
    }
}