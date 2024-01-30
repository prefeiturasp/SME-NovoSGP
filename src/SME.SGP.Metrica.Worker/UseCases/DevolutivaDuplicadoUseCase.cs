using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class DevolutivaDuplicadoUseCase : IDevolutivaDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioDevolutivaDuplicado repositorioDevolutiva;

        public DevolutivaDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioDevolutivaDuplicado repositorioDevolutiva)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioDevolutiva.ExcluirTodos();
            var registrosDuplicados = await repositorioSGP.ObterDevolutivaDuplicados();

            foreach (var registroDuplicado in registrosDuplicados)
            {
                await repositorioDevolutiva.InserirAsync(registroDuplicado);
            }

            return true;
        }
    }
}
