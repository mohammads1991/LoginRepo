using System;
using Autofac;
using Infra.AspNetCoreIdentity;
using Infra.EfCore;

namespace CompositionRoot.Autofac
{
    public static class Composition

    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectUserStore>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Register(r => ProjectDbContextFactory.CreateDbContext())
                .As<ProjectDbContext>()
                .InstancePerLifetimeScope();
        }
    }
}
