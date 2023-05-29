﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasCommandHandler : IRequestHandler<ConciliacaoFrequenciaTurmasCommand, bool>
    {
        private readonly IMediator mediator;

        public ConciliacaoFrequenciaTurmasCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ConciliacaoFrequenciaTurmasCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var periodosPorModalidade = await ObterPeriodosPassadosPorModalidade(request.Data);
                foreach (var modalidade in periodosPorModalidade)
                {
                    var turmasDaModalidade = (await ObterTurmasPorModalidade(modalidade.Key, request.Data.Year, request.TurmaCodigo)).ToList();

                    if (turmasDaModalidade != null && turmasDaModalidade.Any())
                    {
                        if (request.Bimestral)
                        {
                            foreach (var periodoEscolar in modalidade)
                                await PublicarFilaConciliacaoPeriodo(turmasDaModalidade, periodoEscolar.Bimestre, periodoEscolar.DataInicio, periodoEscolar.DataFim, request.ComponenteCurricularId);
                        }

                        if (request.Mensal)
                        {
                            foreach (var mes in ObterMesesAnteriores(request.Data))
                                await PublicarFilaConciliacaoMensal(turmasDaModalidade, mes);
                        }
                    }

                };

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na consolidação de Frequência da turma.", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                throw;
            }
        }

        private async Task PublicarFilaConciliacaoPeriodo(List<string> turmasDaModalidade, int bimestre, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            var dto = new ConciliacaoFrequenciaTurmaPorPeriodoDto(turmasDaModalidade, bimestre, dataInicio, dataFim, componenteCurricularId);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaPorPeriodo, dto, Guid.NewGuid(), null));
        }

        private async Task PublicarFilaConciliacaoMensal(List<string> turmasDaModalidade, int mes)
        {
            var dto = new ConciliacaoFrequenciaTurmaMensalDto(turmasDaModalidade, mes);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaMes, dto, Guid.NewGuid(), null));
        }

        private async Task<IEnumerable<string>> ObterTurmasPorModalidade(ModalidadeTipoCalendario modalidadeTipoCalendario, int ano, string turmaCodigo)
        {
            var modalidades = modalidadeTipoCalendario.ObterModalidades();

            return await mediator.Send(new ObterCodigosTurmasPorAnoModalidadeQuery(ano, modalidades, turmaCodigo));
        }

        private async Task<IEnumerable<IGrouping<ModalidadeTipoCalendario, PeriodoEscolarModalidadeDto>>> ObterPeriodosPassadosPorModalidade(DateTime data)
        {
            var modalidadesPeriodosPassados = await mediator.Send(new ObterModalidadeEPeriodosPassadosNoAnoQuery(data));

            return modalidadesPeriodosPassados.GroupBy(a => a.Modalidade);
        }

        private IEnumerable<int> ObterMesesAnteriores(DateTime data)
        {
            for (var mes = 1; mes <= data.Month; mes++)
                yield return mes;
        }
    }
}
