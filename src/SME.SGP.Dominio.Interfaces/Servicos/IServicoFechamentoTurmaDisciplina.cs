using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoTurmaDisciplina
    {
        Task Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto);
        Task Excluir(long[] entidadesIds);
    }
}
