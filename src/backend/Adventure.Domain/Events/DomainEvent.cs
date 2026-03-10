using MediatR;

namespace Adventure.Domain.Events;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
    }
}
