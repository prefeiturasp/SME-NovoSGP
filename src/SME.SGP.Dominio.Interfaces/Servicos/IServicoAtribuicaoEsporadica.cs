using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAtribuicaoEsporadica
    {
        Task Salvar(AtribuicaoEsporadica atribuicaoEsporadica, int anoLetivo, bool ehInfantil);
    }
}