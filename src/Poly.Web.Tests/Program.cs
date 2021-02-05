using Poly.Web.Tests.Providers;
using Poly.Web.Tests.Services;

namespace Poly.Web.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            PolywebServer polyweb = new PolywebServer();
            
            polyweb.RegisterProvider<DataProvider>();
            polyweb.RegisterProvider<AuthenticationProvider>();
            
            polyweb.RegisterService<PasswordService, IPasswordService>();
            
            polyweb.RegisterController<Controllers.Index>();
            polyweb.RegisterController<Controllers.Echo>();
            
            polyweb.Listen(8080);
        }
    }
}