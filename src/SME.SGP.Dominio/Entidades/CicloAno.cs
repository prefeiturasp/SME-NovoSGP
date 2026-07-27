
using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class CicloAno
    {
        [Key]
        public long Id { get; set; }
        public Ciclo Ciclo { get; set; }
        public long CicloId { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
    }
}
