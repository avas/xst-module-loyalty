using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Xapi.Commands
{
    public class RegisterPointsOperationCommandHandler : IRequestHandler<RegisterPointsOperationCommand, PointsOperation>
    {
        private readonly IPointsOperationService _pointsOperationService;

        public RegisterPointsOperationCommandHandler(IPointsOperationService pointsOperationService)
        {
            _pointsOperationService = pointsOperationService;
        }

        public async Task<PointsOperation> Handle(RegisterPointsOperationCommand request, CancellationToken cancellationToken)
        {
            var pointsOperation = AbstractTypeFactory<PointsOperation>.TryCreateInstance();

            pointsOperation.UserId = request.UserId;
            pointsOperation.StoreId = request.StoreId;
            pointsOperation.Amount = request.Amount;
            pointsOperation.IsDeposit = request.Amount < 0;
            pointsOperation.Reason = request.Reason;

            await _pointsOperationService.SaveChangesAsync(new[] { pointsOperation });

            return pointsOperation;
        }
    }
}
