using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesComNotaDeFechamentoOuConselhoQueryHandlerFake : IRequestHandler<ObterComponentesComNotaDeFechamentoOuConselhoQuery, IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesComNotaDeFechamentoOuConselhoQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesComNotaDeFechamentoOuConselhoQuery request, CancellationToken cancellationToken)
        {
            return new List<ComponenteCurricularDto>() {
                new ComponenteCurricularDto() { Codigo ="1",Descricao="1",DescricaoEol="1",LancaNota=true,TerritorioSaber=false,Regencia=false},
                new ComponenteCurricularDto() { Codigo ="2",Descricao="2",DescricaoEol="2",LancaNota=true,TerritorioSaber=false,Regencia=false},
            };
        }
    }
}
