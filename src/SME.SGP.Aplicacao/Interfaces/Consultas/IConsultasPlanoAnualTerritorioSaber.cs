using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAnualTerritorioSaber
    {
        Task<long> ObterIdPlanoAnualTerritorioSaberPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId);

        Task<PlanoAnualTerritorioSaberCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto);

        Task<IEnumerable<PlanoAnualTerritorioSaberCompletoDto>> ObterPorUETurmaAnoETerritorioExperiencia(string ueId, string turmaId, int anoLetivo, long territorioExperienciaId);
    }
}
