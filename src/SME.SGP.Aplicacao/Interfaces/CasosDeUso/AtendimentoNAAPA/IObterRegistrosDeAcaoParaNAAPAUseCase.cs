using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterRegistrosDeAcaoParaNAAPAUseCase : IUseCase<string, PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>>
    {
    }
}
