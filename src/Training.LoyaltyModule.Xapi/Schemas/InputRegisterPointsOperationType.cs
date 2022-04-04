using GraphQL.Types;

namespace Training.LoyaltyModule.Xapi.Schemas
{
    public class InputRegisterPointsOperationType : InputObjectGraphType
    {
        public InputRegisterPointsOperationType()
        {
            Field<NonNullGraphType<StringGraphType>>("userId", "User ID");
            Field<StringGraphType>("storeId", "Store ID");
            Field<NonNullGraphType<StringGraphType>>("reason", "Description of operation");
            Field<NonNullGraphType<DecimalGraphType>>("amount", "Amount of points to deposit or grant");
        }
    }
}
