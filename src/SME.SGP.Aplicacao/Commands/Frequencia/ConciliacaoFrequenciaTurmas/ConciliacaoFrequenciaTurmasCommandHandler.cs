using MediatR;
using Sentry;
using SME.SGP.Dominio;
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
                    var turmasDaModalidade = await ObterTurmasPorModalidade(modalidade.Key, request.Data.Year);

                    foreach (var periodoEscolar in modalidade)
                        await PublicarFilaConciliacaoTurmas(turmasDaModalidade, periodoEscolar.Bimestre, periodoEscolar.DataInicio, periodoEscolar.DataFim);
                }

                return true;

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }        
        }

        private async Task<bool> PublicarFilaConciliacaoTurmas(IEnumerable<Turma> turmasDaModalidade, int bimestre, DateTime dataInicio, DateTime dataFim)
        {
            Parallel.ForEach(turmasDaModalidade, new ParallelOptions { MaxDegreeOfParallelism = 3 }
                , async turma =>
                    await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaCommand(turma.CodigoTurma, bimestre, dataInicio, dataFim))
                );

            return true;
        }

        private async Task<IEnumerable<Turma>> ObterTurmasPorModalidade(ModalidadeTipoCalendario modalidadeTipoCalendario, int ano)
        {
            var modalidades = modalidadeTipoCalendario.ObterModalidades();

            return await mediator.Send(new ObterTurmasPorAnoModalidadeQuery(ano, modalidades));
        }

        private async Task<IEnumerable<IGrouping<ModalidadeTipoCalendario, PeriodoEscolarModalidadeDto>>> ObterPeriodosPassadosPorModalidade(DateTime data)
        {
            var modalidadesPeriodosPassados = await mediator.Send(new ObterModalidadeEPeriodosPassadosNoAnoQuery(data));

            return modalidadesPeriodosPassados.GroupBy(a => a.Modalidade);
        }
    }
}
