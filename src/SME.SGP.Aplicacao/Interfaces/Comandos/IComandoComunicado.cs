using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandoComunicado
    {
        Task<string> Alterar(long id, ComunicadoInserirDto comunicadoDto);

        Task Excluir(long[] ids);

        Task<string> Inserir(ComunicadoInserirDto comunicadoDto);
    }
}