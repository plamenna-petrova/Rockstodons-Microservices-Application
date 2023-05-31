using MediatR;

namespace Catalog.API.Application.Contracts
{
    public interface ICommand<out TResponse> : IRequest<TResponse> 
    {

    }
}
