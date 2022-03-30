using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace Training.LoyaltyModule.Data.Models
{
    public class UserBalanceEntity : AuditableEntity, IDataEntity<UserBalanceEntity, UserBalance>
    {
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        public string StoreId { get; set; }

        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        public UserBalance ToModel(UserBalance model)
        {
            model.Id = Id;

            model.CreatedDate = CreatedDate;
            model.ModifiedDate = ModifiedDate;
            model.CreatedBy = CreatedBy;
            model.ModifiedBy = ModifiedBy;

            model.UserId = UserId;
            model.StoreId = StoreId;
            model.Amount = Amount;

            return model;
        }

        public UserBalanceEntity FromModel(UserBalance model, PrimaryKeyResolvingMap pkMap)
        {
            pkMap.AddPair(model, this);

            Id = model.Id;

            CreatedDate = model.CreatedDate;
            ModifiedDate = model.ModifiedDate;
            CreatedBy = model.CreatedBy;
            ModifiedBy = model.ModifiedBy;

            UserId = model.UserId;
            StoreId = model.StoreId;
            Amount = model.Amount;

            return this;
        }

        public void Patch(UserBalanceEntity target)
        {
            target.UserId = UserId;
            target.StoreId = StoreId;
            target.Amount = Amount;
        }
    }
}
