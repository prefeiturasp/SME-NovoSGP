using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaFechamentoComponenteUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaFechamentoComponenteUseCase
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ExecutarConsolidacaoTurmaFechamentoComponenteUseCase(IMediator mediator, IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado) : base(mediator)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FechamentoConsolidacaoTurmaComponenteBimestreDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível iniciar a consolidação do fechamento da turma -> componente. O id da turma bimestre componente curricular não foram informados.", LogNivel.Negocio, LogContexto.Turma));                
                return false;
            }

            var consolidadoTurmaComponente = await repositorioFechamentoConsolidado.ObterFechamentoConsolidadoPorTurmaBimestreComponenteCurricularAsync(filtro.TurmaId, filtro.ComponenteCurricularId, filtro.Bimestre);

            var fechamentos = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(filtro.TurmaId, new long[] { filtro.ComponenteCurricularId }, filtro?.Bimestre ?? 0));

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(filtro.TurmaId));
            if (professoresDaTurma is null || !professoresDaTurma.Any())
                return false;

            var fechamento = fechamentos?.FirstOrDefault();

            var atualizarConsolidado = false;

            if (fechamento != null && consolidadoTurmaComponente != null)
                atualizarConsolidado = consolidadoTurmaComponente.Status != fechamento.Situacao;

            (consolidadoTurmaComponente,atualizarConsolidado) = MapearFechamentoConsolidado(filtro, consolidadoTurmaComponente, fechamento, professoresDaTurma,atualizarConsolidado);

            if (consolidadoTurmaComponente.Id == 0 || atualizarConsolidado)
                await repositorioFechamentoConsolidado.SalvarAsync(consolidadoTurmaComponente);

            return true;
        }

        private (FechamentoConsolidadoComponenteTurma fechamento, bool consolidacaoAtualizada) MapearFechamentoConsolidado(FechamentoConsolidacaoTurmaComponenteBimestreDto filtro, FechamentoConsolidadoComponenteTurma consolidadoTurmaComponente, FechamentoTurmaDisciplina fechamento, IEnumerable<Infra.ProfessorTitularDisciplinaEol> professoresDaTurma, bool atualizarConsolidado)
        {
            var statusFechamento = fechamento != null ? fechamento.Situacao : SituacaoFechamento.NaoIniciado;

            var professorComponente = professoresDaTurma != null ? professoresDaTurma.FirstOrDefault(p => p.DisciplinasId.Contains(filtro.ComponenteCurricularId)) : null;

            if (consolidadoTurmaComponente == null)
            {
                consolidadoTurmaComponente = new FechamentoConsolidadoComponenteTurma()
                {
                    Bimestre = filtro.Bimestre,
                    ComponenteCurricularCodigo = filtro.ComponenteCurricularId,
                    TurmaId = filtro.TurmaId,
                    ProfessorNome = professorComponente != null ? professorComponente.ProfessorNome : "Sem professor titular",
                    ProfessorRf = professorComponente != null ? professorComponente.ProfessorRf : String.Empty,                    
                };
            }
            else
            {
                if(professorComponente != null)
                {
                    if (((consolidadoTurmaComponente.ProfessorNome.ToUpper() != professorComponente.ProfessorNome) && professorComponente.ProfessorNome != null) ||
                         (consolidadoTurmaComponente.ProfessorRf != professorComponente.ProfessorRf) && professorComponente.ProfessorRf != null)
                    {
                        consolidadoTurmaComponente.ProfessorNome = professorComponente != null ? professorComponente.ProfessorNome : "Sem professor titular";
                        consolidadoTurmaComponente.ProfessorRf = professorComponente != null ? professorComponente.ProfessorRf : String.Empty;
                        atualizarConsolidado = true;
                    }
                }
            }

            consolidadoTurmaComponente.DataAtualizacao = DateTime.Now;
            consolidadoTurmaComponente.Status = statusFechamento;

            return (consolidadoTurmaComponente, atualizarConsolidado);
        }
    }
}
