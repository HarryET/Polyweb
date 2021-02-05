using Poly.Web.Interfaces;

namespace Poly.Web.Tests.Services
{
    public interface IPasswordService : IService
    {
        public string Hash(string password, int iterations);
        public string Hash(string password);
        public bool IsHashSupported(string hashString);
        public bool Verify(string password, string hashedPassword);
    }
}