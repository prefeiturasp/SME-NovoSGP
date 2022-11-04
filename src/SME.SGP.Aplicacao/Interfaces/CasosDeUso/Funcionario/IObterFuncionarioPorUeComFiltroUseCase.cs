using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterFuncionarioPorUeComFiltroUseCase : IUseCase<(string codigoUe, string filtro), IEnumerable<UsuarioEolRetornoDto>>
    {
    }
}
