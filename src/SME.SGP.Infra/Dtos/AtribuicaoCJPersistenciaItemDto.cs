using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJPersistenciaItemDto
    {
        [Required(ErrorMessage = "É necessário informar a disciplina.")]
        public long DisciplinaId { get; set; }

        public bool Substituir { get; set; }
    }
}