using System;

namespace Aurora.Mediator;

public interface IValidator<in TRequest>
{
    Task ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
}
