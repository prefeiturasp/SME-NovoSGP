using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AprovacaoNotaConselhoClasseCommandHandler : AsyncRequestHandler<AprovacaoNotaConselhoClasseCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;
        private List<WFAprovacaoNotaConselho> WFAprovacoes;
        private List<Ue> Ues;
        private List<TurmasDoAlunoDto> Alunos;
        private List<ComponenteCurricularDescricaoDto> ComponentesCurriculares;
        private List<Usuario> Usuarios;

        public AprovacaoNotaConselhoClasseCommandHandler(IMediator mediator, IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
        }

        protected override async Task Handle(AprovacaoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            WFAprovacoes = (await repositorioWFAprovacaoNotaConselho.ObterNotaAguardandoAprovacaoPorWorkflow()).ToList();
            var agrupamentoPorTurma = WFAprovacoes.GroupBy(wf => wf.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma);

            await CarregarTodasUes();
            await CarregarTodosAlunos();
            await CarregarTodosComponentes();
            await CarregarTodosUsuarios();

            foreach (var grupoTurma in agrupamentoPorTurma)
            {
                var idAprovacao = await EnvicarNotificacao(grupoTurma.Key, grupoTurma.ToList());

                await ExecuteAlteracoesDasAprovacoes(grupoTurma.ToList(), idAprovacao);
            }
        }

        private async Task ExecuteAlteracoesDasAprovacoes(List<WFAprovacaoNotaConselho> aprovacoesPorTurma, long idAprovacao)
        {
            foreach(var aprovacao in aprovacoesPorTurma)
            {
                aprovacao.WfAprovacaoId = idAprovacao;

                await repositorioWFAprovacaoNotaConselho.SalvarAsync(aprovacao);
            }
        }

        private async Task CarregarTodasUes()
        {
            var ueIds = WFAprovacoes.Select(wf => wf.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.UeId).Distinct().ToArray();

            Ues = (await ObterUes(ueIds)).ToList();
        }

        private async Task CarregarTodosAlunos()
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo)).ToArray();

            Alunos = (await ObterAlunos(codigos)).ToList();
        }

        private async Task CarregarTodosComponentes() 
        {
            var codigos = WFAprovacoes.Select(wf => wf.ConselhoClasseNota.ComponenteCurricularCodigo).Distinct().ToArray();

            ComponentesCurriculares = (await ObterComponentes(codigos)).ToList();
        }

        private async Task CarregarTodosUsuarios()
        {
            var ids = WFAprovacoes.Select(wf => wf.UsuarioSolicitanteId).Distinct().ToArray();

            Usuarios = (await ObterUsuarios(ids)).ToList();
        }

        private async Task<long> EnvicarNotificacao(Turma turma, List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var ue = Ues.Find(ue => ue.Id == turma.UeId);
            var titulo = ObterTitulo(ue, turma);
            var descricao = ObterDescricao(ue, turma, aprovacoesPorTurma);

            return await mediator.Send(new EnviarNotificacaoCommand(
                                                                    titulo,
                                                                    descricao,
                                                                    NotificacaoCategoria.Workflow_Aprovacao,
                                                                    NotificacaoTipo.Fechamento,
                                                                    new Cargo[] { Cargo.CP, Cargo.Supervisor },
                                                                    ue.Dre.CodigoDre,
                                                                    ue.CodigoUe,
                                                                    turma.CodigoTurma,
                                                                    WorkflowAprovacaoTipo.AlteracaoNotaConselho,
                                                                    0)); //ConselhoClasseNotaId
        }

        private string ObterDescricao(Ue ue, Turma turma, List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var periodoEscolar = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;
            var descricao = new StringBuilder($@"A alteração de notas/conceitos pós-conselho do bimestre { periodoEscolar.Bimestre } 
                                                 de { turma.AnoLetivo } da turma { turma.NomeFiltro } da { ue.Nome } ({ ue.Dre.Abreviacao }) foram alteradas.");

            descricao.Append(ObterTabelaDosAlunos(aprovacoesPorTurma));

            return descricao.ToString();
        }

        private string ObterTitulo(Ue ue, Turma turma)
        {
            return $@"Alteração em nota/conceito pós-conselho - { ue.Nome } ({ ue.Dre.Abreviacao }) - { turma.NomeFiltro } (ano anterior)";
        }

        private string ObterTabelaDosAlunos(List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var descricao = new StringBuilder();
            descricao.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.AppendLine("<tbody>");
            descricao.AppendLine("<tr>");
            descricao.AppendLine("<td><strong>Componente curricular</strong></td>");
            descricao.AppendLine("<td><strong>Estudante</strong></td>");
            descricao.AppendLine("<td style='text-align: center;'><strong>Valor anterior</strong></td>");
            descricao.AppendLine("<td style='text-align: center;'><strong>Novo valor</strong></td>");
            descricao.AppendLine("<td><strong>Usuário que alterou</strong></td>");
            descricao.AppendLine("<td><strong>Data da alteração</strong></td>");
            descricao.AppendLine("</tr>");

            foreach (var aprovaco in aprovacoesPorTurma)
            {
                descricao.AppendLine(ObterLinhaDoAluno(aprovaco));
            }

            descricao.AppendLine("<tbody>");
            descricao.AppendLine("</table>");

            return descricao.ToString();
        }

        private string ObterLinhaDoAluno(WFAprovacaoNotaConselho aprovacao)
        {
            var aluno = Alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo);
            var componenteCurricular = ComponentesCurriculares.Find(componente => componente.Id == aprovacao.ConselhoClasseNota.ComponenteCurricularCodigo);
            var usuario = Usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);

            return $@"<tr>
                           <td>{componenteCurricular.Descricao}</td>
                           <td>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                           <td>{aprovacao.ConselhoClasseNota.Nota}</td>
                           <td>{aprovacao.Nota}</td>
                           <td>{usuario.Nome} ({usuario.CodigoRf})</td>
                           <td>{aprovacao.CriadoEm.ToString("dd/MM/yyy HH:mm")}</td>
                      </tr>";
        }

        private async Task<IEnumerable<Ue>> ObterUes(long[] ueIds)
            => await mediator.Send(new ObterUesPorIdsQuery(ueIds));

        private async Task<IEnumerable<ComponenteCurricularDescricaoDto>> ObterComponentes(long[] codigos)
            => await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(codigos));

        private async Task<IEnumerable<TurmasDoAlunoDto>> ObterAlunos(long[] codigos)
            => await mediator.Send(new ObterAlunosEolPorCodigosQuery(codigos));

        private async Task<IEnumerable<Usuario>> ObterUsuarios(long[] ids)
            => await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(ids));
    }
}
