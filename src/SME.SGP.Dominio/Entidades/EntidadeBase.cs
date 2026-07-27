using System;
using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public abstract class EntidadeBase
    {
        protected EntidadeBase()
        {
            CriadoEm = DateTimeExtension.HorarioBrasilia();
        }

        [Key]
        public long Id { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
    }
}