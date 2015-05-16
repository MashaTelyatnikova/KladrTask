using System;
using System.Web.Mvc;
using System.Web.Routing;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Concrete;
using KladrTask.WebUI.Infrastructure.Abstract;
using KladrTask.WebUI.Infrastructure.Concrete;
using Ninject;

namespace KladrTask.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            ninjectKernel.Bind<IKladrRepository>().To<DbKladrRepository>();
        }
    }
}