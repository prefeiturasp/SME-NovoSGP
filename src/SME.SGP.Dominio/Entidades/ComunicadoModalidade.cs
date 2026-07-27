
using Dapper.Contrib.Extensions;
namespace SME.SGP.Dominio
{
    public class ComunicadoModalidade
    {
        public ComunicadoModalidade()
        {
        }
        public long ComunicadoId { get; set; }
        public long Modalidade { get; set; }
        [Key]
        public long Id { get; set; }
    }
}
