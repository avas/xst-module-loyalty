using GraphQL.Types;
using Training.LoyaltyModule.Core.Models;

namespace Training.LoyaltyModule.Xapi.Schemas
{
    public class PointsOperationType : ObjectGraphType<PointsOperation>
    {
        public PointsOperationType()
        {
            Field(x => x.Id, nullable: false).Description("ID of the operation");

            Field(x => x.CreatedDate, nullable: false).Description("Creation date");
            Field(x => x.ModifiedDate, nullable: true).Description("Date of last modification");
            Field(x => x.CreatedBy, nullable: true).Description("User name of the creator of this object");
            Field(x => x.ModifiedBy, nullable: true).Description("User name of the person who modified this object last time");

            Field(x => x.UserId, nullable: false).Description("User ID");
            Field(x => x.StoreId, nullable: true).Description("Store ID");
            Field(x => x.Reason, nullable: false).Description("Reason");
            Field(x => x.Amount, nullable: false).Description("Amount");
            Field(x => x.Balance, nullable: false).Description("Balance");
            Field(x => x.IsDeposit, nullable: false).Description("Is deposit");
        }
    }
}
