using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.Aula.ServicosFake
{
    public class ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQueryHandlerFakeSyncRegencia : IRequestHandler<ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery, IEnumerable<DadosTurmaAulasAutomaticaDto>>
    {
        public async Task<IEnumerable<DadosTurmaAulasAutomaticaDto>> Handle(ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            List<DadosTurmaAulasAutomaticaDto> listaRetorno = new();

            if (request.ComponentesCurriculares.Contains("1105"))
                listaRetorno.Add(new DadosTurmaAulasAutomaticaDto() { ComponenteCurricularCodigo = "1105", TurmaCodigo = "1" });
            else
                listaRetorno.Add(new DadosTurmaAulasAutomaticaDto() { ComponenteCurricularCodigo = "1113", TurmaCodigo = "2" });

            return await Task.FromResult(listaRetorno);

        }
    }
}
