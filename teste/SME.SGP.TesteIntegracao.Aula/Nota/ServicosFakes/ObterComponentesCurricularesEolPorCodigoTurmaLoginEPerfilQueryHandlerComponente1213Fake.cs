using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Nota.ServicosFakes
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerComponente1213Fake : IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var componentes = new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = 1213,
                    TerritorioSaber = false
                }
            };

            return Task.FromResult<IEnumerable<ComponenteCurricularEol>>(componentes);
        }
    }
}
