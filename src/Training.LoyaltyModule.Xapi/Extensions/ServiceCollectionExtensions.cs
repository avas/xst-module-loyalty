using AutoMapper;
using GraphQL.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Training.LoyaltyModule.Xapi.Authorization;
using Training.LoyaltyModule.Xapi.Schemas;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;

namespace Training.LoyaltyModule.Xapi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddXLoyalty(this IServiceCollection serviceCollection, IGraphQLBuilder graphQlBuilder)
        {
            serviceCollection.AddSchemaBuilder<LoyaltySchema>();

            graphQlBuilder.AddGraphTypes(typeof(XLoyaltyAnchor));
            serviceCollection.AddMediatR(typeof(XLoyaltyAnchor));
            serviceCollection.AddAutoMapper(typeof(XLoyaltyAnchor));

            serviceCollection.AddSingleton<IAuthorizationHandler, CanReadLoyaltyDataAuthorizationHandler>();
            serviceCollection.AddSingleton<IAuthorizationHandler, CanRegisterPointsOperationsAuthorizationHandler>();

            return serviceCollection;
        }
    }
}
