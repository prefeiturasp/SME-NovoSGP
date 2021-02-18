using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoAndamentoFechamentoCommandHandler : IRequestHandler<ExecutaNotificacaoAndamentoFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoAndamentoFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoAndamentoFechamentoCommand request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(request.PeriodoEncerrandoBimestre.PeriodoFechamento.UeId.Value,
                                                                                                       DateTime.Now.Year,
                                                                                                       request.PeriodoEncerrandoBimestre.PeriodoEscolarId,
                                                                                                       request.ModalidadeTipoCalendario.ObterModalidadesTurma(),
                                                                                                       DateTime.Now.Semestre()));
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());

            if (turmas != null && turmas.Any())
                await EnviarNotificacaoTurmas(turmas, componentes, request.PeriodoEncerrandoBimestre.PeriodoEscolar, request.PeriodoEncerrandoBimestre.PeriodoFechamento.Ue);

            return true;
        }

        private async Task EnviarNotificacaoTurmas(IEnumerable<Turma> turmas, IEnumerable<ComponenteCurricularDto> componentes, PeriodoEscolar periodoEscolar, Ue ue)
        {
            var titulo = $"Situação do processo de fechamento ({periodoEscolar.Bimestre}º Bimestre)";
            var mensagem = new StringBuilder($"Segue abaixo o situação do processo de fechamento do <b>{periodoEscolar.Bimestre}° bimestre da {ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao}):</b>");

            mensagem.Append("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            foreach (var turmasPorModalidade in turmas.GroupBy(c => c.ModalidadeCodigo))
            {
                mensagem.Append(ObterHeaderModalidade(turmasPorModalidade.Key.Name()));

                foreach (var turma in turmasPorModalidade)
                {
                    mensagem.Append(await MontarLinhaDaTurma(turma, componentes, ue, periodoEscolar));
                }
            }
            mensagem.Append("</table>");

            await EnviarNotificacao(titulo, mensagem.ToString(), ue.Dre.CodigoDre, ue.CodigoUe);
        }

        private async Task EnviarNotificacao(string titulo, string mensagem, string codigoDre, string codigoUe)
        {
            var cargos = new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };
            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, cargos, codigoDre, codigoUe));
        }

        private async Task<string> MontarLinhaDaTurma(Turma turma, IEnumerable<ComponenteCurricularDto> componentes, Ue ue, PeriodoEscolar periodoEscolar)
        {
            var mensagem = new StringBuilder();
            var componentesDaTurma = await ObterComponentesDaTurma(turma, ue);
            var componentesCurriculares = componentes.Where(c => componentesDaTurma.Any(t => t.Codigo == c.Codigo));

            foreach (var componenteCurricular in componentesCurriculares)
            {
                mensagem.Append("<tr>");
                if (componenteCurricular.Codigo == componentesCurriculares.First().Codigo)
                    mensagem.Append($"<td style='padding:2px' rowspan='{componentesCurriculares.Count()}'>{turma.Nome}</td>");

                mensagem.Append($"<td style='padding:2px'>{componenteCurricular.Descricao}</td>");
                mensagem.Append($"<td style='padding:2px'>{await ObterSituacaoFechamento(turma, componenteCurricular, periodoEscolar)}</td>");

                if (componenteCurricular.Codigo == componentesCurriculares.First().Codigo)
                    mensagem.Append($"<td  style='padding:2px' rowspan='{componentesCurriculares.Count()}'>{await ObterSituacaoConselhoClasse(turma, periodoEscolar)}</td>");
                mensagem.Append("</tr>");
            }

            return mensagem.ToString();
        }

        private async Task<string> ObterSituacaoFechamento(Turma turma, ComponenteCurricularDto componenteCurricular, PeriodoEscolar periodoEscolar)
        {
            var situacao = await mediator.Send(new ObterSituacaoFechamentoTurmaComponenteQuery(turma.Id, long.Parse(componenteCurricular.Codigo), periodoEscolar.Id));
            return situacao.Name();
        }

        private async Task<string> ObterSituacaoConselhoClasse(Turma turma, PeriodoEscolar periodoEscolar)
        {
            var situacao = await mediator.Send(new ObterSituacaoConselhoClasseQuery(turma.Id, periodoEscolar.Id));
            return situacao.Name();
        }

        private string ObterHeaderModalidade(string modalidade)
        {
            return $"<tr><td colspan='4' style='text-align:center;padding:2px'><b>{modalidade}</b></td></tr><tr><td><b>Turma</b></td><td style='padding:2px'><b>Componente curricular</b></td><td style='padding:2px'><b>Fechamento</b></td><td style='padding:2px' ><b>Conselho de classe</b></td></tr>";
        }

        private async Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesDaTurma(Turma turma, Ue ue)
            => await mediator.Send(new ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery(new[] { turma.CodigoTurma }, ue.CodigoUe));
    }
}
