using System;

namespace SME.SGP.Dominio
{
    public class PrioridadePerfil : EntidadeBase
    {
        public Guid CodigoPerfil { get; set; }
        public string NomePerfil { get; set; }
        public int Ordem { get; set; }
        public TipoPerfil Tipo { get; set; }
    }
}