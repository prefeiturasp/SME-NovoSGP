using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoCompensacaoAusencia
    {
        Task Salvar(long id, CompensacaoAusenciaDto compensacaoDto);
    }
}
