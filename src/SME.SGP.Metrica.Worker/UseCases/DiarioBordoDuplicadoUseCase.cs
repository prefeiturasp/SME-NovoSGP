using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class DiarioBordoDuplicadoUseCase : IDiarioBordoDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioDiarioBordoDuplicado repositorioDuplicados;

        public DiarioBordoDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioDiarioBordoDuplicado repositorioDuplicados)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDuplicados = repositorioDuplicados ?? throw new System.ArgumentNullException(nameof(repositorioDuplicados));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioDuplicados.ExcluirTodos();

            var registrosDuplicados = await repositorioSGP.ObterDiariosBordoDuplicados();

            foreach(var registroDuplicado in registrosDuplicados)
                await repositorioDuplicados.InserirAsync(registroDuplicado);

            return true;
        }
    }
}
