using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAnual
    {
        Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto);

        bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto);
    }
}