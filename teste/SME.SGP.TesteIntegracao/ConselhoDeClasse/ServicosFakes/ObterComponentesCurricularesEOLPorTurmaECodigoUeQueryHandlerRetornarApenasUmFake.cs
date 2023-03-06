using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandlerApenasUmFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>
    {
        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery request, CancellationToken cancellationToken)
        {

            return new List<ComponenteCurricularDto>()
        {
          new ComponenteCurricularDto()
          {
            Codigo = "2",
            Descricao = "MATEMATICA",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "MATEMATICA",
            TerritorioSaber = false
          }
        };
        }
    }
}
