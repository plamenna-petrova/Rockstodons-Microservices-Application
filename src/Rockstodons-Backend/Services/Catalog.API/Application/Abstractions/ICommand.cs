using MediatR;

namespace Catalog.API.Application.Abstractions
{
    public interface ICommand<out TResponse> : IRequest<TResponse> 
    {

    }
}
