using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FrequenciaAlunoInconsistenteUseCase : IFrequenciaAlunoInconsistenteUseCase
    {
        private readonly IRepositorioFrequenciaAlunoInconsistente repositorioFrequenciaAlunoInconsistente;
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IMediator mediator;

        public FrequenciaAlunoInconsistenteUseCase(IRepositorioFrequenciaAlunoInconsistente repositorioFrequenciaAlunoInconsistente,
                                                   IRepositorioSGPConsulta repositorioSGP,
                                                   IMediator mediator)
        {
            this.repositorioFrequenciaAlunoInconsistente = repositorioFrequenciaAlunoInconsistente ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoInconsistente));
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioFrequenciaAlunoInconsistente.ExcluirTodos();

            var ues = await repositorioSGP.ObterUesIds();
            foreach (var ue in ues)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.FrequenciaAlunoInconsistenteUE, new FiltroIdDto(ue)));

            return true;
        }
    }
}
