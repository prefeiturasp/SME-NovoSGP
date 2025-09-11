using SME.SGP.Dominio.Entidades;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PerfilEventoTipo : EntidadeBase
    {
        public EventoTipo EventoTipo { get; set; }
        public long EventoTipoId { get; set; }

        public Guid CodigoPerfil { get; set; }

        public bool Excluido { get; set; }
    }
}
