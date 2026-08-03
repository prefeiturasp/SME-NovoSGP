using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SME.SGP.Dominio
{
    [Table("componente_curricular_grupo_matriz")]
    public class ComponenteCurricularGrupoMatriz
    {
   
        [Column("id")]
        public long Id { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
    }
}
