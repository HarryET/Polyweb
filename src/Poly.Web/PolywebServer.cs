using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Poly.Web.Attributes;
using Poly.Web.Exceptions;
using Poly.Web.Interfaces;

namespace Poly.Web
{
    public class PolywebServer
    {
        private IDataProvider DataProvider { get; set; }
        private IAuthenticationProvider AuthenticationProvider { get; set; }
        private ILogger Logger { get; set; }
        private Dictionary<Type, IProvider> Providers { get; set; }
        private Dictionary<Type, IService> Services { get; set; }
        private Dictionary<string, IApiController> Controllers { get; set; }
        private bool ServerOnline { get; set; }
        private HttpListener Listener { get; set; }

        public PolywebServer()
        {
            /* Init Logging */
            Logger = new Logger();
            Logger.Info("Initialising.");
            
            /* Init */
            Providers = new Dictionary<Type, IProvider>();
            Services = new Dictionary<Type, IService>();
            Controllers = new Dictionary<string, IApiController>();
            Listener = new HttpListener();
        }

        public void SetLogger(ILogger logger)
        {
            Logger.Info($"Logger changing to {logger.GetType().Name}.");
            Logger = logger;
            Logger.Info($"Logger changed to {logger.GetType().Name}.");
        }
        
        private void AddNewServiceRegistration<T>(Type service, Type interfaceType)
        {
            if (Services.ContainsKey(service))
            {
                throw new ServiceWithTypeAlreadyRegistered();
            }
            
            ConstructorInfo[] constructors = service.GetConstructors();

            if (constructors.Length != 1)
            {
                throw new TooManyConstructors();
            }

            ConstructorInfo constructor = constructors[0];
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo el = parameters[i];
                if (!Services.ContainsKey(el.ParameterType))
                {
                    throw new ServiceNotRegisteredException();
                }

                args[i] = Services[el.ParameterType];
            }
            
            Services.Add(interfaceType, (IService) Activator.CreateInstance(service, args));
            
            Logger.Info($"Registered Service: {service.Name}.");
        }
        
        public void RegisterService<T>() => AddNewServiceRegistration<T>(typeof(T), typeof(T));
        public void RegisterService<T, I>() => AddNewServiceRegistration<T>(typeof(T), typeof(I));
        private void AddNewProviderRegistration<T>(T provider)
        {
            Type providerType = provider.GetType();
            if (providerType == typeof(IDataProvider))
            {
                if (DataProvider != null)
                {
                    throw new DataProviderAlreadySet();
                }
                DataProvider = (IDataProvider) provider;
                Logger.Info($"Set Data Provider To {providerType.Name}.");
                return;
            }
            
            if (providerType == typeof(IAuthenticationProvider))
            {
                if (AuthenticationProvider != null)
                {
                    throw new AuthenticationProviderAlreadySet();
                }
                AuthenticationProvider = (IAuthenticationProvider) provider;
                Logger.Info($"Set Authentication Provider To {providerType.Name}.");
                return;
            }
            if (Providers.ContainsKey(providerType))
            {
                throw new ProviderWithTypeAlreadyRegistered();
            }
            
            Providers.Add(providerType, (IProvider) provider);
            Logger.Info($"Registered Provider: {providerType.Name}.");
        }
        public void RegisterProvider<T>()
        {
            T provider = Activator.CreateInstance<T>();
            AddNewProviderRegistration<T>(provider);
        }
        
        public void RegisterProvider(IProvider provider)
        {
            AddNewProviderRegistration(provider);
        }

        public void RegisterController<T>()
        {
            Route routeData = (Route) Attribute.GetCustomAttribute(typeof(T), typeof(Route));

            if (routeData == null)
            {
                throw new MissingRouteAttribute();
            }

            if (Controllers.ContainsKey(routeData.HttpRoute))
            {
                throw new ControllerAlreadyExistsWithRoute();
            }

            Type t = typeof(T);
            ConstructorInfo[] constructors = t.GetConstructors();

            if (constructors.Length != 1)
            {
                throw new TooManyConstructors();
            }

            ConstructorInfo constructor = constructors[0];
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo el = parameters[i];
                args[i] = Activator.CreateInstance(el.ParameterType);
            }
            
