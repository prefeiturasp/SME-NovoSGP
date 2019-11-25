using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoAtividadeAvaliativa
    {
        Task Alterar(TipoAtividadeAvaliativaDto dto, long id);

        Task Excluir(long idTipoAtividadeAvaliativa);

        Task Inserir(TipoAtividadeAvaliativaDto dto);
    }
}