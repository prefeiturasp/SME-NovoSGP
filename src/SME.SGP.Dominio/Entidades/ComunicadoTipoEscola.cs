using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class ComunicadoTipoEscola
    {
        public ComunicadoTipoEscola()
        {
        }
        public long ComunicadoId { get; set; }
        public long TipoEscola { get; set; }
        [Key]
        public long Id { get; set; }
    }
}
