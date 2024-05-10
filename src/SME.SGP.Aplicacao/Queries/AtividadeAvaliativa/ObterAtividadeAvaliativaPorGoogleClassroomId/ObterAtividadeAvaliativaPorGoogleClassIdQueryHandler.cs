using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadeAvaliativaPorGoogleClassroomIdQueryHandler : IRequestHandler<
        ObterAtividadeAvaliativaPorGoogleClassroomIdQuery, AtividadeAvaliativa>
    {
        public readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterAtividadeAvaliativaPorGoogleClassroomIdQueryHandler(
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa;
        }

        public async Task<AtividadeAvaliativa> Handle(ObterAtividadeAvaliativaPorGoogleClassroomIdQuery request,
            CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativa.ObterPorAtividadeClassroomId(request.GoogleClassroomId);
        }
    }
}