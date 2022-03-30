using GraphQL.Types;
using Training.LoyaltyModule.Core.Models;

namespace Training.LoyaltyModule.Xapi.Schemas
{
    public class UserBalanceType : ObjectGraphType<UserBalance>
    {
        public UserBalanceType()
        {
            Field(x => x.Id, nullable: false).Description("ID of the balance");

            Field(x => x.CreatedDate, nullable: false).Description("Creation date");
            Field(x => x.ModifiedDate, nullable: true).Description("Date of last modification");
            Field(x => x.CreatedBy, nullable: true).Description("User name of the creator of this object");
            Field(x => x.ModifiedBy, nullable: true).Description("User name of the person who modified this object last time");

            Field(x => x.UserId, nullable: false).Description("User ID");
            Field(x => x.StoreId, nullable: true).Description("Store ID");
            Field(x => x.Amount, nullable: true).Description("Amount");
            Field<ListGraphType<PointsOperationType>>("operations", resolve: context => context.Source.Operations);
        }
    }
}
