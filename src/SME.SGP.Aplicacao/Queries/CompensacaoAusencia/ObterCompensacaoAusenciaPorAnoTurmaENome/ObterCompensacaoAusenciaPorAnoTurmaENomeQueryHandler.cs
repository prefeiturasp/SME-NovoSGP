using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterCompensacaoAusenciaPorAnoTurmaENomeQueryHandler : IRequestHandler<ObterCompensacaoAusenciaPorAnoTurmaENomeQuery, CompensacaoAusencia>
    {
        private readonly IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusenciaConsulta;

        public ObterCompensacaoAusenciaPorAnoTurmaENomeQueryHandler(IRepositorioCompensacaoAusenciaConsulta repositorio)
        {
            this.repositorioCompensacaoAusenciaConsulta = repositorio;
        }

        public async Task<CompensacaoAusencia> Handle(ObterCompensacaoAusenciaPorAnoTurmaENomeQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusenciaConsulta.ObterPorAnoTurmaENome(request.AnoLetivo, request.TurmaId, request.Atividade, request.Id);
    }
}

