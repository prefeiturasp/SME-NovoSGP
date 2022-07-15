using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaIntervaloTurmaDisciplinaQueryHandler : IRequestHandler<ObterAulaIntervaloTurmaDisciplinaQuery,AulaConsultaDto>
    {
        private readonly IRepositorioAulaConsulta repositorioAulaConsulta;

        public ObterAulaIntervaloTurmaDisciplinaQueryHandler(IRepositorioAulaConsulta repositorioAulaConsulta)
        {
            this.repositorioAulaConsulta = repositorioAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioAulaConsulta));
        }

        public async Task<AulaConsultaDto> Handle(ObterAulaIntervaloTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAulaConsulta.ObterAulaIntervaloTurmaDisciplina(request.DataInicio, request.DataFim, request.TurmaId, request.AtividadeAvaliativaId);
        }
    }
}