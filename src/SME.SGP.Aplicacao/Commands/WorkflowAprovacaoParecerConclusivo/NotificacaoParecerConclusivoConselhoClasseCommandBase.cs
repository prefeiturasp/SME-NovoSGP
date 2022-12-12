using MediatR;
using SME.SGP.Dominio;
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
    public abstract class NotificacaoParecerConclusivoConselhoClasseCommandBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        protected readonly IMediator mediator;
        protected List<TurmasDoAlunoDto> Alunos;
        protected List<Usuario> Usuarios;
        protected IEnumerable<WFAprovacaoParecerConclusivoDto> WFAprovacoes;

        public NotificacaoParecerConclusivoConselhoClasseCommandBase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected async Task<Turma> ObterTurma(long turmaId)
           => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

        protected async Task CarregarInformacoesParaNotificacao(IEnumerable<WFAprovacaoParecerConclusivoDto> wfAprovacoes)
        {
            WFAprovacoes = wfAprovacoes;
            await CarregarTodosAlunos();
            await CarregarTodosUsuarios();
        }

        protected abstract string ObterTextoCabecalho(Turma turma);
        protected abstract string ObterTextoRodape();
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected virtual string ObterTitulo(Turma turma)
        {
            return $@"Alteração de parecer conclusivo - {turma.Ue.Nome} (ano anterior)";
        }

        protected string ObterMensagem(Turma turma, List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var msg = new StringBuilder();
            msg.Append(ObterTextoCabecalho(turma));
            msg.Append(ObterTabelaPareceresAlterados(aprovacoesPorTurma));
            msg.Append(ObterTextoRodape());
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
                msg.AppendLine(ObterLinhaParecerAlterado(aprovaco));

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
                <td style='padding: 3px;'>{aprovacao.CriadoEm:dd/MM/yyyy HH:mm}</td>
            </tr>";
        }

        private async Task CarregarTodosAlunos()
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.AlunoCodigo)).ToArray();
            Alunos = (await ObterAlunos(codigos)).ToList();
        }
        
        private async Task CarregarTodosUsuarios()
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
