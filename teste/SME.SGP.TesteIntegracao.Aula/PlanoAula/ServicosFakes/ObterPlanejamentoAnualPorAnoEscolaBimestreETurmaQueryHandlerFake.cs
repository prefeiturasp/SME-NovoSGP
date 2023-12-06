using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes
{
    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandlerFake: IRequestHandler<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery,PlanejamentoAnual>
    {
        public async Task<PlanejamentoAnual> Handle(ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PlanejamentoAnual()
            {
                Id = 1,
                TurmaId = 1,
                ComponenteCurricularId = long.Parse("138"),
                CriadoEm = DateTime.Now, 
                CriadoPor = "Sistema", 
                CriadoRF = "1"
            });
        }
    }
}