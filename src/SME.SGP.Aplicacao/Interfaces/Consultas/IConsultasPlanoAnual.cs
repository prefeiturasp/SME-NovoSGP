using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAnual
    {
        Task<PlanoAnualCompletoDto> ObterBimestreExpandido(FiltroPlanoAnualBimestreExpandidoDto filtro);

        Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto);

        Task<PlanoAnualObjetivosDisciplinaDto> ObterObjetivosEscolaTurmaDisciplina(FiltroPlanoAnualDisciplinaDto filtroPlanoDisciplinaDto);

        bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto);
    }
}