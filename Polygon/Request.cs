using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Polygon.Exceptions;
using Polygon.Interfaces;

namespace Polygon
{
    public class Request : IRequest
    {
        private IDataProvider _dataProvider;
        private readonly HttpListenerContext _context;
        private bool _responded;

        IDataProvider IRequest.DataProvider
        {
            get => _dataProvider;
            set => _dataProvider = value;
        }
        
        public Request(HttpListenerContext ctx, IDataProvider dataProvider, IAuthenticationProvider authenticationProvider)
        {
            _dataProvider = dataProvider;
            _context = ctx;
        }
        
        public async Task Json(object json)
        {
            if (!_responded)
            {
                HttpListenerResponse resp = _context.Response;
            
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(json));
                resp.ContentType = "application/json";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;
                
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                
                _responded = true;
                
                resp.Close();
            }
            else
            {
                throw new ResponseAlreadySent();
            }
        }

        public async Task Text(string text)
        {
            if (!_responded)
            {
                HttpListenerResponse resp = _context.Response;

                byte[] data = Encoding.UTF8.GetBytes(text);
                resp.ContentType = "text/plain";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                                
                _responded = true;
                
                resp.Close();
            }
            else
            {
                throw new ResponseAlreadySent();
            }
        }
    }
}