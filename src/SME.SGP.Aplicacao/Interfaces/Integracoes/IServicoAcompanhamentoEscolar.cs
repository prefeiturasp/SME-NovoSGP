using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoAcompanhamentoEscolar
    {
        Task AlterarComunicado(ComunicadoInserirAeDto comunicado, long id);

        Task CriarComunicado(ComunicadoInserirAeDto comunicado);

        Task ExcluirComunicado(long[] ids);
    }
}