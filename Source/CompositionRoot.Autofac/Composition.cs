﻿using System;
using Autofac;
using Common.Models;
using Infra.AspNetCoreIdentity;
using Infra.EfCore;
using Microsoft.AspNetCore.Identity;

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
            builder.RegisterType<ProjectRoleStore>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<ClaimFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserClaimsPrincipalFactory<User>>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
