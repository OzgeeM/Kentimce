using FluentValidation;
using MediatR;


namespace Kentimce.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. Önce bu request için yazılmış bir Validator var mı diye bak
            if (!_validators.Any())
            {
                return await next();
            }

            // 2. Context oluştur
            var context = new ValidationContext<TRequest>(request);

            // 3. Tüm validasyonları çalıştır
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            // 4. Hata varsa "ValidationException" fırlat (Bunu birazdan yakalayacağız)
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            // 5. Hata yoksa yola devam et
            return await next();
        }
    }
}