using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos
{
    public class FechamentoTurmaConsolidacaoDto
    {
        public FechamentoTurmaConsolidacaoDto(long[] IdsTurmas, int bimestre = 0)
        {
            IdsTurma = IdsTurmas;
            Bimestre = bimestre;
        }

        [ListaTemElementos(ErrorMessage = "Informe ao menos um código de turma")]
        public long[] IdsTurma { get; set; }
        [Range(0, 4, ErrorMessage = "Informe um bimestre válido")]
        public int Bimestre { get; set; }
    }
}
