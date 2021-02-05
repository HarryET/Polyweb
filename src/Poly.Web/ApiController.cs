using System.Threading.Tasks;
using Poly.Web.Interfaces;

namespace Poly.Web
{
    public class ApiController : IApiController
    {
        public virtual Task Get(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Post(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Put(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Patch(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Delete(IRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}