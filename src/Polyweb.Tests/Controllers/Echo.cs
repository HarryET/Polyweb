using System.Threading.Tasks;
using Polyweb.Attributes;
using Polyweb.Interfaces;

namespace Polyweb.Tests.Controllers
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