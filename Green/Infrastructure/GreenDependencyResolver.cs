using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Green.Infrastructure
{
    public class GreenDependencyResolver : IDependencyResolver
    {
        private IUnityContainer unityContainer;

        public GreenDependencyResolver(IUnityContainer _unityContainer)
        {
            unityContainer = _unityContainer;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return unityContainer.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return unityContainer.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }
    }
}