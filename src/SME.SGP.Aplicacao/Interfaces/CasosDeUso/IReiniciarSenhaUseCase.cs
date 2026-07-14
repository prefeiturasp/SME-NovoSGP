using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IReiniciarSenhaUseCase
    {
        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf, string dreCodigo, string ueCodigo);
    }
}
