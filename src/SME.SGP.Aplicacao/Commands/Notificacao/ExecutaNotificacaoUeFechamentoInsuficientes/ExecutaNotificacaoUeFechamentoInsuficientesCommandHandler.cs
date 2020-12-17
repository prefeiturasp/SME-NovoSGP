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
    public class ExecutaNotificacaoUeFechamentoInsuficientesCommandHandler : IRequestHandler<ExecutaNotificacaoUeFechamentoInsuficientesCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoUeFechamentoInsuficientesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoUeFechamentoInsuficientesCommand request, CancellationToken cancellationToken)
        {
            await VerificaTurmasComPendenciaFechamento(request.PeriodosEncerrando, request.Modalidade, request.PercentualFechamentoInsuficiente);

            return true;
        }

        private async Task VerificaTurmasComPendenciaFechamento(IGrouping<long, PeriodoFechamentoBimestre> periodosEncerrando, ModalidadeTipoCalendario modalidade, double percentualFechamentoInsuficiente)
        {
            var dre = periodosEncerrando.First().PeriodoFechamento.Ue.Dre;

            var listaUes = new List<(bool notificar, Ue ue, int quantidadeTurmasPendentes)>();
            foreach(var periodoEncerrandoBimestre in periodosEncerrando.GroupBy(c => c.PeriodoEscolar.Bimestre))
            {
                foreach(var periodoEncerrando in periodoEncerrandoBimestre)
                {
                    var turmas = await mediator.Send(new ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(periodoEncerrando.PeriodoFechamento.UeId.Value,
                                                                                                               DateTime.Now.Year,
                                                                                                               periodoEncerrando.PeriodoEscolarId,
                                                                                                               modalidade.ObterModalidadesTurma(),
                                                                                                               DateTime.Now.Semestre()));

                    if (turmas != null && turmas.Any())
                        listaUes.Add(await VerificaTurmasComPendenciaFechamentoNaUe(turmas, periodoEncerrando.PeriodoFechamento.Ue, percentualFechamentoInsuficiente));
                }

                if (listaUes.Any(c => c.notificar))
                    await NotificarUesInsuficientes(listaUes, periodoEncerrandoBimestre.Key, dre, percentualFechamentoInsuficiente);

                if (EhFechamentoFinal(periodoEncerrandoBimestre.Key, modalidade))
                    await VerificaPendenciaFechamentoFinal(periodoEncerrandoBimestre, modalidade, dre, percentualFechamentoInsuficiente);
            }
        }

        private bool EhFechamentoFinal(int bimestre, ModalidadeTipoCalendario modalidade)
            => bimestre == (modalidade == ModalidadeTipoCalendario.EJA ? 2 : 4);

        private async Task<(bool notificar, Ue ue, int quantidadeTurmasPendentes)> VerificaTurmasComPendenciaFechamentoNaUe(IEnumerable<Turma> turmas, Ue ue, double percentualFechamentoInsuficiente)
        {
            var ano = turmas.First().AnoLetivo;
            var quantidadeTurmasUe = await mediator.Send(new ObterQuantidadeTurmasSeriadasNaUeQuery(ue.Id, ano));

            var indiceTurmasPendentes = turmas.Count() / (double)quantidadeTurmasUe * 100;

            return (indiceTurmasPendentes > percentualFechamentoInsuficiente, ue, turmas.Count());
        }


        private async Task VerificaPendenciaFechamentoFinal(IGrouping<int, PeriodoFechamentoBimestre> periodosEncerramentoUes, ModalidadeTipoCalendario modalidade, Dre dre, double percentualFechamentoInsuficiente)
        {
            var listaUes = new List<(bool notificar, Ue ue, int quantidadeTurmasPendentes)>();
            foreach (var periodoEncerramentoUe in periodosEncerramentoUes)
            {
                var turmas = await mediator.Send(new ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(periodoEncerramentoUe.PeriodoFechamento.UeId.Value,
                                                                                                           DateTime.Now.Year,
                                                                                                           null,
                                                                                                           modalidade.ObterModalidadesTurma(),
                                                                                                           DateTime.Now.Semestre()));
                if (turmas != null && turmas.Any())
                    listaUes.Add(await VerificaTurmasComPendenciaFechamentoNaUe(turmas, periodoEncerramentoUe.PeriodoFechamento.Ue, percentualFechamentoInsuficiente));
            }

            if (listaUes.Any(c => c.notificar))
                await NotificarUesInsuficientes(listaUes, null, dre, percentualFechamentoInsuficiente);
        }

        private async Task NotificarUesInsuficientes(List<(bool notificar, Ue ue, int quantidadeTurmasPendentes)> listaUes, int? bimestre, Dre dre, double percentualFechamentoInsuficiente)
        {
            var descricaoBimestre = bimestre.HasValue ?
                $"{bimestre}º bimestre" :
                "bimestre final";

            var titulo = $"UEs com processo de fechamento pendente no {descricaoBimestre} ({dre.Abreviacao})";
            var mensagem = new StringBuilder($"As UEs da <b>{dre.Abreviacao}</b> abaixo estão com menos de {percentualFechamentoInsuficiente}% do fechamento do <b>{descricaoBimestre}</b> concluído:<br/>");

            mensagem.Append(ObterHeaderTabela());
            foreach(var notificarUe in listaUes.Where(c => c.notificar))
            {
                mensagem.Append("<tr>");
                mensagem.Append($"<td>{notificarUe.ue.TipoEscola.ShortName()} {notificarUe.ue.Nome}</td>");
                mensagem.Append($"<td style='text-align: center'>{notificarUe.quantidadeTurmasPendentes}</td>");
                mensagem.Append("</tr>");
            }
            mensagem.Append("</table>");

            var supervisores = await ObterUsuariosSupervisores(dre.CodigoDre);
            if (supervisores != null && supervisores.Any())
                await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, supervisores, dre.CodigoDre));
        }

        private async Task<IEnumerable<long>> ObterUsuariosSupervisores(String codigoDre)
        {
            var supervisores = await mediator.Send(new ObterSupervisoresPorDreQuery(codigoDre));

            var listaUsuarios = new List<long>();
            foreach (var supervisor in supervisores.GroupBy(c => c.SupervisorId))
                listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(supervisor.Key)));

            return listaUsuarios;
        }

        private string ObterHeaderTabela()
            => "<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>"
             + "<tr style='font-weight: bold'><td>Unidade Escolar</td><td>Turmas Pendentes</td></tr>";
    }
}
