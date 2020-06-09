using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
   public interface IRepositorioPlanoAnualTerritorioSaber : IRepositorioBase<PlanoAnualTerritorioSaber>
    {
        PlanoAnualTerritorioSaber ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId);

        PlanoAnualTerritorioSaberCompletoDto ObterPlanoAnualTerritorioSaberCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, string turmaId, int bimestre, long territorioExperienciaId);

        IEnumerable<PlanoAnualTerritorioSaberCompletoDto> ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(int ano, string ueId, string turmaId, long componenteCurricularEolId);
    }
}
