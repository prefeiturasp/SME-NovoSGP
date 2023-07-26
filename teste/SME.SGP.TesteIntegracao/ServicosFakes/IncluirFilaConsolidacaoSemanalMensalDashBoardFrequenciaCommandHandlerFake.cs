using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao
{
    public class IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommandHandlerFake : IRequestHandler<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConsolidarFrequenciaPorTurmaSemanalUseCase consolidarFrequenciaPorTurmaSemanalUseCase;
        private readonly IConsolidarFrequenciaPorTurmaMensalUseCase consolidarFrequenciaPorTurmaMensalUseCase;

        public IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommandHandlerFake(IMediator mediator, IConsolidarFrequenciaPorTurmaSemanalUseCase consolidarFrequenciaPorTurmaSemanalUseCase,IConsolidarFrequenciaPorTurmaMensalUseCase consolidarFrequenciaPorTurmaMensalUseCase)  
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consolidarFrequenciaPorTurmaSemanalUseCase = consolidarFrequenciaPorTurmaSemanalUseCase ?? throw new ArgumentNullException(nameof(consolidarFrequenciaPorTurmaSemanalUseCase));
            this.consolidarFrequenciaPorTurmaMensalUseCase = consolidarFrequenciaPorTurmaMensalUseCase ?? throw new ArgumentNullException(nameof(consolidarFrequenciaPorTurmaMensalUseCase));
        }

        public async Task<bool> Handle(IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var parametro = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(request.EhModalidadeInfantil ? TipoParametroSistema.PercentualFrequenciaMinimaInfantil : TipoParametroSistema.PercentualFrequenciaCritico, request.AnoLetivo));

            var filtroTurma = JsonSerializer.Serialize(new FiltroConsolidacaoFrequenciaTurma(request.TurmaId, request.CodigoTurma, double.Parse(parametro.Valor),request.DataAula));
                
            await consolidarFrequenciaPorTurmaSemanalUseCase.Executar(new MensagemRabbit(filtroTurma));
            
            await consolidarFrequenciaPorTurmaMensalUseCase.Executar(new MensagemRabbit(filtroTurma));

            return true;
        }
    }
}
