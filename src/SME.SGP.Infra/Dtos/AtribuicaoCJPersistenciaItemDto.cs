using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJPersistenciaItemDto
    {
        [Required(ErrorMessage = "É necessário informar o componente curricular.")]
        public long DisciplinaId { get; set; }

        public bool Substituir { get; set; }
    }
}