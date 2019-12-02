using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJPersistenciaItemDto
    {
        [Required(ErrorMessage = "É necessário informar a disciplina.")]
        public string DisciplinaId { get; set; }

        public bool Substituir { get; set; }
    }
}