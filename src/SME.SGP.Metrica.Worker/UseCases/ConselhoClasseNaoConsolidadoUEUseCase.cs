using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseNaoConsolidadoUEUseCase : IConselhoClasseNaoConsolidadoUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConselhoClasseNaoConsolidado repositorioConselhoNaoConsolidado;

        public ConselhoClasseNaoConsolidadoUEUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                     IRepositorioConselhoClasseNaoConsolidado repositorioConselhoNaoConsolidado)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioConselhoNaoConsolidado = repositorioConselhoNaoConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoNaoConsolidado));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var conselhosNaoConsolidados = await repositorioSGP.ObterConselhosClasseNaoConsolidados(ue.Id);
            foreach (var conselhoNaoConsolidado in conselhosNaoConsolidados)
                await repositorioConselhoNaoConsolidado.InserirAsync(conselhoNaoConsolidado);

            return true;
        }
    }
}
