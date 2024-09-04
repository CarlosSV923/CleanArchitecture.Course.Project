using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using MediatR;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand
    {
        
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {
        
    }

    public interface IBaseCommand {

    } 
}