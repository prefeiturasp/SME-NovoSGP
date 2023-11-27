using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterRegistrosAcaoCriancaEstudanteAusenteUseCase : IUseCase<FiltroRegistrosAcaoCriancasEstudantesAusentesDto, PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>>
    {
        
    }
}
