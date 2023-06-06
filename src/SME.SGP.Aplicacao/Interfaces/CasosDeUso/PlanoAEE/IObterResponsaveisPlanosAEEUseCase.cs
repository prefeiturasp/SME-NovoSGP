using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterResponsaveisPlanosAEEUseCase : IUseCase<FiltroPlanosAEEDto, IEnumerable<UsuarioEolRetornoDto>>
    {
    }
}
