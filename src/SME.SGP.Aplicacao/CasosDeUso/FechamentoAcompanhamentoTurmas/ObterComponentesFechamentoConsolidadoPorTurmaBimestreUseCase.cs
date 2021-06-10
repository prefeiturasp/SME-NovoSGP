using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase : AbstractUseCase, IObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase
    {
        public ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>> Executar(FiltroComponentesFechamentoConsolidadoDto filtro)
        {
            var componentesCurriculares = new List<ConsolidacaoTurmaComponenteCurricularDto>();

            int[] situacoesFechamento = new int[] { filtro.SituacaoFechamento };

            if (filtro.SituacaoFechamento == (int)SituacaoFechamento.NaoIniciado)            
                situacoesFechamento = new int[] { (int)SituacaoFechamento.NaoIniciado, (int)SituacaoFechamento.EmProcessamento };

            var componentesCurricularesPorTurma = new List<ComponenteCurricularPorTurma>();

            var componentes = await mediator.Send(new ObterComponentesFechamentoConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre, situacoesFechamento));
            
            foreach(var componente in componentes)
            {
                componentesCurricularesPorTurma.Add(new ComponenteCurricularPorTurma
                {
                    GrupoMatriz = new ComponenteCurricularGrupoMatriz{ Id = componente.GrupoMatrizId},
                    AreaDoConhecimento = new AreaDoConhecimento { Id = componente.AreaConnhecimentoId},
                    Disciplina = componente.Descricao,
                    CodDisciplina = componente.Id 
                });
            }

            var componentesOrdenados = await mediator.Send(new OrdenarComponentesPorGrupoMatrizAreaConhecimentoQuery(componentesCurricularesPorTurma));


            foreach (var cc in componentesOrdenados)
            {
                var componente = componentes.FirstOrDefault(c => c.GrupoMatrizId == cc.GrupoMatriz.Id && 
                c.AreaConnhecimentoId == cc.AreaDoConhecimento.Id && 
                c.Id == cc.CodDisciplina);

                componentesCurriculares.Add(componente);
            };

            //return componentesOrdenados.Select(a => new ConsolidacaoTurmaComponenteCurricularDto() { AreaConnhecimentoId = a.AreaDoConhecimento. });

            return componentesCurriculares;
        }
    }
}
