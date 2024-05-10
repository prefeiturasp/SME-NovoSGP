using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IReiniciarSenhaUseCase
    {
        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf, string dreCodigo, string ueCodigo);
    }
}
