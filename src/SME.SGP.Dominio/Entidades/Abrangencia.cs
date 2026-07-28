using Dapper.Contrib.Extensions;
using System;

namespace SME.SGP.Dominio
{
    public class Abrangencia
    {
        public long? DreId { get; set; }
        [Key]
        public long Id { get; set; }
        public Guid Perfil { get; set; }
        public long? TurmaId { get; set; }
        public long? UeId { get; set; }
        public long UsuarioId { get; set; }
        public bool Historico { get; set; }
    }
}