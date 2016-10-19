using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effort;
using Effort.DataLoaders;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Managers;
using Its.Systems.HR.Infrastructure;
using Its.Systems.HR.Infrastructure.Repository;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Its.Systems.HR.Test
{
    public class BaseTest
    {
        public static IUnityContainer _ambientContainer;
        public static IServiceLocator _ambientLocator;

        public IDbRepository db;
        public BaseTest()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sv-SE");

            ClassInitialize();
        }

        public static IUnityContainer Container()
        {
            return _ambientContainer;
        }

        public static void ClassInitialize()
        {

            _ambientContainer = new UnityContainer();
            ConfigureApplication(_ambientContainer);
            _ambientLocator = new UnityServiceLocator(_ambientContainer);
            ServiceLocator.SetLocatorProvider(() => _ambientLocator);
            foreach (var listener in Trace.Listeners)
            {
                var traceListener = (TraceListener)listener;
                traceListener.TraceOutputOptions = traceListener.TraceOutputOptions | TraceOptions.DateTime;
            }
        }

        private static void ConfigureApplication(IUnityContainer container)
        {
            
            //_ambientContainer.RegisterType<IDbRepository, DbRepository>(new PerResolveLifetimeManager());

            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("UseEffort")) == true)
            {
                IDataLoader loader =
                    new Effort.DataLoaders.CsvDataLoader(AppDomain.CurrentDomain.BaseDirectory + "\\Seed");
                _ambientContainer.RegisterType<HRContext, HRContext>(new PerResolveLifetimeManager(),
                    new InjectionConstructor(DbConnectionFactory.CreateTransient(loader)));
            }
            else
            {
                _ambientContainer.RegisterType<HRContext, HRContext>(new PerResolveLifetimeManager(),
                    new InjectionConstructor());
            }
            _ambientContainer.RegisterType<IDbRepository, DbRepository>(new PerResolveLifetimeManager());
            _ambientContainer.RegisterType<IActivityManager, ActivityManager>(new PerResolveLifetimeManager());
            _ambientContainer.RegisterType<IPersonManager, PersonManager>(new PerResolveLifetimeManager());
            _ambientContainer.RegisterType<ISessionManager, SessionManager>(new PerResolveLifetimeManager());
            _ambientContainer.RegisterType<IUtilityManager, UtilityManager>(new PerResolveLifetimeManager());

        }
    }
}
