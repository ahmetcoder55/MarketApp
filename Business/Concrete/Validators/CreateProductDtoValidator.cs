using Core.Entities.Concrete.DTOs;
using FluentValidation;

namespace Business.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş geçilemez!")
                .MinimumLength(2).WithMessage("Ürün adı en az 2 karakter olmalıdır!")
                .MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olabilir!");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır!");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı 0'dan küçük olamaz!");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir!");
        }
    }
}