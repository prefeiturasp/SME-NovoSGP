using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAtividadeAvaliativa
    {
        Task Alterar(AtividadeAvaliativaDto dto, long id);

        Task Excluir(long idAtividadeAvaliativa);

        Task Inserir(AtividadeAvaliativaDto dto);

        Task Validar(FiltroAtividadeAvaliativaDto dto);
    }
}