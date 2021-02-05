using System;

namespace Poly.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class Route : Attribute
    {
        public string HttpRoute { get; set; }
        
        public Route(string route)
        {
            HttpRoute = route;
        }
        
    }
}