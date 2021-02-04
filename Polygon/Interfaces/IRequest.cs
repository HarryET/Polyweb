using System.Net;
using System.Threading.Tasks;

namespace Polygon.Interfaces
{
    public interface IRequest
    {
        public IDataProvider DataProvider { get; protected set; }

        public Task Json(object json);
        public Task Text(string text);
    }
}