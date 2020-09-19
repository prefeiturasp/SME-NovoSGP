using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualCommandHandler : IRequestHandler<SalvarPlanejamentoAnualCommand, AuditoriaDto>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;

        public SalvarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                     IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                     IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }
        public async Task<AuditoriaDto> Handle(SalvarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            var planejamentoAnual = new PlanejamentoAnual(comando.TurmaId, comando.ComponenteCurricularId);

            await repositorioPlanejamentoAnual.SalvarAsync(planejamentoAnual);

            var planejamentoPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(comando.PeriodoEscolarId)
            {
                PlanejamentoAnualId = planejamentoAnual.Id
            };

            await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(planejamentoPeriodoEscolar);

            var planejamentoAnualComponentes = new List<PlanejamentoAnualComponente>();
            foreach (var componente in comando.Componentes)
            {
                var planejamentoAnualComponente = new PlanejamentoAnualComponente
                {
                    ComponenteCurricularId = componente.ComponenteCurricularId,
                    Descricao = componente.Descricao,
                    PlanejamentoAnualPeriodoEscolarId = planejamentoPeriodoEscolar.Id
                };
                //SALVAR COMPONENTES

                var objetivos = componente.ObjetivosAprendizagemId.Select(c => new PlanejamentoAnualObjetivoAprendizagem
                {
                    ObjetivoAprendizagemId = c,
                    PlanejamentoAnualComponenteId = planejamentoAnualComponente.Id
                });

                await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponente);
                await Task.Run(() => repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(objetivos));
            }

            return (AuditoriaDto)planejamentoAnual;
        }
    }
}
