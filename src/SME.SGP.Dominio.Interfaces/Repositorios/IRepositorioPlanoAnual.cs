using SME.SGP.Dto;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAnual : IRepositorioBase<PlanoAnual>
    {
        PlanoAnualCompletoDto ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre);

        PlanoAnual ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre);

        bool ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(int ano, string escolaId, long turmaId, int bimestre);
    }
}