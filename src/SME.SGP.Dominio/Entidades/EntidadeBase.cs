using System;

namespace SME.SGP.Dominio
{
    public abstract class EntidadeBase
    {
        protected EntidadeBase()
        {
            CriadoEm = DateTime.Now;
        }

        public DateTime AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public long Id { get; set; }
    }
}