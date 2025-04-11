
using FluentValidation;
using NetDapperWebApi.DTO.Creates;

namespace NetDapperWebApi.Common.Validators
{
    public class CreateMultipleImageValidator : AbstractValidator<CreateMutipleImage>
    {
        public CreateMultipleImageValidator()
        {
            RuleFor(x => x.EntityType)
                .NotEmpty().WithMessage("EntityType không được để trống.");

            RuleFor(x => x.EntityId)
                .GreaterThan(0).WithMessage("EntityId phải lớn hơn 0.");
            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("Vui lòng tải lên ít nhất một ảnh.");

            RuleFor(x => x.SortOrder)
                .NotEmpty().WithMessage("SortOrder không được để trống.");

            RuleFor(x => x)
                .Must(x => x.Images.Count == x.SortOrder.Count)
                .WithMessage("Số lượng ảnh và SortOrder phải bằng nhau!");

            RuleFor(x => x.SortOrder)
                .Must(list => list.Distinct().Count() == list.Count)
                .WithMessage("SortOrder không được trùng nhau!");
        }
    }

}