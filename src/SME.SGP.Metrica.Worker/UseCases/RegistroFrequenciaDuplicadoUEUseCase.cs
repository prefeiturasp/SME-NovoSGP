using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class RegistroFrequenciaDuplicadoUEUseCase : IRegistroFrequenciaDuplicadoUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioRegistroFrequenciaDuplicado repositorioDuplicados;
        private readonly IMediator mediator;

        public RegistroFrequenciaDuplicadoUEUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                    IRepositorioRegistroFrequenciaDuplicado repositorioDuplicados,
                                                    IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDuplicados = repositorioDuplicados ?? throw new System.ArgumentNullException(nameof(repositorioDuplicados));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var registrosDuplicados = await repositorioSGP.ObterRegistroFrequenciaDuplicados(ue.Id);

            foreach(var registroDuplicado in registrosDuplicados)
            {
                await repositorioDuplicados.InserirAsync(registroDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaRegistroFrequenciaDuplicado, registroDuplicado));
            }

            return true;
        }
    }
}
