using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorBimestresUeAlunoTurmaUseCase : IObterNotasPorBimestresUeAlunoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterNotasPorBimestresUeAlunoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Executar(NotaConceitoPorBimestresAlunoTurmaDto dto)
        {
            return await mediator.Send(new ObterNotasPorBimestresUeAlunoTurmaQuery(dto.Bimestres, dto.TurmaCodigo, dto.UeCodigo, dto.AlunoCodigo));
        }
    }
}
