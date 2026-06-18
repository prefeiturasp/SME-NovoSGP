using System;

namespace SME.SGP.Infra
{
   public class DataPlanoAulaTurmaDto
    {
        public DateTime Data { get; set; }
        public string TurmaId { get; set; }
        public bool Sobreescrever { get; set; }
    }
}
