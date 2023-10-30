using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentoAlunoDuplicadoUEUseCase : IFechamentoAlunoDuplicadoUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentoAlunoDuplicado repositorioFechamentoAlunoDuplicado;
        private readonly IMediator mediator;

        public FechamentoAlunoDuplicadoUEUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                 IRepositorioFechamentoAlunoDuplicado repositorioFechamentoAlunoDuplicado,
                                                 IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentoAlunoDuplicado = repositorioFechamentoAlunoDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoAlunoDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();
            var fechamentosAlunoDuplicados = await repositorioSGP.ObterFechamentosAlunoDuplicados(ue.Id);

            foreach(var fechamentoAlunoDuplicado in fechamentosAlunoDuplicados)
            {
                await repositorioFechamentoAlunoDuplicado.InserirAsync(fechamentoAlunoDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaFechamentoAlunoDuplicado, fechamentoAlunoDuplicado));
            }

            return true;
        }
    }
}
