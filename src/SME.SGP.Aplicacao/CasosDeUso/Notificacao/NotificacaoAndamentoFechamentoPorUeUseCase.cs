using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoAndamentoFechamentoPorUeUseCase : AbstractUseCase, INotificacaoAndamentoFechamentoPorUeUseCase
    {
        public NotificacaoAndamentoFechamentoPorUeUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dto = param.ObterObjetoMensagem<NotificacaoAndamentoFechamentoPorUeDto>();
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(dto.PeriodoEscolarId));
            var ue = await mediator.Send(new ObterUePorIdQuery(dto.UeId));
            var titulo = $"Situação do processo de fechamento ({periodoEscolar.Bimestre}º Bimestre)";
            var turmas = await mediator.Send(new ObterTurmasPorIdsQuery(dto.TurmasIds));
            ue.Dre = await mediator.Send(new ObterDREPorIdQuery(ue.DreId));
            var mensagem = new StringBuilder($"Segue abaixo a situação do processo de fechamento do <b>{periodoEscolar.Bimestre}° bimestre da {ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao}):</b>");
            mensagem.Append("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");

            foreach (var turmasPorModalidade in turmas.GroupBy(c => c.ModalidadeCodigo))
            {
                mensagem.Append(ObterHeaderModalidade(turmasPorModalidade.Key.Name()));

                foreach (var turma in turmasPorModalidade.OrderBy(t => t.Nome))
                    mensagem.Append(await MontarLinhaDaTurma(turma, dto.Componentes, ue, periodoEscolar));

                mensagem.Append("</table>");
                await EnviarNotificacao(titulo, mensagem.ToString(), ue.Dre.CodigoDre, ue.CodigoUe);
            }

            return true;
        }

        private string ObterHeaderModalidade(string modalidade)
        {
            return $"<tr><td colspan='4' style='text-align:center;padding:2px'><b>{modalidade}</b></td></tr><tr><td><b>Turma</b></td><td style='padding:2px'><b>Componente curricular</b></td><td style='padding:2px'><b>Fechamento</b></td><td style='padding:2px' ><b>Conselho de classe</b></td></tr>";
        }

        private async Task<string> MontarLinhaDaTurma(Turma turma, IEnumerable<ComponenteCurricularDto> componentes, Ue ue, PeriodoEscolar periodoEscolar)
        {
            var mensagem = new StringBuilder();
            var componentesDaTurma = await ObterComponentesDaTurma(turma, ue);
            var componentesCurriculares = componentes.Where(c => componentesDaTurma.Any(t => t.Codigo == c.Codigo)).OrderBy(cc => cc.Descricao);

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

        private async Task EnviarNotificacao(string titulo, string mensagem, string codigoDre, string codigoUe)
        {
            var cargos = new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };
            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, cargos, codigoDre, codigoUe));
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

        private async Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesDaTurma(Turma turma, Ue ue)
            => await mediator.Send(new ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery(new[] { turma.CodigoTurma }, ue.CodigoUe));
    }
}