            Controllers.Add(routeData.HttpRoute, (IApiController) Activator.CreateInstance(t, args));
            Logger.Info($"Registered Controller For Route {routeData.HttpRoute}.");
        }
        
        public async Task HandleIncomingConnections()
        {
            ServerOnline = true;
            while (ServerOnline)
            {
                HttpListenerContext ctx = await Listener.GetContextAsync();
                if (!String.IsNullOrEmpty(ctx.Request.Url?.LocalPath))
                {
                    if (ctx.Request.Url.LocalPath == "/")
                    {
                        HandleIndex(ctx);
                        return;
                    }
                    
                    Dictionary<string, string> urlParams;
                    bool requestHandled = false;
                    
                    if (ctx.Request.Url?.LocalPath == "/")
                    {
                        if (Controllers.ContainsKey("/"))
                        {
                            requestHandled = true;
                            HandleRequest(ctx, new List<string>() { "/" }, new Dictionary<string, string>() {});
                        }
                    }
                    
                    string[] splitUrl = ctx.Request.Url?.LocalPath.TrimStart('/').TrimEnd('/').Split("/");
                    Dictionary<string,IApiController>.KeyCollection keys = Controllers.Keys;
                    
                    foreach (var key in keys)
                    {
                        List<string> el = key.Split("/").Where(x => !string.IsNullOrEmpty(x)).ToList();
                        bool isEmpty = !el.Any();
                        
                        urlParams = new Dictionary<string, string>();
                        
                        if (!isEmpty)
                        {
                            for (int i = 0; i < el.Count; i++)
                            {
                                if (i > splitUrl.Length)
                                {
                                    return;
                                }
                                
                                if (el[i] == splitUrl[i])
                                {
                                    if (i == splitUrl.Length - 1)
                                    {
                                        requestHandled = true;
                                        HandleRequest(ctx, el, urlParams);
                                        break;
                                    }
                                } 
                                else if (el[i][0] == ':')
                                {
                                    urlParams.Add(el[i].TrimStart(':'), splitUrl[i]);
                                    if (i == splitUrl.Length - 1)
                                    {
                                        requestHandled = true;
                                        HandleRequest(ctx, el, urlParams);
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (!requestHandled)
                    {
                        HttpListenerResponse resp = ctx.Response;
            
                        byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
                        {
                            Error = "404, Endpoint Not Found",
                            Status = 404
                        }));
                        
                        resp.StatusCode = 404;
                        resp.ContentType = "application/json";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;
                
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);

                        resp.Close();
                    }
                }
            }
        }

        private async void HandleIndex(HttpListenerContext ctx)
        {
            string url = "/";

            IRequest request = new Request(ctx, DataProvider, AuthenticationProvider, new Dictionary<string, string>() {});

            if (!Controllers.ContainsKey("/"))
            {
                HttpListenerResponse resp = ctx.Response;
            
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
                {
                    Error = "404, Index Not Found",
                    Status = 404
                }));
                        
                resp.StatusCode = 404;
                resp.ContentType = "application/json";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;
                
                await resp.OutputStream.WriteAsync(data, 0, data.Length);

                resp.Close();
                
                return;
            }

            IApiController controller = Controllers["/"];
            
            switch (ctx.Request.HttpMethod.ToUpper())
            {
                case "GET":
                {
                    controller.Get(request);
                    break;
                }
                case "POST":
                {
                    controller.Post(request);
                    break;
                }
                case "PUT":
                {
                    controller.Put(request);
                    break;
                }
                case "PATCH":
                {
                    controller.Patch(request);
                    break;
                }
                case "DELETE":
                {
                    controller.Delete(request);
                    break;
                }
            }
        }
        
        private void HandleRequest(HttpListenerContext ctx, List<string> el, Dictionary<string, string> urlParams)
        {
            string url = "/";

            IRequest request = new Request(ctx, DataProvider, AuthenticationProvider, urlParams);
            
            string[] splitUrl = ctx.Request.Url?.LocalPath.TrimStart('/').TrimEnd('/').Split("/");
            
            for (int i = 0; i < el.Count; i++)
            {
                if (el[i].StartsWith(":"))
                {
                    url += $":{el[i].TrimStart(':')}/";
                }
                else
                {
                    url += $"{splitUrl[i]}/";
                }
            }
            
            IApiController controller = Controllers[url.TrimEnd('/')];
            
            switch (ctx.Request.HttpMethod.ToUpper())
            {
                case "GET":
                {
                    controller.Get(request);
                    break;
                }
                case "POST":
                {
                    controller.Post(request);
                    break;
                }
                case "PUT":
                {
                    controller.Put(request);
                    break;
                }
                case "PATCH":
                {
                    controller.Patch(request);
                    break;
                }
                case "DELETE":
                {
                    controller.Delete(request);
                    break;
                }
            }
        }
        
        public void Listen(int port)
        {
            Listener.Prefixes.Add($"http://localhost:{port}/");
            Listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            Listener.Start();
            
            Logger.Info("Server Online.");
            
            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            Listener.Close();
        }
    }
}