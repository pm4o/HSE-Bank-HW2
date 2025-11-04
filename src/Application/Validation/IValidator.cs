namespace FinanceTracker.Application.Validation;

public interface IValidator<in TDto>
{
    void Validate(TDto dto);
}