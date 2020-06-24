using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAnualTerritorioSaber : IRepositorioBase<PlanoAnualTerritorioSaber>
    {
        Task<PlanoAnualTerritorioSaber> ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId);

        Task<PlanoAnualTerritorioSaberCompletoDto> ObterPlanoAnualTerritorioSaberCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, string turmaId, int bimestre, long territorioExperienciaId);

        Task<IEnumerable<PlanoAnualTerritorioSaberCompletoDto>> ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(int ano, string ueId, string turmaId, long componenteCurricularEolId);
    }
}
