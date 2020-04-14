using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoAcompanhamentoEscolar
    {
        Task AlterarComunicado(ComunicadoInserirDto comunicado);

        Task CriarComunicado(ComunicadoInserirDto comunicado);

        Task ExcluirComunicado(long id);
    }
}