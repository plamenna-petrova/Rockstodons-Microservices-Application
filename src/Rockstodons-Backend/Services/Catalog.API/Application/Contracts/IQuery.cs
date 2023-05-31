using MediatR;

namespace Catalog.API.Application.Contracts
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {

    }
}
