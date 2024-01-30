using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class DevolutivaSemDiarioUseCase : IDevolutivaSemDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioDevolutivaSemDiario repositorioDevolutiva;

        public DevolutivaSemDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioDevolutivaSemDiario repositorioDevolutiva)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioDevolutiva.ExcluirTodos();
            var registrosSemDiario = await repositorioSGP.ObterDevolutivaSemDiario();

            foreach (var registro in registrosSemDiario)
            {
                await repositorioDevolutiva.InserirAsync(registro);
            }

            return true;
        }
    }
}
