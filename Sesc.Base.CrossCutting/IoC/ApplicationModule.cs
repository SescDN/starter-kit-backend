using Autofac;
using Autofac.Extras.DynamicProxy;
using FluentValidation;
using Sesc.Base.Domain.Entities;
using Sesc.Base.Domain.Repositories;
using Sesc.Base.Domain.Validations;
using Stefanini.Common;
using Stefanini.Log.Interceptors;
using Sesc.Base.Domain.Business;
using Sesc.Base.Domain.Business.Interfaces;
using Sesc.Base.Application.Services;
using Sesc.Base.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stefanini.Repository.EntityFramework.Interceptors;
using AutoMapper;
using Sesc.Base.Application;
using Sesc.Base.Infrastructure.Data;
using Sesc.Base.Infrastructure.Data.Repositories;

namespace Sesc.Base.CrossCutting.IoC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogInterceptor>();
            builder.RegisterType<TransactionContextInterceptor>();

            builder.Register(c =>
            {
                
                
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                optionsBuilder.UseSqlServer($"Data Source={c.Resolve<EnviromentConfiguration>().GetValue("Database:Host")};Initial Catalog={c.Resolve<EnviromentConfiguration>().GetValue("Database:Database")};User ID={c.Resolve<EnviromentConfiguration>().GetValue("Database:User")};Password={c.Resolve<EnviromentConfiguration>().GetValue("Database:Password")}", providerOptions => providerOptions.CommandTimeout(60))
                              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                return optionsBuilder.Options;
            });

            builder.RegisterType<ApplicationContext>().As<ApplicationContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<EnviromentConfiguration>().SingleInstance();
            builder.RegisterType<Notification>().As<INotification>().InstancePerLifetimeScope();

            // Configuro o auto mapper
            builder.Register(c =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new DomainToViewModelMappingProfile());
                    cfg.AddProfile(new ViewModelToDomainMappingProfile());
                });
                return config.CreateMapper();
            });

            builder.RegisterType<AlunoService>().As<IAlunoService>().EnableInterfaceInterceptors().InterceptedBy(typeof(LogInterceptor), typeof(TransactionContextInterceptor));

            builder.RegisterType<AlunoBusiness>().As<IAlunoBusiness>().EnableInterfaceInterceptors().InterceptedBy(typeof(LogInterceptor));

            builder.RegisterType<AlunoValidation>().As<IValidator<Aluno>>();

            builder.RegisterType<AlunoRepository>().As<IAlunoRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(LogInterceptor));
        }
    }
}
