using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class RegistroFrequenciaAlunoDuplicadoTurmaUseCase : IRegistroFrequenciaAlunoDuplicadoTurmaUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioRegistroFrequenciaAlunoDuplicado repositorioDuplicados;
        private readonly IMediator mediator;

        public RegistroFrequenciaAlunoDuplicadoTurmaUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                            IRepositorioRegistroFrequenciaAlunoDuplicado repositorioDuplicados,
                                                            IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDuplicados = repositorioDuplicados ?? throw new System.ArgumentNullException(nameof(repositorioDuplicados));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var turma = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var registrosDuplicados = await repositorioSGP.ObterRegistroFrequenciaAlunoDuplicados(turma.Id);

            foreach(var registroDuplicado in registrosDuplicados)
            {
                await repositorioDuplicados.InserirAsync(registroDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaRegistroFrequenciaAlunoDuplicado, registroDuplicado));
            }

            return true;
        }
    }
}
