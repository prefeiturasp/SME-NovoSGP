using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake138 : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private const string LINGUA_PORTUGUESA = "LINGUA PORTUGUESA";
        private const long CODIGO_PORTUGUES = 138;

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                Codigo = CODIGO_PORTUGUES,
                Descricao = LINGUA_PORTUGUESA,
                LancaNota = true,
                Regencia = false,
                TerritorioSaber = false
                }
            };
        }
    }
}