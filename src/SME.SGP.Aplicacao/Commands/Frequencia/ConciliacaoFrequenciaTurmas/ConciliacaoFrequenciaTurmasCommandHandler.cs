using MediatR;
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
                        foreach (var periodoEscolar in modalidade)
                            await PublicarFilaConciliacaoTurmas(turmasDaModalidade, periodoEscolar.Bimestre, periodoEscolar.DataInicio, periodoEscolar.DataFim, request.ComponenteCurricularId);
                };

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na consolidação de Frequência da turma.", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                throw;
            }
        }

        private Task<bool> PublicarFilaConciliacaoTurmas(IEnumerable<string> turmasDaModalidade, int bimestre, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            Parallel.ForEach(turmasDaModalidade, async turma =>
                await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaCommand(turma, bimestre, componenteCurricularId, dataInicio, dataFim)));

            return Task.FromResult(true);
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
    }
}
