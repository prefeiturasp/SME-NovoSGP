using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandler : IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery, PeriodoFechamentoBimestre>
    {
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamentoConsulta;

        public ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandler(IRepositorioEventoFechamentoConsulta repositorioEventoFechamentoConsulta)
        {
            this.repositorioEventoFechamentoConsulta = repositorioEventoFechamentoConsulta ?? throw new ArgumentNullException(nameof(repositorioEventoFechamentoConsulta));
        }

        public async Task<PeriodoFechamentoBimestre> Handle(ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioEventoFechamentoConsulta.UeEmFechamentoBimestre(request.TipoCandarioId, request.EhTurmaInfantil, request.Bimestre);
    }
}
