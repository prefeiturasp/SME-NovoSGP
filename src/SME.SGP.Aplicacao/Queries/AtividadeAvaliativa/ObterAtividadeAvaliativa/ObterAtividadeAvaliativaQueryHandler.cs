using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadeAvaliativaQueryHandler : IRequestHandler<ObterAtividadeAvaliativaQuery, AtividadeAvaliativa>
    {
        public readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        public ObterAtividadeAvaliativaQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa;
        }

        public async Task<AtividadeAvaliativa> Handle(ObterAtividadeAvaliativaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativa.ObterAtividadeAvaliativa(request.DataAvaliacao, request.DisciplinaId, request.TurmaId, request.UeId);
        }
    }
}
