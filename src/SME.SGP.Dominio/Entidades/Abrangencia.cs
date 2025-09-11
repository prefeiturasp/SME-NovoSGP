using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Abrangencia
    {
        public long? DreId { get; set; }
        public long Id { get; set; }
        public Guid Perfil { get; set; }
        public long? TurmaId { get; set; }
        public long? UeId { get; set; }
        public long UsuarioId { get; set; }
        public bool Historico { get; set; }
    }
}