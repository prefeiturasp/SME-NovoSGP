using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroDisciplinaPlanejamentoDto
    {
        public int CodigoDisciplina { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "É necessario informar o codigo da turma")]
        public long CodigoTurma { get; set; }

        public bool Regencia { get; set; }
    }
}