using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase : AbstractUseCase, IObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase
    {
        private readonly IServicoUsuario servicoUsuario;

        public ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase(IMediator mediator, IServicoUsuario servicoUsuario) : base(mediator)
        {
            this.servicoUsuario = servicoUsuario ??
                 throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>> Executar(FiltroComponentesFechamentoConsolidadoDto filtro)
        {
            var componentesCurriculares = new List<ConsolidacaoTurmaComponenteCurricularDto>();

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

            int[] situacoesFechamento = new int[] { filtro.SituacaoFechamento };

            if (filtro.SituacaoFechamento == (int)SituacaoFechamento.NaoIniciado)            
                situacoesFechamento = new int[] { (int)SituacaoFechamento.NaoIniciado, (int)SituacaoFechamento.EmProcessamento };

            var componentesCurricularesPorTurma = new List<ComponenteCurricularPorTurma>();

            var componentesCurricularesEOL = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turma.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, true));

            var componentes = await mediator.Send(new ObterComponentesFechamentoConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre, situacoesFechamento));
            
            foreach(var componente in componentes)
            {
                var descricao = componentesCurricularesEOL.FirstOrDefault(c => c.Codigo == componente.Id || c.CodigoComponenteTerritorioSaber == componente.Id)?.Descricao;

                componentesCurricularesPorTurma.Add(new ComponenteCurricularPorTurma
                {
                    GrupoMatriz = new ComponenteCurricularGrupoMatriz{ Id = componente.GrupoMatrizId},
                    AreaDoConhecimento = new AreaDoConhecimento { Id = componente.AreaConnhecimentoId},
                    Disciplina = descricao.NaoEhNulo() && descricao.Any() ? descricao : componente.Descricao,
                    CodDisciplina = componente.Id
                }); ;
            }

            var componentesOrdenados = await mediator.Send(new OrdenarComponentesPorGrupoMatrizAreaConhecimentoQuery(componentesCurricularesPorTurma));


            foreach (var cc in componentesOrdenados)
            {
                var componente = componentes.FirstOrDefault(c => c.GrupoMatrizId == cc.GrupoMatriz.Id && 
                c.AreaConnhecimentoId == cc.AreaDoConhecimento.Id && 
                c.Id == cc.CodDisciplina);
                componente.Descricao = cc.Disciplina;
                componentesCurriculares.Add(componente);
            };            

            return componentesCurriculares;
        }
    }
}
