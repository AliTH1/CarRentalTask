using CarRental.Entities.Dtos.Cars;
using FluentValidation;

namespace CarRental.Validators.Cars
{
    public class CreateCarDtoValidator : AbstractValidator<CreateCarDto>
    {
        public CreateCarDtoValidator()
        {
            RuleFor(p => p.ModelYear)
                .NotNull()
                .GreaterThanOrEqualTo(1980).WithMessage("Model year must be greater than 1980")
                .LessThanOrEqualTo(2023).WithMessage("Model year must be less than 2023");
            RuleFor(c => c.DailyPrice)
                .NotNull()
                .GreaterThanOrEqualTo(100)
                .LessThanOrEqualTo(1000);
                
        }
    }
}
