using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasseNota
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ServicoConselhoClasseNota(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task SalvarNovaNotaPosConselhoAsync(PosConselhosNotasPersistirDto posConselhosNotasPersistirDto)
        {
            //var conselhoClasse = await repositorioConselhoClasse.ObterPorTurmaBimestreAsync(posConselhosNotasPersistirDto.TurmaCodigo, posConselhosNotasPersistirDto.Bimestre);
        }
    }
}