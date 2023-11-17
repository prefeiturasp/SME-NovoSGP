using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse.ServicosFakes
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>
    {
        public ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<InfoComponenteCurricular>> Handle(ObterInfoComponentesCurricularesESPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return new List<InfoComponenteCurricular>(){
                new InfoComponenteCurricular(){ Codigo= 138, CodigoComponenteCurricularPai=null, Nome="138", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
                new InfoComponenteCurricular(){ Codigo= 139, CodigoComponenteCurricularPai=null, Nome="139", EhRegencia = false, EhTerritorioSaber = false, RegistraFrequencia = true, LancaNota = true},
            };
        }
    }
}
