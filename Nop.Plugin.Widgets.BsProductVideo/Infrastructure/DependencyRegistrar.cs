using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.BsProductVideo.Data;
using Nop.Plugin.Widgets.BsProductVideo.Domain;
using Nop.Plugin.Widgets.BsProductVideo.Services;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Infrastructure.Extensions;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.BsProductVideo.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
           const string  context= "nop_object_context_Bs_product_video";
            builder.RegisterType<ProductVideoRecordService>().As<IProductVideoRecordService>().InstancePerLifetimeScope();

            //data context
            builder.RegisterPluginDataContext<ProductVideoObjectContext>( context);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<ProductVideoRecord>>()
                .As<IRepository<ProductVideoRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(context))
                .InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 1; }
        }


       
    }
}
