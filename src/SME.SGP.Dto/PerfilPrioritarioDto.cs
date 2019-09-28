using System;

namespace SME.SGP.Dto
{
    public class PerfilPrioritarioDto
    {
        public string Descricao { get; set; }
        public bool EhPrioritario { get; set; }
        public Guid Guid { get; set; }
    }
}