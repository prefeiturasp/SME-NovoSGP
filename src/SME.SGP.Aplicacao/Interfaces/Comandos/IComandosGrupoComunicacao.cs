using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosGrupoComunicacao
    {
        Task Alterar(GrupoComunicacaoDto dto, long id);

        Task Excluir(long id);

        Task Inserir(GrupoComunicacaoDto dto);
    }
}