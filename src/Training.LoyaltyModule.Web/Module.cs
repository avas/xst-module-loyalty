using System;
using System.Linq;
using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Training.LoyaltyModule.Core;
using Training.LoyaltyModule.Core.Services;
using Training.LoyaltyModule.Data.Handlers;
using Training.LoyaltyModule.Data.Repositories;
using Training.LoyaltyModule.Data.Services;
using Training.LoyaltyModule.Xapi.Extensions;
using VirtoCommerce.OrdersModule.Core.Events;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace Training.LoyaltyModule.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<LoyaltyDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("Training.Loyalty") ?? configuration.GetConnectionString("VirtoCommerce");

                options.UseSqlServer(connectionString);
            });

            serviceCollection.AddTransient<ILoyaltyRepository, LoyaltyRepository>();
            serviceCollection.AddTransient<Func<ILoyaltyRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<ILoyaltyRepository>());

            serviceCollection.AddTransient<IPointsOperationService, PointsOperationService>();
            serviceCollection.AddTransient<IPointsOperationSearchService, PointsOperationSearchService>();
            serviceCollection.AddTransient<IUserBalanceService, UserBalanceService>();
            serviceCollection.AddTransient<IUserBalanceSearchService, UserBalanceSearchService>();

            serviceCollection.AddTransient<LoyaltyOrderChangedEventHandler>();

            var graphQlBuilder = serviceCollection.AddGraphQL(options =>
                {
                    options.EnableMetrics = false;
                })
                .AddNewtonsoftJson(deserializerSettings => { }, serializerSettings => { })
                .AddErrorInfoProvider(options =>
                {
                    options.ExposeExtensions = true;
                    options.ExposeExceptionStackTrace = true;
                });

            serviceCollection.AddXLoyalty(graphQlBuilder);
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<LoyaltyDbContext>())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }

            var handlerRegistrar = appBuilder.ApplicationServices.GetRequiredService<IHandlerRegistrar>();
            handlerRegistrar.RegisterHandler<OrderChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetRequiredService<LoyaltyOrderChangedEventHandler>().Handle(message));

            // Register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // Register permissions
            var permissionsRegistrar = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsRegistrar.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions
                .Select(x => new Permission { ModuleId = ModuleInfo.Id, GroupName = "TrainingLoyaltyModule", Name = x })
                .ToArray());
        }

        public void Uninstall()
        {
        }
    }
}
