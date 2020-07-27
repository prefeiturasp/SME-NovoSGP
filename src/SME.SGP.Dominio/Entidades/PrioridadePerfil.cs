using System;

namespace SME.SGP.Dominio
{
    public class PrioridadePerfil : EntidadeBase
    {
        public Guid CodigoPerfil { get; set; }
        public string NomePerfil { get; set; }
        public int Ordem { get; set; }
        public TipoPerfil Tipo { get; set; }

        public bool EhPerfilInfantil()
        {
            return CodigoPerfil == Perfis.PERFIL_PROFESSOR_INFANTIL || CodigoPerfil == Perfis.PERFIL_CJ_INFANTIL;
        }
    }
}