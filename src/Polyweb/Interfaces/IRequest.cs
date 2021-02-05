using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

namespace Polyweb.Interfaces
{
    public interface IRequest
    {
        public IDataProvider DataProvider { get; protected set; }
        public NameValueCollection Query { get; protected set; }
        public NameValueCollection Headers { get; protected set; }
        public CookieCollection Cookies { get; protected set; }
        public Dictionary<string, string> Params { get; protected set; }
        public IRequest Status(int status);
        public Task<IRequest> Json(object json);
        public Task<IRequest> Text(string text);
    }
}