using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Polyweb.Attributes;
using Polyweb.Interfaces;

namespace Polyweb.Tests.Controllers
{
    [Route("/")]
    public class Index : ApiController
    {
        public override async Task Get(IRequest request)
        {
            await request.Status(200).Json(new
            {
                A = request.Query["a"] ?? "Query A Not Included."
            });
        }
    }
}