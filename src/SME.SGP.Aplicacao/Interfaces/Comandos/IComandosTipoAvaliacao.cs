using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoAvaliacao
    {
        Task Alterar(TipoAvaliacaoDto dto, long id);

        Task Excluir(long[] ids);

        Task Inserir(TipoAvaliacaoDto dto);
    }
}