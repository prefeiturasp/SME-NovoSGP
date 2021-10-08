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
    public class GerarWFAprovacaoNotaConselhoClasseCommandHandler : AsyncRequestHandler<GerarWFAprovacaoNotaConselhoClasseCommand>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;
        private readonly IMediator mediator;

        public GerarWFAprovacaoNotaConselhoClasseCommandHandler(IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho, IMediator mediator)
        {
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new System.ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(GerarWFAprovacaoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            await ExcluirWorkflowAprovacao(request.ConselhoClasseNotaId);
            var wfAprovacaoId = await GerarWFAprovacao(request);

            await repositorioWFAprovacaoNotaConselho.Salvar(new WFAprovacaoNotaConselho()
            {
                ConselhoClasseNotaId = request.ConselhoClasseNotaId,
                Nota = request.Nota,
                ConceitoId = request.ConceitoId,
                WfAprovacaoId = wfAprovacaoId,
                UsuarioSolicitanteId = request.UsuarioLogado.Id
            });
        }

        private async Task ExcluirWorkflowAprovacao(long conselhoClasseNotaId)
        {
            await mediator.Send(new ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand(conselhoClasseNotaId));
        }

        private async Task<long> GerarWFAprovacao(GerarWFAprovacaoNotaConselhoClasseCommand request)
        {
            var componenteCurricular = await ObterComponente(request.ComponenteCurricularCodigo);
            var bimestre = request.Bimestre.HasValue && request.Bimestre.Value > 0 ? request.Bimestre.ToString() : "Final";
            var ue = await ObterUe(request.Turma.UeId);
            var turma = $"{request.Turma.Nome} da {ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao}) de {request.Turma.AnoLetivo}";
            var professor = $"{request.UsuarioLogado.Nome} ({request.UsuarioLogado.CodigoRf})";
            var data = $"{DateTime.Today:dd/MM/yyyy} às {DateTime.Now:HH:mm}";
            var aluno = await ObterAluno(request.AlunoCodigo, request.Turma.AnoLetivo);

            var titulo = $"Alteração em nota/conceito pós-conselho - {aluno.Nome} ({aluno.CodigoAluno}) - {componenteCurricular} - {request.Turma.Nome} ({request.Turma.AnoLetivo})";
            var descricao = new StringBuilder($"A nota/conceito pós-conselho do bimestre {bimestre} do componente curricular {componenteCurricular} da turma {turma} foi alterada pelo Professor {professor} em {data}.<br/>");
            descricao.AppendLine(await ObterTabelaAluno(aluno, request.Nota, request.ConceitoId, request.NotaAnterior, request.ConceitoIdAnterior));
            descricao.AppendLine("<br/>Você precisa aceitar esta notificação para que a alteração seja considerada válida.");

            return await mediator.Send(new EnviarNotificacaoCommand(titulo,
                                                                    descricao.ToString(),
                                                                    Dominio.NotificacaoCategoria.Workflow_Aprovacao,
                                                                    Dominio.NotificacaoTipo.Fechamento,
                                                                    new Cargo[] { Cargo.CP, Cargo.Supervisor },
                                                                    ue.Dre.CodigoDre,
                                                                    ue.CodigoUe,
                                                                    request.Turma.CodigoTurma,
                                                                    WorkflowAprovacaoTipo.AlteracaoNotaConselho,
                                                                    request.ConselhoClasseNotaId));
        }

        private async Task<string> ObterTabelaAluno(AlunoReduzidoDto aluno, double? nota, long? conceitoId, double? notaAnterior, long? conceitoIdAnterior)
        {
            var valorAnterior = "";
            var valorNovo = "";

            if (conceitoId.HasValue || conceitoIdAnterior.HasValue)
            {
                var conceitos = await ObterConceitos();

                if (conceitoIdAnterior.HasValue)
                    valorAnterior = conceitos.FirstOrDefault(a => a.Id == conceitoIdAnterior)?.Descricao;

                if (conceitoId.HasValue)
                    valorNovo = conceitos.FirstOrDefault(a => a.Id == conceitoId)?.Descricao;
            }
            else
            {
                valorAnterior = notaAnterior?.ToString() ?? "";
                valorNovo = nota?.ToString() ?? "";
            }


            var descricao = new StringBuilder();
            descricao.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.AppendLine("<tbody>");
            descricao.AppendLine("<tr>");
            descricao.AppendLine("<td><strong>Estudante</strong></td>");
            descricao.AppendLine("<td style='text-align: center;'><strong>Valor anterior</strong></td>");
            descricao.AppendLine("<td style='text-align: center;'><strong>Novo valor</strong></td>");
            descricao.AppendLine("</tr>");
            descricao.AppendLine("<tr>");
            descricao.AppendLine($"<td>{aluno.NumeroAlunoChamada} - {aluno.Nome} ({aluno.CodigoAluno})</td>");
            descricao.AppendLine($"<td style='text-align: center;'>{valorAnterior}</td>");
            descricao.AppendLine($"<td style='text-align: center;'>{valorNovo}</td>");
            descricao.AppendLine("</tr>");
            descricao.AppendLine("<tbody>");
            descricao.AppendLine("</table>");

            return descricao.ToString();
        }

        private async Task<IEnumerable<Conceito>> ObterConceitos()
            => await mediator.Send(new ObterConceitosValoresQuery());

        private async Task<Ue> ObterUe(long ueId)
            => await mediator.Send(new ObterUeComDrePorIdQuery(ueId));

        private async Task<string> ObterComponente(long componenteCurricularCodigo)
            => await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(componenteCurricularCodigo));

        private async Task<AlunoReduzidoDto> ObterAluno(string alunoCodigo, int anoLetivo)
            => await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(alunoCodigo, anoLetivo));
    }
}
