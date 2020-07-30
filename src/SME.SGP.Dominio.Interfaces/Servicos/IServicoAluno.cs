using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAluno
    {
        MarcadorFrequenciaDto ObterMarcadorAluno(AlunoPorTurmaResposta aluno, PeriodoEscolar bimestre, bool ehInfantil = false);
    }
}