using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventoFechamenoPorIdQueryHandler : IRequestHandler<ObterEventoFechamenoPorIdQuery, EventoFechamento>
    {
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;

        public ObterEventoFechamenoPorIdQueryHandler(IRepositorioEventoFechamentoConsulta repositorio)
        {
            this.repositorioEventoFechamento = repositorio;
        }

        public async Task<EventoFechamento> Handle(ObterEventoFechamenoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioEventoFechamento.ObterPorIdFechamento(request.Id);
    }
}