using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Builders;
using GraphQL.Resolvers;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Xapi.Commands;
using Training.LoyaltyModule.Xapi.Queries;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;

namespace Training.LoyaltyModule.Xapi.Schemas
{
    public class LoyaltySchema : ISchemaBuilder
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediator _mediator;

        public LoyaltySchema(IAuthorizationService authorizationService, IMediator mediator)
        {
            _authorizationService = authorizationService;
            _mediator = mediator;
        }

        // TODO: uncomment authorization checks in this class after the testing (i.e. when there will be a way to authenticate the customer)

        public void Build(ISchema schema)
        {
            var userBalanceField = new FieldType
            {
                Name = "userBalance",
                Arguments = new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userId" },
                    new QueryArgument<StringGraphType> { Name = "storeId" },
                    new QueryArgument<BooleanGraphType> { Name = "includeOperations" }
                ),
                Type = GraphTypeExtenstionHelper.GetActualType<UserBalanceType>(),
                Resolver = new AsyncFieldResolver<object>(async context =>
                {
                    var request = new GetUserBalanceQuery
                    {
                        UserId = context.GetArgument<string>("userId"),
                        StoreId = context.GetArgument<string>("storeId"),
                        IncludeOperations = context.GetArgument<bool>("includeArguments"),
                    };

                    //var authorizationResult = await _authorizationService.AuthorizeAsync(context.GetCurrentPrincipal(), request, new CanReadLoyaltyDataAuthorizationRequirement());
                    //if (!authorizationResult.Succeeded)
                    //{
                    //    throw new AuthorizationError("Balance of requested user is not available for current user.");
                    //}

                    var result = await _mediator.Send(request);

                    return result.UserBalance;
                })
            };
            schema.Query.AddField(userBalanceField);

            var pointsOperationConnectionBuilder = GraphTypeExtenstionHelper
                .CreateConnection<PointsOperationType, object>()
                .Name("pointsOperations")
                .Argument<NonNullGraphType<StringGraphType>>("userId", "User ID")
                .Argument<StringGraphType>("storeId", "Store ID")
                .Argument<BooleanGraphType>("isDeposit", "Is deposit")
                .Argument<DateTimeGraphType>("createdSince", "Created since")
                .Argument<DateTimeGraphType>("createdTill", "Created till")
                .Argument<StringGraphType>("sort", "Sort expression")
                .PageSize(20);

            pointsOperationConnectionBuilder.ResolveAsync(async context => await ResolvePointsOperationConnectionAsync(_mediator, context));

            schema.Query.AddField(pointsOperationConnectionBuilder.FieldType);

            var registerPointsOperationFieldBuilder = FieldBuilder.Create<object, PointsOperation>(GraphTypeExtenstionHelper.GetActualType<PointsOperationType>())
                .Name("registerPointsOperation")
                .Argument(GraphTypeExtenstionHelper.GetActualComplexType<NonNullGraphType<InputRegisterPointsOperationType>>(), "command")
                .ResolveAsync(async context =>
                {
                    var type = GenericTypeHelper.GetActualType<RegisterPointsOperationCommand>();
                    var command = context.GetArgument(type, "command");

                    //var authorizationResult = await _authorizationService.AuthorizeAsync(context.GetCurrentPrincipal(), command, new CanRegisterPointsOperationsAuthorizationRequirement());
                    //if (!authorizationResult.Succeeded)
                    //{
                    //    throw new AuthorizationError("Current user can't register points operations.");
                    //}

                    var response = (PointsOperation)await _mediator.Send(command);

                    return response;
                });
            schema.Mutation.AddField(registerPointsOperationFieldBuilder.FieldType);
        }

        private async Task<object> ResolvePointsOperationConnectionAsync(IMediator mediator, IResolveConnectionContext<object> context)
        {
            var query = new SearchPointsOperationsQuery
            {
                UserId = context.GetArgument<string>("userId"),
                StoreId = context.GetArgument<string>("storeId"),
                IsDeposit = context.GetArgument<bool?>("isDeposit"),
                CreatedSince = context.GetArgument<DateTime?>("createdSince"),
                CreatedTill = context.GetArgument<DateTime?>("createdTill"),
                Sort = context.GetArgument<string>("sort"),
                Skip = Convert.ToInt32(context.After ?? 0.ToString()),
                Take = context.First ?? context.PageSize ?? 20,
            };

            //var authorizationResult = await _authorizationService.AuthorizeAsync(context.GetCurrentPrincipal(), query, new CanReadLoyaltyDataAuthorizationRequirement());
            //if (!authorizationResult.Succeeded)
            //{
            //    throw new AuthorizationError("Points operations of requested user are not available for current user.");
            //}

            var response = await mediator.Send(query);

            return new PagedConnection<PointsOperation>(response.Results, query.Skip, query.Take, response.TotalCount);
        }
    }
}
