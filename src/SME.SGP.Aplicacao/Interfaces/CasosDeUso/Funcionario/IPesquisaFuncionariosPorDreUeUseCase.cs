using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPesquisaFuncionariosPorDreUeUseCase : IUseCase<FiltroPesquisaFuncionarioDto, PaginacaoResultadoDto<UsuarioEolRetornoDto>>
    {
    }
}
