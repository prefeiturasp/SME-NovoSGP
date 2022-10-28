using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQueryHandlerFake : IRequestHandler<ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery, IEnumerable<PeriodoFechamentoBimestre>>
    {
        public async Task<IEnumerable<PeriodoFechamentoBimestre>> Handle(ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<PeriodoFechamentoBimestre>
            {
                new PeriodoFechamentoBimestre
                {
                    PeriodoFechamento = null,
                    PeriodoFechamentoId = 0,
                    FinalDoFechamento = DateTime.Today,
                    Id = 1,
                    InicioDoFechamento = DateTime.Today,
                    PeriodoEscolarId = 1,
                },
            };

            return lista;
        }
    }
}