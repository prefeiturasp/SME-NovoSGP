using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFakeValidarSituacaoConselho :
        IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>
    {
        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "139",
                    Descricao = "ARTE",
                    LancaNota = true,
                    Regencia = false,
                    DescricaoEol = "ARTE",
                    TerritorioSaber = false
                },
                new ComponenteCurricularDto()
                {
                    Codigo = "138",
                    Descricao = "LINGUA PORTUGUESA",
                    LancaNota = true,
                    Regencia = false,
                    DescricaoEol = "LINGUA PORTUGUESA",
                    TerritorioSaber = false
                }
            };
        }
    }
}