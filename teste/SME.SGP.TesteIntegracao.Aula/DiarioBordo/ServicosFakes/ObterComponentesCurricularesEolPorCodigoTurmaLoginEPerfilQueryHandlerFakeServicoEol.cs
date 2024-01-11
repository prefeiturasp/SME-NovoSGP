using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.DiarioBordo.ServicosFakes
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeServicoEol : IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = 512,
                    CodigoComponenteCurricularPai = 512,
                    TerritorioSaber = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 2, Nome = "" },
                    LancaNota = false
                },
                new ComponenteCurricularEol()
                {
                    Codigo = 513,
                     CodigoComponenteCurricularPai = 512,
                    TerritorioSaber = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 1, Nome = "" },
                    LancaNota = true
                }
            });
        }
    }
}
