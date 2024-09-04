using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using MediatR;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
        
    }
}
