using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoParecerConclusivoCommandHandler : AsyncRequestHandler<NotificarAprovacaoParecerConclusivoCommand>
    {
        private readonly IMediator mediator;
        
        //separar
        protected List<TurmasDoAlunoDto> Alunos;
        protected List<Usuario> Usuarios;
        
        public NotificarAprovacaoParecerConclusivoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(NotificarAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacoes = request.PareceresEmAprovacao;
            await CarregarInformacoesParaNotificacao(wfAprovacoes);
            var aprovacao = request.Aprovado ? "aprovada" : "recusada";

            var turma = await ObterTurma(wfAprovacoes.FirstOrDefault().TurmaId);
            var titulo = $@"Alteração de parecer conclusivo - {turma.Ue.Nome} (ano anterior)";
            var mensagem = ObterMensagem(turma.Ue, turma, wfAprovacoes.ToList(), request.Aprovado, request.Motivo);

            foreach (var usuario in Usuarios)
            {
                await mediator.Send(new NotificarUsuarioCommand(
                                                                titulo,
                                                                mensagem,
                                                                usuario.CodigoRf,
                                                                NotificacaoCategoria.Aviso,
                                                                NotificacaoTipo.Fechamento,
                                                                turma.Ue.Dre.CodigoDre,
                                                                turma.Ue.CodigoUe,
                                                                turma.CodigoTurma));
            }
        }


        private async Task<Turma> ObterTurma(long turmaId)
           => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

        private async Task CarregarInformacoesParaNotificacao(IEnumerable<WFAprovacaoParecerConclusivoDto> wfAprovacoes)
        {
            await CarregarTodosAlunos(wfAprovacoes);
            await CarregarTodosUsuarios(wfAprovacoes);
        }

        protected string ObterMensagem(Ue ue, Turma turma, List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma, bool aprovada, string motivo)
        {
            var descricaoAprovadoRecusado = aprovada ? "aprovada" : "recusada";
            var msg = new StringBuilder();
            msg.Append($@"A alteração de pareceres conclusivos dos estudantes abaixo da turma {turma.Nome} da {ue.Nome} ({ue.Dre.Abreviacao}) de {turma.AnoLetivo} foi {descricaoAprovadoRecusado}. Motivo: {motivo}.");
            msg.Append(ObterTabelaPareceresAlterados(aprovacoesPorTurma));
            return msg.ToString();
        }

        private string ObterTabelaPareceresAlterados(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var msg = new StringBuilder();
            msg.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            msg.AppendLine("<tbody>");
            msg.AppendLine("<tr>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Estudante</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Parecer anterior</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Novo Parecer</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Usuário que alterou</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Data da alteração</strong></td>");
            msg.AppendLine("</tr>");

            foreach (var aprovaco in aprovacoesPorTurma)
            {
                msg.AppendLine(ObterLinhaParecerAlterado(aprovaco));
            }
            msg.AppendLine("<tbody>");
            msg.AppendLine("</table>");
            return msg.ToString();
        }

        private string ObterLinhaParecerAlterado(WFAprovacaoParecerConclusivoDto aprovacao)
        {
            var aluno = Alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.AlunoCodigo);
            var usuario = Usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);

            return $@"<tr>
                           <td style='padding: 3px;'>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                           <td style='padding: 3px;'>{aprovacao.NomeParecerAnterior}</td>
                           <td style='padding: 3px;'>{aprovacao.NomeParecerNovo}</td>
                           <td style='padding: 3px;'>{usuario.Nome} ({usuario.CodigoRf})</td>
                           <td style='padding: 3px;'>{aprovacao.CriadoEm.ToString("dd/MM/yyy HH:mm")}</td>
                      </tr>";
        }

        private async Task CarregarTodosAlunos(IEnumerable<WFAprovacaoParecerConclusivoDto> WFAprovacoes)
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.AlunoCodigo)).ToArray();

            Alunos = (await ObterAlunos(codigos)).ToList();
        }
        private async Task CarregarTodosUsuarios(IEnumerable<WFAprovacaoParecerConclusivoDto> WFAprovacoes)
        {
            var ids = WFAprovacoes.Select(wf => wf.UsuarioSolicitanteId).Distinct().ToArray();

            Usuarios = (await ObterUsuarios(ids)).ToList();
        }

        private async Task<IEnumerable<TurmasDoAlunoDto>> ObterAlunos(long[] codigos)
                => await mediator.Send(new ObterAlunosEolPorCodigosQuery(codigos));
        private async Task<IEnumerable<Usuario>> ObterUsuarios(long[] ids)
            => await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(ids));

    }
}
