using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake : IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>
    {
        public readonly IMediator mediator;
        private readonly IConsolidarFrequenciaAlunoPorTurmaEMesUseCase consolidarFrequenciaAlunoPorTurmaEMesUseCase;
        private readonly ICalculoFrequenciaTurmaDisciplinaUseCase calculoFrequenciaTurmaDisciplinaUseCase;
        

        public IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake(IMediator mediator,IConsolidarFrequenciaAlunoPorTurmaEMesUseCase consolidarFrequenciaAlunoPorTurmaEMesUseCase, ICalculoFrequenciaTurmaDisciplinaUseCase calculoFrequenciaTurmaDisciplinaUseCase)  
        {
            this.calculoFrequenciaTurmaDisciplinaUseCase = calculoFrequenciaTurmaDisciplinaUseCase ?? throw new ArgumentNullException(nameof(calculoFrequenciaTurmaDisciplinaUseCase));
            this.consolidarFrequenciaAlunoPorTurmaEMesUseCase = consolidarFrequenciaAlunoPorTurmaEMesUseCase ?? throw new ArgumentNullException(nameof(consolidarFrequenciaAlunoPorTurmaEMesUseCase));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaCalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var calculo = new CalcularFrequenciaPorTurmaCommand(request.Alunos, request.DataAula, request.TurmaId, request.DisciplinaId);
            await calculoFrequenciaTurmaDisciplinaUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(calculo)));

            var consolidacao = new FiltroConsolidacaoFrequenciaAlunoMensal(request.TurmaId, request.DataAula.Month);
            await consolidarFrequenciaAlunoPorTurmaEMesUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(consolidacao)));

            return true;
        }
    }
}
