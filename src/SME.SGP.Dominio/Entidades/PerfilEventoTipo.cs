using SME.SGP.Dominio.Entidades;
using System;

namespace SME.SGP.Dominio
{
    public class PerfilEventoTipo : EntidadeBase
    {
        public EventoTipo EventoTipo { get; set; }
        public long EventoTipoId { get; set; }

        public Guid CodigoPerfil { get; set; }

        public bool Excluido { get; set; }
    }
}
