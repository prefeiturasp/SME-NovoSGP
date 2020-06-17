using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAnualTerritorioSaber
    {
        long ObterIdPlanoAnualTerritorioSaberPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId);

        PlanoAnualTerritorioSaberCompletoDto ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto);

        IEnumerable<PlanoAnualTerritorioSaberCompletoDto> ObterPorUETurmaAnoETerritorioExperiencia(string ueId, string turmaId, int anoLetivo, long territorioExperienciaId);
    }
}
