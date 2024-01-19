using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConsolidacaoFrequenciaAlunoMensalInconsistenteUseCase : IConsolidacaoFrequenciaAlunoMensalInconsistenteUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensalInconsistente repositorioInconsistencia;
        private readonly IMediator mediator;

        public ConsolidacaoFrequenciaAlunoMensalInconsistenteUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                                     IRepositorioConsolidacaoFrequenciaAlunoMensalInconsistente repositorioInconsistencia,
                                                                     IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioInconsistencia = repositorioInconsistencia ?? throw new System.ArgumentNullException(nameof(repositorioInconsistencia));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioInconsistencia.ExcluirTodos();

            var ues = await repositorioSGP.ObterUesIds();
            foreach(var ue in ues)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.ConsolidacaoFrequenciaAlunoMensalInconsistenteUE, new FiltroIdDto(ue)));

            return true;
        }
    }
}
