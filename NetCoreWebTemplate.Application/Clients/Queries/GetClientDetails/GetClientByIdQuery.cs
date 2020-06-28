using MediatR;

namespace NetCoreWebTemplate.Application.Clients.Queries.GetClientDetails
{
    public class GetClientByIdQuery : IRequest<Unit>
    {
        public long ClientId { get; set; }

        public GetClientByIdQuery(long clientId)
        {
            ClientId = clientId;
        }
    }
}
