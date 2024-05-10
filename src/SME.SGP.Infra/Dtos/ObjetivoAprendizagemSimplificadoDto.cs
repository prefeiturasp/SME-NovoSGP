using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class ObjetivoAprendizagemSimplificadoDto
    {
        [Range(1, long.MaxValue, ErrorMessage = "O id do objetivo deve ser informado")]
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O componente curricular deve ser informado")]
        public long IdComponenteCurricular { get; set; }

        public long ComponenteCurricularEolId { get; set; }
    }
}