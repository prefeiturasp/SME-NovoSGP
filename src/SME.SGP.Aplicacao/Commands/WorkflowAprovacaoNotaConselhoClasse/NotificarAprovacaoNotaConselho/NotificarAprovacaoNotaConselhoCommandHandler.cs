using MediatR;
using SME.SGP.Aplicacao.Integracoes;
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
    public class NotificarAprovacaoNotaConselhoCommandHandler : AsyncRequestHandler<NotificarAprovacaoNotaConselhoCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioUsuarioConsulta repositorioUsuario;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IServicoEol servicoEOL;
        public NotificarAprovacaoNotaConselhoCommandHandler(IMediator mediator, 
                                                            IRepositorioNotificacao repositorioNotificacao,
                                                            IRepositorioTurmaConsulta repositorioTurma,
                                                            IRepositorioUsuarioConsulta repositorioUsuario,
                                                            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                                            IServicoEol servicoEOL)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        protected override async Task Handle(NotificarAprovacaoNotaConselhoCommand request, CancellationToken cancellationToken)
        {
            var usuarioRf = await mediator.Send(new ObterCriadorWorkflowQuery(request.WorkFlowId));
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(usuarioRf, "");

            if(usuario != null)
            {
                var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
                var notaConceitoTitulo = request.NotasEmAprovacao.ConceitoId.HasValue ? "conceito" : "nota";
                var periodoEscolarId = request.NotasEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolarId;
                var bimestre = periodoEscolarId.HasValue ? 
                    await mediator.Send(new ObterBimestreDoPeriodoEscolarQuery(periodoEscolarId.Value)) :
                    0;

                var alunosTurma = servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma).Result;

                var codigoAluno = repositorioConselhoClasseAluno.ObterPorId(request.NotasEmAprovacao.ConselhoClasseNota.ConselhoClasseAlunoId);
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == codigoAluno.AlunoCodigo);
                var componenteCurricular = await ObterComponente(request.NotasEmAprovacao.ConselhoClasseNota.ComponenteCurricularCodigo);

                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = turma.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = DateTime.Today.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = turma.Ue.Dre.CodigoDre,
                    Titulo = $"Alteração em {notaConceitoTitulo} pós-conselho - {aluno.NomeAluno} ({aluno.CodigoAluno}) - {componenteCurricular} - {turma.Nome} ({turma.AnoLetivo})",
                    Tipo = NotificacaoTipo.Notas,
                    Codigo = request.CodigoDaNotificacao ?? 0,
                    Mensagem = await MontaMensagemAprovacaoNotaPosConselho(turma,
                                                                           aluno,
                                                                           request.NotasEmAprovacao,
                                                                           request.Aprovada,
                                                                           request.Justificativa,
                                                                           bimestre,
                                                                           request.NotaAnterior,
                                                                           request.ConceitoAnterior,
                                                                           componenteCurricular)
                });
            }
        }

        private async Task<string> MontaMensagemAprovacaoNotaPosConselho(Turma turma,
                                                                         AlunoPorTurmaResposta aluno,
                                                                         WFAprovacaoNotaConselho notaEmAprovacao,
                                                                         bool aprovado,
                                                                         string justificativa,
                                                                         int bimestre,
                                                                         double? notaAnterior,
                                                                         long? conceitoAnterior,
                                                                         string componenteCurricular)
        {
            var notaConceito = notaEmAprovacao.ConceitoId.HasValue ? "O conceito" : "A nota";
            var aprovadaRecusada = aprovado ? "aprovada" : "recusada";
            var motivo = aprovado ? "" : $"Motivo: {justificativa}.";
            var bimestreFormatado = bimestre == 0 ? "bimestre final" : $"{bimestre}º bimestre";

            var mensagem = new StringBuilder($@"<p>{notaConceito} pós-conselho do {bimestreFormatado} do componente curricular {componenteCurricular}  
                            da turma {turma.Nome} da {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) 
                            de {turma.AnoLetivo} para o(s) estudantes(s) abaixo foi {aprovadaRecusada}. {motivo}</p>");

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 10px;'>Estudante</td>");
            mensagem.AppendLine("<td style='padding: 5px;'>Valor Anterior</td>");
            mensagem.AppendLine("<td style='padding: 5px;'>Novo Valor</td>");
            mensagem.AppendLine("</tr>");

            mensagem.AppendLine("<tr>");
            mensagem.Append($"<td style='padding: 10px;'> {aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({aluno?.CodigoAluno})</td>");

            if (!notaEmAprovacao.ConceitoId.HasValue)
            {
                mensagem.Append($"<td style='padding: 5px;'>{ObterNota(notaAnterior)}</td>");
                mensagem.Append($"<td style='padding: 5px;'>{ObterNota(notaEmAprovacao.Nota.Value)}</td>");
            }
            else
            {
                mensagem.Append($"<td style='padding: 5px;'>{ObterConceito(conceitoAnterior)}</td>");
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
                return ConceitoValores.P.Name();
            else if (conceitoId == (int)ConceitoValores.S)
                return ConceitoValores.S.Name();
            else
                return ConceitoValores.NS.Name();
        }

        private async Task<string> ObterComponente(long componenteCurricularCodigo)
           => await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(componenteCurricularCodigo));

    }
}
