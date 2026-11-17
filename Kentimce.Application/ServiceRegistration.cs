using FluentValidation;
using Kentimce.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Kentimce.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Application katmanının Assembly'sini  alıyoruz
            var assembly = Assembly.GetExecutingAssembly();

            // 1. MEDIATR KAYDI
            // Bu Assembly içindeki tüm Command/Query Handler'ları bulur ve kaydeder.
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            // 2. AUTOMAPPER KAYDI
            // Bu Assembly içindeki tüm "Profile" sınıflarını (Mapping profilleri) bulur.
            services.AddAutoMapper(assembly);

            // 3. FLUENT VALIDATION KAYDI
            // Bu Assembly içindeki tüm "AbstractValidator" sınıflarını bulur.
            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}