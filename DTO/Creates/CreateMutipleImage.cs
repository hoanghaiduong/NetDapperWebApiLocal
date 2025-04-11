

using System.ComponentModel.DataAnnotations;

namespace NetDapperWebApi.DTO.Creates
{
    public class CreateMutipleImage : IValidatableObject
    {
        [Required]
        public string EntityType { get; set; }

        [Required]
        public int EntityId { get; set; }

        [Required(ErrorMessage = "Vui lòng tải lên ít nhất một ảnh.")]
        public List<IFormFile> Images { get; set; } = [];

        [Required(ErrorMessage = "SortOrder không được để trống.")]
        public List<int> SortOrder { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EntityId <= 0)
            {
                yield return new ValidationResult("EntityId không được nhỏ hơn hoặc bằng 0!", [nameof(EntityId)]);
            }

            if (Images.Count != SortOrder.Count)
            {
                yield return new ValidationResult("Số lượng ảnh và số lượng SortOrder phải bằng nhau!", [nameof(Images), nameof(SortOrder)]);
            }
            if (SortOrder.Any(x => x <= 0))
            {
                yield return new ValidationResult("SortOrder không được nhỏ hơn hoặc bằng 0!", [nameof(SortOrder)]);
            }
            if (SortOrder.Distinct().Count() != SortOrder.Count)
            {
                yield return new ValidationResult("SortOrder không được trùng nhau!", [nameof(SortOrder)]);
            }
        }
    }

}
