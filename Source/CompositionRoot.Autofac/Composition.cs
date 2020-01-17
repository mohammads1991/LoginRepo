using System;
using Autofac;
using Infra.AspNetCoreIdentity;

namespace CompositionRoot.Autofac
{
    public static class Composition

    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectUserStore>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }
    }
}
