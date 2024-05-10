using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPesquisaFuncionariosPorDreUeUseCase : IUseCase<FiltroPesquisaFuncionarioDto, PaginacaoResultadoDto<UsuarioEolRetornoDto>>
    {
    }
}
