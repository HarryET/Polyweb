using System.Threading.Tasks;

namespace Poly.Web.Interfaces
{
    public interface IApiController
    {
        public Task Get(IRequest request);
        public Task Post(IRequest request);
        public Task Put(IRequest request);
        public Task Patch(IRequest request);
        public Task Delete(IRequest request);
    }
}