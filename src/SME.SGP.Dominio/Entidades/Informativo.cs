using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Informativo : EntidadeBase
    {
        public Dre Dre { get; set; }
        public long? DreId { get; set; }
        public Ue Ue { get; set; }
        public long? UeId { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime DataEnvio { get; set; }
        public List<InformativoPerfil> Perfis { get; set; }
        public bool Excluido { get; set; }
        public List<InformativoModalidade> Modalidades { get; set; }
    }
}
