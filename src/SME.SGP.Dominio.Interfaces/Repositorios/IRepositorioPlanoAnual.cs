using SME.SGP.Dto;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAnual : IRepositorioBase<PlanoAnual>
    {
        PlanoAnualCompletoDto ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(int ano, long escolaId, long turmaId, int bimestre);

        PlanoAnual ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(int ano, long escolaId, long turmaId, int bimestre);

        bool ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(int ano, long escolaId, long turmaId, int bimestre);
    }
}