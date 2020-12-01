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
    public class ExecutaNotificacaoPeriodoFechamentoEncerrandoCommandHandler : IRequestHandler<ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoPeriodoFechamentoEncerrandoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasComInicioFechamentoQuery(request.PeriodoIniciandoBimestre.PeriodoFechamento.UeId.Value,
                request.PeriodoIniciandoBimestre.PeriodoEscolarId,
                request.ModalidadeTipoCalendario.ObterModalidadesTurma()));

            if (turmas != null && turmas.Any())
            {
                await EnviarNotificacaoTurmas(turmas, request.PeriodoIniciandoBimestre.PeriodoEscolar, request.PeriodoIniciandoBimestre,  request.PeriodoIniciandoBimestre.PeriodoFechamento.Ue);
            }
            //    await EnviarNotificacaoTurmas(turmas, componentes, request.PeriodoIniciandoBimestre.PeriodoEscolar, request.PeriodoIniciandoBimestre.PeriodoFechamento.Ue);

            return true;
        }

        private async Task EnviarNotificacaoTurmas(IEnumerable<Turma> turmas, PeriodoEscolar periodoEscolar, PeriodoFechamentoBimestre periodoFechamentoBimestre, Ue ue)
        {
            var titulo = $"Início do período de fechamento do {periodoEscolar.Bimestre}º bimestre - {ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var mensagem = @$"O fechamento do <b>{periodoEscolar.Bimestre}º bimestre</b> na <b>{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})</b> irá iniciar no dia <b>{periodoFechamentoBimestre.InicioDoFechamento.Date}</b>.";

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
                    mensagem.Append($"<td rowspan='{componentesCurriculares.Count()}'>{turma.Nome}</td>");

                mensagem.Append($"<td>{componenteCurricular.Descricao}</td>");
                mensagem.Append($"<td>{await ObterSituacaoFechamento(turma, componenteCurricular, periodoEscolar)}</td>");

                if (componenteCurricular.Codigo == componentesCurriculares.First().Codigo)
                    mensagem.Append($"<td rowspan='{componentesCurriculares.Count()}'>{await ObterSituacaoConselhoClasse(turma, periodoEscolar)}</td>");
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
            return $"<tr><td colspan='4'><b>{modalidade}</b></td></tr><tr><td><b>Turma</b></td><td><b>Componente curricular</b></td><td><b>Fechamento</b></td><td><b>Conselho de classe</b></td></tr>";
        }

        private async Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesDaTurma(Turma turma, Ue ue)
            => await mediator.Send(new ObterComponentesCurricularesPorTurmaECodigoUeQuery(new[] { turma.CodigoTurma }, ue.CodigoUe));
    }
}
