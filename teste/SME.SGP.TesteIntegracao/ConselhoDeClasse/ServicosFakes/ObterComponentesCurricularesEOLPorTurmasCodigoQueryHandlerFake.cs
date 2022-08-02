using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = "Língua Portuguesa"
                }
            };
        }
    }
}
