using System.Threading.Tasks;
using Poly.Web.Attributes;
using Poly.Web.Interfaces;

namespace Poly.Web.Tests.Controllers
{
    [Route("/echo/:name/:gender/:location")]
    public class Echo : ApiController
    {
        public override async Task Get(IRequest request)
        {
            await request.Status(200).Json(new
            {
                Name = request.Params["name"],
                Gender = request.Params["gender"],
                Location = request.Params["location"]
            });
        }
    }
}