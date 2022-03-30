using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace Training.LoyaltyModule.Data.Models
{
    public class PointsOperationEntity : AuditableEntity, IDataEntity<PointsOperationEntity, PointsOperation>
    {
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        public string StoreId { get; set; }

        [StringLength(1024)]
        public string Reason { get; set; }

        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "Money")]
        public decimal Balance { get; set; }

        public bool IsDeposit { get; set; }

        public PointsOperation ToModel(PointsOperation model)
        {
            model.Id = Id;

            model.CreatedDate = CreatedDate;
            model.ModifiedDate = ModifiedDate;
            model.CreatedBy = CreatedBy;
            model.ModifiedBy = ModifiedBy;

            model.UserId = UserId;
            model.StoreId = StoreId;
            model.Reason = Reason;
            model.Amount = Amount;
            model.Balance = Balance;
            model.IsDeposit = IsDeposit;

            return model;
        }

        public PointsOperationEntity FromModel(PointsOperation model, PrimaryKeyResolvingMap pkMap)
        {
            pkMap.AddPair(model, this);

            Id = model.Id;

            CreatedDate = model.CreatedDate;
            ModifiedDate = model.ModifiedDate;
            CreatedBy = model.CreatedBy;
            ModifiedBy = model.ModifiedBy;

            UserId = model.UserId;
            StoreId = model.StoreId;
            Reason = model.Reason;
            Amount = model.Amount;
            Balance = model.Balance;
            IsDeposit = model.IsDeposit;

            return this;
        }

        public void Patch(PointsOperationEntity target)
        {
            target.UserId = UserId;
            target.StoreId = StoreId;
            target.Reason = Reason;
            target.Amount = Amount;
            target.Balance = Balance;
            target.IsDeposit = IsDeposit;
        }
    }
}
