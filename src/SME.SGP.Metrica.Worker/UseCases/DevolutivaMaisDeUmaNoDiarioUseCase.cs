using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class DevolutivaMaisDeUmaNoDiarioUseCase : IDevolutivaMaisDeUmaNoDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioDevolutivaMaisDeUmaNoDiario repositorioDevolutiva;

        public DevolutivaMaisDeUmaNoDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioDevolutivaMaisDeUmaNoDiario repositorioDevolutiva)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioDevolutiva.ExcluirTodos();
            var registrosMaisDeUm = await repositorioSGP.ObterDevolutivaMaisDeUmaNoDiario();

            foreach (var registro in registrosMaisDeUm)
            {
                await repositorioDevolutiva.InserirAsync(registro);
            }

            return true;
        }
    }
}
