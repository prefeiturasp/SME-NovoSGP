using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterFuncionariosPorUeUseCase
    {
        Task<IEnumerable<UsuarioEolRetornoDto>> Executar(string codigoUe, string filtro);
    }
}