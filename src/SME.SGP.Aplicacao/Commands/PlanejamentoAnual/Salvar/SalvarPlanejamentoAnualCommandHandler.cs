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

        public SalvarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }
        public async Task<AuditoriaDto> Handle(SalvarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            var planejamentoAnual = new PlanejamentoAnual(comando.TurmaId, comando.ComponenteCurricularId);
            var planejamentoPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(comando.PeriodoEscolarId);
            var planejamentoAnualComponentes = new List<PlanejamentoAnualComponente>();
            //chamar salvar repositorio planejamentoPeriodoEscolar

            //SALVAR PLANEJAMENTO ANUAL
            await repositorioPlanejamentoAnual.SalvarAsync(planejamentoAnual);
            //SALVAR PERIODO ESCOLAR
            await repositorioPlanejamentoAnual.SalvarPlanejamentoPeriodoEscolarAsync(planejamentoPeriodoEscolar);
            //SALVAR COMPONENTES

            //SALVAR OBJETIVOS DOS COMPONENTES

            foreach (var componente in comando.Componentes)
            {
                //planejamentoAnualComponentes.Add();
                var planejamentoAnualComponente = new PlanejamentoAnualComponente
                {
                    ComponenteCurricularId = componente.ComponenteCurricularId,
                    PlanejamentoAnualPeriodoEscolarId = planejamentoPeriodoEscolar.Id
                };
                await repositorioPlanejamentoAnual.SalvarPlanejamentoAnualComponenteAsync(planejamentoAnualComponente);

                var objetivos = componente.ObjetivosAprendizagemId.Select(c => new PlanejamentoAnualObjetivoAprendizagem
                {
                    ObjetivoAprendizagemId = c,
                    PlanejamentoAnualComponenteId = planejamentoAnualComponente.Id
                });
                await repositorioPlanejamentoAnual.SalvarPlanejamentoAnualComponenteAsync(planejamentoAnualComponente);
            }

            planejamentoPeriodoEscolar.PlanejamentoAnualId = planejamentoAnual.Id;



            return (AuditoriaDto)planejamentoAnual;
        }
    }
}
