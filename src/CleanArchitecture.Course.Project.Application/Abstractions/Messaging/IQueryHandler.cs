using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using MediatR;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
        // Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}