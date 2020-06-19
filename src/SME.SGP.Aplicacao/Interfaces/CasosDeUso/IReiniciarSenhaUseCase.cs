using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IReiniciarSenhaUseCase
    {
        Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroFuncionarioDto filtroFuncionariosDto);

        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf);
    }
}
