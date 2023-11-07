using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConsolidacaoConselhoClasseNotaNuloUseCase : IConsolidacaoConselhoClasseNotaNuloUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConsolidacaoConselhoClasseNotaNulos repositorioNotaNulo;

        public ConsolidacaoConselhoClasseNotaNuloUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                         IRepositorioConsolidacaoConselhoClasseNotaNulos repositorioNotaNulo)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioNotaNulo = repositorioNotaNulo ?? throw new System.ArgumentNullException(nameof(repositorioNotaNulo));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioNotaNulo.ExcluirTodos();
            var consolidacoesNotasNulos = await repositorioSGP.ObterConsolidacaoCCNotasNulos();

            foreach (var consolidacaoNotasNulos in consolidacoesNotasNulos)
                await repositorioNotaNulo.InserirAsync(consolidacaoNotasNulos);

            return true;
        }
    }
}
