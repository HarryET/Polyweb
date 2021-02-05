using Polyweb.Tests.Providers;
using Polyweb.Tests.Services;

namespace Polyweb.Tests
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