using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarPlanoAEEUseCase
    {
        Task<RetornoPlanoAEEDto> Executar(PlanoAEEPersistenciaDto planoDto);
    }
}
