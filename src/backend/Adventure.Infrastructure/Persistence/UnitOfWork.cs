using Adventure.Domain.Events;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AdventureDbContext _context;
    private readonly IPublisher _publisher;

    public UnitOfWork(AdventureDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entitiesWithEvents = _context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entitiesWithEvents)
            entity.ClearDomainEvents();

        var result = await _context.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, ct);

        return result;
    }
}
