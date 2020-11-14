using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarPendenciaAulaDiasNaoLetivosCommandHandler : IRequestHandler<VerificarPendenciaAulaDiasNaoLetivosCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoNotificacao servicoNotificacao;

        public VerificarPendenciaAulaDiasNaoLetivosCommandHandler(IMediator mediator, IServicoNotificacao servicoNotificacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<bool> Handle(VerificarPendenciaAulaDiasNaoLetivosCommand request, CancellationToken cancellationToken)
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Fundamental, anoAtual, 0));
            if (tipoCalendarioId > 0)
                await VerificaPendenciasAulaDiasNaoLetivos(tipoCalendarioId, ModalidadeTipoCalendario.FundamentalMedio, anoAtual);

            tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.EJA, anoAtual, 1));
            if (tipoCalendarioId > 0)
                await VerificaPendenciasAulaDiasNaoLetivos(tipoCalendarioId, ModalidadeTipoCalendario.EJA, anoAtual);

            tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.EJA, anoAtual, 2));
            if (tipoCalendarioId > 0)
                await VerificaPendenciasAulaDiasNaoLetivos(tipoCalendarioId, ModalidadeTipoCalendario.EJA, anoAtual);

            return true;
        }

        private async Task VerificaPendenciasAulaDiasNaoLetivos(long tipoCalendarioId, ModalidadeTipoCalendario modalidadeCalendario, int anoAtual)
        {
            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));
            var aulas = await mediator.Send(new ObterAulasReduzidaPorTipoCalendarioQuery(tipoCalendarioId));
            string instrucao = "Você precisa excluir estas aulas no Calendário do Professor ou entrar em contato com a gestão da UE para ajustar o calendário da escola.";

            var diasComEventosNaoLetivos = diasLetivosENaoLetivos.Where(e => e.EhNaoLetivo);

            if (aulas != null)
            {
                var listaAgrupada = aulas.Where(a => diasComEventosNaoLetivos.Any(d => d.Data == a.Data)).GroupBy(x => new { x.TurmaId, x.DisciplinaId }).ToList();

                var motivos = diasComEventosNaoLetivos.Where(d => aulas.Any(a => a.Data == d.Data)).Select(d => new { data = d.Data, motivo = d.Motivo }).ToList();

                foreach (var turmas in listaAgrupada)
                {
                    var pendenciaId = await mediator.Send(new ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery(turmas.Key.TurmaId, turmas.Key.DisciplinaId));

                    var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turmas.Key.TurmaId));

                    if (pendenciaId == 0)
                        pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AulaNaoLetivo, TipoPendencia.AulaNaoLetivo.Name(), instrucao));

                    foreach (var aula in turmas)
                    {
                        var pendenciaAulaId = await mediator.Send(new ObterPendenciaAulaPorAulaIdQuery(aula.aulaId, TipoPendencia.AulaNaoLetivo));
                        var motivo = motivos.FirstOrDefault(m => m.data == aula.Data)?.motivo;
                        if (pendenciaAulaId == 0)
                        {
                            await mediator.Send(new SalvarPendenciaAulaDiasNaoLetivosCommand(aula.aulaId, motivo, pendenciaId));

                            await mediator.Send(new RelacionaPendenciaUsuarioCommand(TipoParametroSistema.GerarPendenciaAulasDiasNaoLetivos, ue.CodigoUe, pendenciaId, aula.aulaId));
                        }
                    }
                }
            }
        }
    }
}
