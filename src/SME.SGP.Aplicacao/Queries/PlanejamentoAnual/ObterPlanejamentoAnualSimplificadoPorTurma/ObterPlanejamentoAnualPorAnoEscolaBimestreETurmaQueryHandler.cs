using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandler : IRequestHandler<ObterPlanejamentoAnualSimplificadoPorTurmaQuery, PlanoAnualResumoDto>
    {
        public readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }
        public async Task<PlanoAnualResumoDto> Handle(ObterPlanejamentoAnualSimplificadoPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var planoAnual = await repositorioPlanejamentoAnual.ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(request.Ano, request.EscolaId, request.TurmaId, request.Bimestre, request.DisciplinaId);

            return MapearParaDto(planoAnual);
        }

        private PlanoAnualResumoDto MapearParaDto(PlanoAnual plano) =>
            plano == null ? null :
            new PlanoAnualResumoDto()
            {
                Id = plano.Id,
                ObjetivosAprendizagemOpcionais = plano.ObjetivosAprendizagemOpcionais
            };
    }
}
