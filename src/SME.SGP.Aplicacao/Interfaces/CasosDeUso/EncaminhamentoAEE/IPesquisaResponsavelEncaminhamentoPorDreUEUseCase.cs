using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPesquisaResponsavelEncaminhamentoPorDreUEUseCase : IUseCase<FiltroPesquisaFuncionarioDto, PaginacaoResultadoDto<UsuarioEolRetornoDto>>
    {
    }
}
