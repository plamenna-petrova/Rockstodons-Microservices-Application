using MediatR;

namespace Catalog.API.Application.Abstractions
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {

    }
}
