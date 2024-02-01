using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentoTurmaDisciplinaDuplicadoUseCase : IFechamentoTurmaDisciplinaDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentoTurmaDisciplinaDuplicado repositorioFechamentoTurmaDisciplinaDuplicado;
        private readonly IMediator mediator;

        public FechamentoTurmaDisciplinaDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
            IRepositorioFechamentoTurmaDisciplinaDuplicado repositorioFechamentoTurmaDisciplinaDuplicado,
            IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentoTurmaDisciplinaDuplicado = repositorioFechamentoTurmaDisciplinaDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplinaDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioFechamentoTurmaDisciplinaDuplicado.ExcluirTodos();

            var fechamentosTurmaDisciplinaDuplicados = await repositorioSGP.ObterFechamentosTurmaDisciplinaDuplicados();

            foreach(var fechamentoTurmaDisciplinaDuplicados in fechamentosTurmaDisciplinaDuplicados)
            {
                await repositorioFechamentoTurmaDisciplinaDuplicado.InserirAsync(fechamentoTurmaDisciplinaDuplicados);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaFechamentoTurmaDisciplinaDuplicado, fechamentoTurmaDisciplinaDuplicados));
            }

            return true;
        }
    }
}
