using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Usuarios
{
    public interface IObterUsuariosCoreSsoPorRfUseCase
    {
        Task<UsuarioCoreSsoDto> Executar(string codigoRf);
    }
}
