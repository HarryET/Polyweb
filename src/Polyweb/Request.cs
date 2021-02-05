using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Polyweb.Exceptions;
using Polyweb.Interfaces;

namespace Polyweb
{
    public class Request : IRequest
    {
        private IDataProvider _dataProvider;
        private readonly HttpListenerContext _context;
        private bool _responded;
        private int _status;
        private NameValueCollection _query;
        private NameValueCollection _headers;
        private CookieCollection _cookies;
        private Dictionary<string, string> _params;

        IDataProvider IRequest.DataProvider
        {
            get => _dataProvider;
            set => _dataProvider = value;
        }

        NameValueCollection IRequest.Query
        {
            get => _query;
            set => _query = value;
        }

        NameValueCollection IRequest.Headers
        {
            get => _headers;
            set => _headers = value;
        }

        CookieCollection IRequest.Cookies
        {
            get => _cookies;
            set => _cookies = value;
        }

        Dictionary<string, string> IRequest.Params
        {
            get => _params;
            set => _params = value;
        }

        public Request(HttpListenerContext ctx, IDataProvider dataProvider, IAuthenticationProvider authenticationProvider, Dictionary<string, string> urlParams)
        {
            _dataProvider = dataProvider;
            _context = ctx;
            _query = ctx.Request.QueryString;
            _headers = ctx.Request.Headers;
            _cookies = ctx.Request.Cookies;
            _params = urlParams;
            
            _context.Response.AddHeader("X-Powered-By", "Polyweb");
        }

        public IRequest Status(int status)
        {
            _status = status;
            return this;
        }

        public async Task<IRequest> Json(object json)
        {
            if (!_responded)
            {
                HttpListenerResponse resp = _context.Response;
            
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(json));
                resp.StatusCode = _status;
                resp.ContentType = "application/json";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;
                
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                
                _responded = true;
                
                resp.Close();

                return this;
            }
            else
            {
                throw new ResponseAlreadySent();
            }
        }

        public async Task<IRequest> Text(string text)
        {
            if (!_responded)
            {
                HttpListenerResponse resp = _context.Response;

                byte[] data = Encoding.UTF8.GetBytes(text);
                resp.StatusCode = _status;
                resp.ContentType = "text/plain";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                                
                _responded = true;
                
                resp.Close();

                return this;
            }
            else
            {
                throw new ResponseAlreadySent();
            }
        }
    }
}