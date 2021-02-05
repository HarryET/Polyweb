using System.Threading.Tasks;
using Poly.Web.Attributes;
using Poly.Web.Interfaces;

namespace Poly.Web.Tests.Controllers
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