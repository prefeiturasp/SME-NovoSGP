using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPesquisaResponsavelPlanoPorDreUEUseCase : IUseCase<FiltroPesquisaFuncionarioDto, PaginacaoResultadoDto<UsuarioEolRetornoDto>>
    {
    }
}
