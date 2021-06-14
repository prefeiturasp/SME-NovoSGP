using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarAtualPorTurmaQuery(turma, DateTime.Now.Date));

            return periodoEscolar == null ? null :
                new BimestreDto() { Id = periodoEscolar.Id, Numero = periodoEscolar.Bimestre };
        }
    }
}
