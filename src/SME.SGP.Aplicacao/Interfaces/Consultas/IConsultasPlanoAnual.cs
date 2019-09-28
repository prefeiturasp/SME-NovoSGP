using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAnual
    {
        Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto);

        bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto);
    }
}