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
        protected const string MENSAGEM_DINAMICA_TABELA_POR_ALUNO = "<mensagemDinamicaTabelaPorAluno>";
        private const string SEM_PARECER = "Sem parecer";
        protected readonly IMediator mediator;
        protected List<TurmasDoAlunoDto> Alunos;
        protected List<Usuario> Usuarios;
        protected IEnumerable<WFAprovacaoParecerConclusivoDto> WFAprovacoes;

        protected NotificacaoParecerConclusivoConselhoClasseCommandBase(IMediator mediator)
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

        protected virtual string ObterTextoCabecalho(Turma turma) => string.Empty;
        protected virtual string ObterTextoRodape() => string.Empty;
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected virtual string ObterTitulo(Turma turma)
        {
            return $@"Alteração de parecer conclusivo - Turma {turma.Nome} (ano anterior) - {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) ";
        }

        protected string ObterMensagem(Turma turma, List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var msg = new StringBuilder();
            msg.Append(ObterTextoCabecalho(turma));
            msg.Append(ObterTabelaPareceresAlterados(aprovacoesPorTurma, turma));
            msg.Append(ObterTextoRodape());
            return msg.ToString();
        }

        protected virtual string ObterTabelaPareceresAlterados(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma, Turma turma)
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
                msg.AppendLine(ObterLinhaParecerAlterado(aprovaco, turma));

            msg.AppendLine("<tbody>");
            msg.AppendLine("</table>");

            return msg.ToString();
        }

        private string ObterLinhaParecerAlterado(WFAprovacaoParecerConclusivoDto aprovacao, Turma turma)
        {
            var aluno = Alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.AlunoCodigo && aluno.CodigoTurma.ToString() == turma.CodigoTurma);
            var usuario = Usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);

            return $@"<tr>
                <td style='padding: 3px;'>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                <td style='padding: 3px;'>{ObterNomeParecer(aprovacao.NomeParecerAnterior)}</td>
                <td style='padding: 3px;'>{ObterNomeParecer(aprovacao.NomeParecerNovo)}</td>
                <td style='padding: 3px;'>{usuario.Nome} ({usuario.CodigoRf})</td>
                <td style='padding: 3px;'>{aprovacao.CriadoEm:dd/MM/yyyy HH:mm}</td>
            </tr>";
        }

        private string ObterNomeParecer(string parecer)
        {
            return string.IsNullOrEmpty(parecer) ? SEM_PARECER : parecer;
        }

        private async Task CarregarTodosAlunos()
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.AlunoCodigo)).ToArray();
            var wfAprovacaoParecerConclusivo = WFAprovacoes.FirstOrDefault();

            if (wfAprovacaoParecerConclusivo.EhNulo())
                return;
            
            var anoLetivo = wfAprovacaoParecerConclusivo.AnoLetivo;
            Alunos = (await ObterAlunos(codigos, anoLetivo)).ToList();
        }
        
        private async Task CarregarTodosUsuarios()
        {
            var ids = WFAprovacoes.Select(wf => wf.UsuarioSolicitanteId).Distinct().ToArray();
            Usuarios = (await ObterUsuarios(ids)).ToList();
        }

        private async Task<IEnumerable<TurmasDoAlunoDto>> ObterAlunos(long[] codigos, int anoLetivo)
            => await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(codigos, anoLetivo));
        
        private async Task<IEnumerable<Usuario>> ObterUsuarios(long[] ids)
            => await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(ids));
    }
 }
