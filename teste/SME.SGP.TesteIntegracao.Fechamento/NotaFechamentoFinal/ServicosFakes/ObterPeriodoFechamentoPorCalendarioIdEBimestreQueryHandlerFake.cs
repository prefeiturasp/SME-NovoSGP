using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoFinal.ServicosFakes
{
    internal class ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandlerFake : IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery, PeriodoFechamentoBimestre>
    {
        public async Task<PeriodoFechamentoBimestre> Handle(ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery request, CancellationToken cancellationToken)
        {
            return new PeriodoFechamentoBimestre()
            {
                InicioDoFechamento = new DateTime(DateTime.Now.Year, 10,03),
                FinalDoFechamento = new DateTime(DateTime.Now.Year, 12,22),
                PeriodoFechamentoId = 1
            };
        }
    }
}
