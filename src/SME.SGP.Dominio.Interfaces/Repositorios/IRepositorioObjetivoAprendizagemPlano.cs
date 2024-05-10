using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoAprendizagemPlano : IRepositorioBase<ObjetivoAprendizagemPlano>
    {
        IEnumerable<ComponenteCurricularSimplificadoDto> ObterDisciplinasDoBimestrePlanoAula(int ano, int bimestre, long turmaId, long componenteCurricularId);

        long ObterIdPorObjetivoAprendizagemJurema(long planoId, long objetivoAprendizagemJuremaId);

        Task<long> ObterIdPorObjetivoAprendizagemJuremaAsync(long planoId, long objetivoAprendizagemJuremaId);

        IEnumerable<ObjetivoAprendizagemPlano> ObterObjetivosAprendizagemPorIdPlano(long idPlano);

        Task<IEnumerable<ObjetivoAprendizagem>> ObterObjetivosPlanoDisciplina(int ano, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool ehRegencia = false);

        Task<IEnumerable<ObjetivosAprendizagemPorComponenteDto>> ObterObjetivosPorComponenteNoPlano(int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool ehRegencia = false);
    }
}