using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Polygon.Attributes;
using Polygon.Exceptions;
using Polygon.Interfaces;

namespace Polygon
{
    public class PolygonServer
    {
        private IDataProvider DataProvider { get; set; }
        private IAuthenticationProvider AuthenticationProvider { get; set; }
        private ILogger Logger { get; set; }
        private Dictionary<Type, IProvider> Providers { get; set; }
        private Dictionary<Type, IService> Services { get; set; }
        private Dictionary<string, IApiController> Controllers { get; set; }
        private bool ServerOnline { get; set; }
        private HttpListener Listener { get; set; }

        public PolygonServer()
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
                    if (Controllers.ContainsKey(ctx.Request.Url.LocalPath))
                    {
                        IApiController controller = Controllers[ctx.Request.Url.LocalPath];
                        IRequest request = new Request(ctx, DataProvider, AuthenticationProvider);
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
                }
            }
        }
        
        public void Listen(int port)
        {
            Listener.Prefixes.Add($"http://localhost:{port}/");
            Listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            Listener.Start();
            
            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            Listener.Close();
        }
    }
}