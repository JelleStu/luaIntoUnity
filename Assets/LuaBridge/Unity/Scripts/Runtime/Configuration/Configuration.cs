using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaBridge.Core.Configuration
{
    public class AppConfiguration
    {
        private struct ServiceDefinition
        {
            public Type AbstractType { get; set; }
            public Type ConcreteType { get; set; }
            public object[] Injections { get; set; }
        }

        public class UnResolvedDependencyException : Exception
        {
            public UnResolvedDependencyException(string message) : base(message)
            {
            }
        }

        private Dictionary<Type, ServiceDefinition> _services;
        private Dictionary<Type, object> _concretes;

        public AppConfiguration()
        {
            _services = new Dictionary<Type, ServiceDefinition>();
            _concretes = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Add a singleton service
        /// Dependencies are resolved automatically!
        /// Takes the first constructor with the most valid arguments
        /// Injects other services first, then the objects defined in the injections parameter
        /// Example: MyService(IFooService fooService, IBarService barService, string myString) will take the first 2 services from previously registered singletons and the myString variable form the injections[] 
        /// </summary>
        /// <param name="injections">additional injections, IN ORDER AS DEFINED IN THE CONSTRUCTOR</param>
        /// <typeparam name="TAbstract">abstract service type to search by</typeparam>
        /// <typeparam name="TConcrete">concrete service type to instantiate</typeparam>
        /// <returns>AppConfiguration</returns>
        public AppConfiguration AddSingleton<TAbstract, TConcrete>(params object[] injections)
            where TConcrete : class, TAbstract
        {
            _services.Add(typeof(TAbstract), new ServiceDefinition
            {
                AbstractType = typeof(TAbstract),
                ConcreteType = typeof(TConcrete),
                Injections = injections,
            });
            return this;
        }

        /// <summary>
        /// Add a singleton service
        /// Dependencies are resolved automatically!
        /// Takes the first constructor with the most valid arguments
        /// Injects other services first, then the objects defined in the injections parameter
        /// Example: MyService(IFooService fooService, IBarService barService, string myString) will take the first 2 services from previously registered singletons and the myString variable form the injections[] 
        /// </summary>
        /// <param name="injections">additional injections, IN ORDER AS DEFINED IN THE CONSTRUCTOR</param>
        /// <typeparam name="TConcrete">concrete service type to instantiate</typeparam>
        /// <returns>AppConfiguration</returns>
        public AppConfiguration AddSingleton<TConcrete>(params object[] injections)
            where TConcrete : class
        {
            _services.Add(typeof(TConcrete), new ServiceDefinition
            {
                AbstractType = typeof(TConcrete),
                ConcreteType = typeof(TConcrete),
                Injections = injections,
            });
            return this;
        }

        /// <summary>
        /// Add a singleton service
        /// </summary>
        /// <param name="singleton">the concrete, instantiated singleton to inject</param>
        /// <returns>AppConfiguration</returns>
        public AppConfiguration AddSingleton(object singleton)
        {
            _concretes.Add(singleton.GetType(), singleton);
            return this;
        }

        /// <summary>
        /// Build the configuration
        /// </summary>
        /// <returns>AppContainer with all services instantiated</returns>
        /// <exception cref="UnResolvedDependencyException">Exceptions with details of an unresolved dependency</exception>
        public AppContainer Build()
        {
            Events.EventBus.Factory.Create();
            var looping = true;
            do
            {
                var before = _services.Count;
                foreach (var serviceDef in _services.Values.OrderBy(s => s.ConcreteType.GetConstructors().Length))
                {
                    object instance = null;
                    foreach (var constructor in serviceDef.ConcreteType.GetConstructors().OrderByDescending(c => c.GetParameters().Length))
                    {
                        try
                        {
                            var deps = constructor.GetParameters().Select(parameter =>
                            {
                                if (_concretes.TryGetValue(parameter.ParameterType, out var dep))
                                    return dep;
                                return null;
                            }).Where(d => d != null).Concat(serviceDef.Injections).ToArray();
                            instance = serviceDef.ConcreteType.GetConstructor(deps.Select(t => t.GetType()).ToArray()).Invoke(deps);
                        }
                        catch (Exception)
                        {
                        }

                        if (instance != null)
                            break;
                    }

                    if (instance == null)
                    {
                        continue;
                    }

                    _concretes.Add(serviceDef.AbstractType, instance);
                    _services.Remove(serviceDef.AbstractType);
                }

                looping = before != _services.Count;
            } while (looping);

            if (_services.Count > 0)
            {
                var report = new StringBuilder();
                foreach (var s in _services.Values)
                    report.AppendLine($"{s.AbstractType} -> {s.ConcreteType}");
                throw new UnResolvedDependencyException(
                    $"Failed to resolve service dependencies!{Environment.NewLine}Please make sure you inject the dependencies defined in the constructor and order additional injections properly!{Environment.NewLine}Also: Cyclic dependencies are NOT supported!{Environment.NewLine}See Report Below:{Environment.NewLine}{report}");
            }

            return new AppContainer(_concretes);
        }
    }
}