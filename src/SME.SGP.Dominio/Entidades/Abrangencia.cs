using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class Abrangencia
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public long? TurmaId { get; set; }
        public Guid Perfil { get; set; }
    }
}
