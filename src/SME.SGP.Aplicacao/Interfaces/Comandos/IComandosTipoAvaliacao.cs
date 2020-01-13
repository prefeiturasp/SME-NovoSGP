using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoAvaliacao
    {
        Task Alterar(TipoAvaliacaoDto dto, long id);

        Task Excluir(long[] idTipoAtividadeAvaliativa);

        Task Inserir(TipoAvaliacaoDto dto);
    }
}