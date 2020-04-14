using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasseNota : IServicoConselhoClasse
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ServicoConselhoClasseNota(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            if (conselhoClasseAluno.Id == 0)
            {
            }
            return default;
        }
    }
}