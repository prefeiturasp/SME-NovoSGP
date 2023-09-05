using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterUsuarioPorCpfUseCase
    {
        Task<UsuarioEscolaAquiDto> Executar(string codigoDre, string codigoUe, string cpf);
    }
}