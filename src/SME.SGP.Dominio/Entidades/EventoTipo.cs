using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class EventoTipo : EntidadeBase
    {
        public string Descricao { get; set; }
        public EventoLocalOcorrencia LocalOcorrencia { get; set; }
        public bool Dependencia { get; set; }
        public bool Concomitancia { get; set; }
        public EventoTipoData TipoData { get; set; }
        public EventoLetivo Letivo { get; set; }
        public bool Ativo { get; set; }
        public bool Migrado { get; set; }
    }
}
