using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Core.Services;
using Training.OrdersModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Events;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace Training.LoyaltyModule.Data.Handlers
{
    public class LoyaltyOrderChangedEventHandler : IEventHandler<OrderChangedEvent>
    {
        private const string _completedOrderStatus = "Completed";

        private readonly IPointsOperationService _pointsOperationService;
        private readonly ICrudService<CustomerOrder> _customerOrderService;

        public LoyaltyOrderChangedEventHandler(IPointsOperationService pointsOperationService, ICrudService<CustomerOrder> customerOrderService)
        {
            _pointsOperationService = pointsOperationService;
            _customerOrderService = customerOrderService;
        }

        public virtual Task Handle(OrderChangedEvent message)
        {
            var orderIds = message.ChangedEntries
                .Where(x => OrderIsEligibleForPointsOperation(x.NewEntry))
                .Select(x => x.NewEntry.Id)
                .ToList();

            if (orderIds.Any())
            {
                BackgroundJob.Enqueue(() => CreatePointsOperationsForOrders(orderIds));
            }

            return Task.CompletedTask;
        }

        public virtual async Task CreatePointsOperationsForOrders(IList<string> orderIds)
        {
            var orders = await _customerOrderService.GetAsync(orderIds.ToList());

            var pointsOperations = new List<PointsOperation>();

            foreach (var order in orders.OfType<TrainingOrder>())
            {
                var pointsOperation = BuildPointsOperationForOrder(order);
                pointsOperations.Add(pointsOperation);

                order.LoyaltyCalculated = true;
            }

            await _pointsOperationService.SaveChangesAsync(pointsOperations);
            await _customerOrderService.SaveChangesAsync(orders);
        }


        protected virtual bool OrderIsEligibleForPointsOperation(CustomerOrder order)
        {
            return order.Status.EqualsInvariant(_completedOrderStatus) &&
                   order is TrainingOrder { LoyaltyCalculated: false };
        }

        protected virtual PointsOperation BuildPointsOperationForOrder(CustomerOrder order)
        {
            var result = AbstractTypeFactory<PointsOperation>.TryCreateInstance();

            result.UserId = order.CustomerId;
            result.StoreId = order.StoreId;
            result.IsDeposit = false;
            result.Amount = order.Total;
            result.Reason = $"Completion of order {order.Number}";

            return result;
        }
    }
}
