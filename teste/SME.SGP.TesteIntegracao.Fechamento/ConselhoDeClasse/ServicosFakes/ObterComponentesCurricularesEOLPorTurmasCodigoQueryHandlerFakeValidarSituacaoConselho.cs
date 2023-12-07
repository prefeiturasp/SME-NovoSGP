using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFakeValidarSituacaoConselho :
        IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = 139,
                    Descricao = "ARTE",
                    LancaNota = true,
                    Regencia = false,
                    TerritorioSaber = false
                },
                new ComponenteCurricularEol()
                {
                    Codigo = 138,
                    Descricao = "LINGUA PORTUGUESA",
                    LancaNota = true,
                    Regencia = false,
                    TerritorioSaber = false
                }
            });
        }
    }
}