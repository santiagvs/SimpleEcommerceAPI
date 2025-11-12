using FluentValidation;
using Mediator;

namespace SimpleEcommerce.Application.Behaviors;

public sealed class ValidationBehavior<TMessage,TResponse>(
    IEnumerable<IValidator<TMessage>> validators
) : IPipelineBehavior<TMessage,TResponse>
    where TMessage : notnull, IMessage
{
    private readonly IEnumerable<IValidator<TMessage>> _validators = validators;

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage,TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TMessage>(message);
            var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
                throw new Exception(string.Join("; ", failures.Select(f => f.ErrorMessage)));
        }

        return await next(message, cancellationToken);
    }
}