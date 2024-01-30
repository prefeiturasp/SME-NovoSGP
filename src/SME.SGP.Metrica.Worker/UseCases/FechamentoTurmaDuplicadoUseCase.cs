using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentoTurmaDuplicadoUseCase : IFechamentoTurmaDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentoTurmaDuplicado repositorioFechamentoTurmaDuplicado;
        private readonly IMediator mediator;

        public FechamentoTurmaDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                               IRepositorioFechamentoTurmaDuplicado repositorioFechamentoTurmaDuplicado,
                                               IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentoTurmaDuplicado = repositorioFechamentoTurmaDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioFechamentoTurmaDuplicado.ExcluirTodos();

            var fechamentosTurmaDuplicados = await repositorioSGP.ObterFechamentosTurmaDuplicados();
            foreach(var fechamentoTurmaDuplicado in fechamentosTurmaDuplicados)
            {
                await repositorioFechamentoTurmaDuplicado.InserirAsync(fechamentoTurmaDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaFechamentoTurmaDuplicado, fechamentoTurmaDuplicado));
            }

            return true;
        }
    }
}
