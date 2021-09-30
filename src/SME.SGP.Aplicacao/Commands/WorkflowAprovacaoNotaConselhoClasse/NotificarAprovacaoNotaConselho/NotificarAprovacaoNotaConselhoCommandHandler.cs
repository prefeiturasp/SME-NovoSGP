using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoNotaConselhoCommandHandler : AsyncRequestHandler<NotificarAprovacaoNotaConselhoCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IServicoEol servicoEOL;
        public NotificarAprovacaoNotaConselhoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        protected override async Task Handle(NotificarAprovacaoNotaConselhoCommand request, CancellationToken cancellationToken)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
            var usuarioRf = request.NotasEmAprovacao.ConselhoClasseNota.AlteradoRF;
            var notaConceitoTitulo = request.NotasEmAprovacao.ConceitoId.HasValue ? "conceito" : "nota";
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(usuarioRf, "");
            var bimestre = repositorioConselhoClasseNota.ObterBimestreEmAprovacaoWf(request.WorkFlowId).Result;

            if (usuario != null)
            {
                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = turma.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = DateTime.Today.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = turma.Ue.Dre.CodigoDre,
                    Titulo = $"Alteração em {notaConceitoTitulo} final - Turma {turma.Nome} ({turma.AnoLetivo})",
                    Tipo = NotificacaoTipo.Notas,
                    Codigo = request.CodigoDaNotificacao ?? 0,
                    Mensagem = await MontaMensagemAprovacaoNotaPosConselho(turma, notaConceitoTitulo, request.NotasEmAprovacao, request.Aprovada, request.Justificativa, bimestre)
                });
            }
        }

        private async Task<string> MontaMensagemAprovacaoNotaPosConselho(Turma turma, string notaConceitoTitulo, WFAprovacaoNotaConselho notaEmAprovacao, bool aprovado, string justificativa, int bimestre)
        {
            var aprovadaRecusada = aprovado ? "aprovada" : "recusada";
            var motivo = aprovado ? "" : $"Motivo: {justificativa}.";
            var componenteCurricular = await ObterComponente(notaEmAprovacao.ConselhoClasseNota.ComponenteCurricularCodigo);
            var bimestreFormatado = bimestre == 0 ? "bimestre final" : $"{bimestre}º bimestre";

            var mensagem = new StringBuilder($@"<p>A alteração de {notaConceitoTitulo}(s) final(is) do {bimestreFormatado} do componente curricular {componenteCurricular}  
                            da turma {turma.Nome} da {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) 
                            de {turma.AnoLetivo} para o(s) estudantes(s) abaxo foi {aprovadaRecusada}. {motivo}</p>");

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 10px;'>Estudante</td>");
            mensagem.AppendLine("<td style='padding: 5px;'>Valor Anterior</td>");
            mensagem.AppendLine("<td style='padding: 5px;'>Novo Valor</td>");
            mensagem.AppendLine("</tr>");

            var alunosTurma = servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma).Result;

            var codigoAluno = repositorioConselhoClasseAluno.ObterPorId(notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAlunoId);
            var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == codigoAluno.AlunoCodigo);

            mensagem.AppendLine("<tr>");
            mensagem.Append($"<td style='padding: 10px;'> {aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({aluno?.CodigoAluno})</td>");

            if (!notaEmAprovacao.ConceitoId.HasValue)
            {
                mensagem.Append($"<td style='padding: 5px;'>{ObterNota(notaEmAprovacao.ConselhoClasseNota.Nota.Value)}</td>");
                mensagem.Append($"<td style='padding: 5px;'>{ObterNota(notaEmAprovacao.Nota.Value)}</td>");
            }
            else
            {
                mensagem.Append($"<td style='padding: 5px;'>{ObterConceito(notaEmAprovacao.ConselhoClasseNota.ConceitoId)}</td>");
                mensagem.Append($"<td style='padding: 5px;'>{ObterConceito(notaEmAprovacao.ConceitoId)}</td>");
            }
            mensagem.AppendLine("</tr>");

            mensagem.AppendLine("</table>");

            return mensagem.ToString();
        }

        private string ObterNota(double? nota)
        {
            if (!nota.HasValue)
                return string.Empty;
            else
                return nota.ToString();
        }

        private string ObterConceito(long? conceitoId)
        {
            if (!conceitoId.HasValue)
                return string.Empty;

            if (conceitoId == (int)ConceitoValores.P)
                return ConceitoValores.P.ToString();
            else if (conceitoId == (int)ConceitoValores.S)
                return ConceitoValores.S.ToString();
            else
                return ConceitoValores.NS.ToString();
        }

        private async Task<string> ObterComponente(long componenteCurricularCodigo)
           => await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(componenteCurricularCodigo));

    }
}
