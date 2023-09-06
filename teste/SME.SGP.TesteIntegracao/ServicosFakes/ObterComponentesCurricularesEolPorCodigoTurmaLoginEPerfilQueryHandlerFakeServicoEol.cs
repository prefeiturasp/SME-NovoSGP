using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeServicoEol : IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            return new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = 1106,
                    TerritorioSaber = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 2, Nome = "Diversificada" },
                    LancaNota = false
                },
                new ComponenteCurricularEol()
                {
                    Codigo = 138,
                    TerritorioSaber = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 1, Nome = "Base Nacional Comum" },
                    LancaNota = true
                }
            };
        }
    }
}
