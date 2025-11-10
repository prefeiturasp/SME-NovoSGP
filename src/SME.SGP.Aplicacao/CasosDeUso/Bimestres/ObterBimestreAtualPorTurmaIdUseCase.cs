using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualPorTurmaIdUseCase : IObterBimestreAtualPorTurmaIdUseCase
    {
        private readonly IMediator mediator;

        public ObterBimestreAtualPorTurmaIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<BimestreDto> Executar(long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            if (turma.EhNulo())
                throw new NegocioException("A turma informada não foi encontrada");

            var periodoEscolar = turma.AnoLetivo.Equals(DateTime.Today.Year) ? await mediator.Send(new ObterPeriodoEscolarAtualQuery(turmaId, DateTime.Now.Date)) : null;

            return periodoEscolar.EhNulo() ? null :
                new BimestreDto() { Id = periodoEscolar?.Id ?? 0, Numero = periodoEscolar?.Bimestre ?? 0 };
        }
    }
}
