using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPesquisaResponsavelEncaminhamentoPorDreUEUseCase : IUseCase<FiltroPesquisaFuncionarioDto, PaginacaoResultadoDto<UsuarioEolRetornoDto>>
    {
    }
}
