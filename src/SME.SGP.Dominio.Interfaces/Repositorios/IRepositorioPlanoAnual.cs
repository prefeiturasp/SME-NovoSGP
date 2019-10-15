using SME.SGP.Dto;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAnual : IRepositorioBase<PlanoAnual>
    {
        PlanoAnualCompletoDto ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long componenteCurricularEolId);

        PlanoAnual ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId);

        bool ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(int ano, string escolaId, long turmaId, int bimestre, long componenteCurricularEolId);
    }
}