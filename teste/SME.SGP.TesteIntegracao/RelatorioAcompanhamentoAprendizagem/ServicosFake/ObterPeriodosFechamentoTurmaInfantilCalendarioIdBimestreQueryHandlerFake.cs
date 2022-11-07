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
                new ()
                {
                    PeriodoFechamento = null,
                    PeriodoFechamentoId = 0,
                    FinalDoFechamento = DateTimeExtension.HorarioBrasilia().Date,
                    Id = 1,
                    InicioDoFechamento = DateTimeExtension.HorarioBrasilia().Date,
                    PeriodoEscolarId = 1,
                },
            };

            return lista;
        }
    }
}