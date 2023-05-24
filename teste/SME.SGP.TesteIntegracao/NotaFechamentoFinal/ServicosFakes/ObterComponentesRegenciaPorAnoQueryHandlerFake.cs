using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes
{
    public class ObterComponentesRegenciaPorAnoQueryHandlerFake : IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private const int COMPONENTE_CIENCIAS_ID_89 = 89;
        private const int COMPONENTE_GEOGRAFIA_ID_8 = 8;
        private const int COMPONENTE_MATEMATICA_ID_2 = 2;
        private const int COMPONENTE_HISTORIA_ID_7 = 7;
        private const int COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105 = 1105;

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesRegenciaPorAnoEolQuery request, CancellationToken cancellationToken)
        {
            return new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = COMPONENTE_CIENCIAS_ID_89
                },
                new ComponenteCurricularEol()
                {
                    Codigo = COMPONENTE_GEOGRAFIA_ID_8
                },
                new ComponenteCurricularEol()
                {
                    Codigo = COMPONENTE_MATEMATICA_ID_2
                },
                new ComponenteCurricularEol()
                {
                    Codigo = COMPONENTE_HISTORIA_ID_7
                },
                 new ComponenteCurricularEol()
                {
                    Codigo = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105
                }
            };
        }
    }
}
