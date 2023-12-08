using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>
    {
        public ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<InfoComponenteCurricular>> Handle(ObterInfoComponentesCurricularesESPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<InfoComponenteCurricular>(){
                new InfoComponenteCurricular(){ Codigo= 1, CodigoComponenteCurricularPai=null, Nome="1", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
                new InfoComponenteCurricular(){ Codigo= 2, CodigoComponenteCurricularPai=null, Nome="2", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
                new InfoComponenteCurricular(){ Codigo= 3, CodigoComponenteCurricularPai=null, Nome="3", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
            });
        }
    }
}
