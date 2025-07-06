using FluentValidation;

public class WeatherRequestValidator : AbstractValidator<Location>
{
    public WeatherRequestValidator()
    {
        //validations for city
        RuleFor(x => x.City).NotEmpty().WithMessage("Please provide city.");
        RuleFor(x => x.City).Matches("^[a-zA-Z ]+$").WithMessage("Please provide valid city. City must contain only letters and spaces.");

        //validations for country
        RuleFor(x => x.Country).NotEmpty().WithMessage("Please provide country.");
        RuleFor(x => x.Country).Length(2).WithMessage("Please provide 2 character country code. Example 'au' for Australia.");
        RuleFor(x => x.Country).Matches("^[a-zA-Z]+$").WithMessage("Please provide a valid country code.");
    }
}


/// <summary>
/// Registering validator with ASP.NET pipeline
/// </summary>
public static class ValidationRegistrationExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Location>, WeatherRequestValidator>();
        return services;
    }
}