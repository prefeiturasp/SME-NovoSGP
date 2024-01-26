using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FrequenciaAlunoDuplicadoUEUseCase : IFrequenciaAlunoDuplicadoUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFrequenciaAlunoDuplicado repositorioDuplicados;
        private readonly IMediator mediator;

        public FrequenciaAlunoDuplicadoUEUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                 IRepositorioFrequenciaAlunoDuplicado repositorioDuplicados,
                                                 IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDuplicados = repositorioDuplicados ?? throw new System.ArgumentNullException(nameof(repositorioDuplicados));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var registrosDuplicados = await repositorioSGP.ObterFrequenciaAlunoDuplicados(ue.Id);
            foreach(var registroDuplicado in registrosDuplicados)
            {
                await repositorioDuplicados.InserirAsync(registroDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaFrequenciaAlunoDuplicado, registroDuplicado));
            }

            return true;
        }
    }
}
