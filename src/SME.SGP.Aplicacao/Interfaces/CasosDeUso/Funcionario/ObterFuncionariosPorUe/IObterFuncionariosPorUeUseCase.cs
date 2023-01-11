using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterFuncionariosPorUeUseCase
    {
        Task<IEnumerable<UsuarioEolRetornoDto>> Executar(string codigoUe, string filtro);
    }
}