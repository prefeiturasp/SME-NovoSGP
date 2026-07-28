using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class TipoDocumento
    {
        [Key]
        public long Id { get; set; }
        public string Descricao { get; set; }
    }
}