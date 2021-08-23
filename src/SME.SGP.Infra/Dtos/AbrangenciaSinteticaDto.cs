using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dto
{
    public class AbrangenciaSinteticaDto
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public string Login { get; set; }
        public long DreId { get; set; }
        public string CodigoDre { get; set; }
        public long UeId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string CodigoTurma { get; set; }
        public bool Historico { get; set; }
        public Guid Perfil { get; set; }

        public bool EhPerfilProfessor()
          => EhProfessor()
          || EhProfessorCj()
          || EhProfessorInfantil()
          || EhProfessorCjInfantil()
          || EhProfessorPoa()
          || EhProfessorPaee()
          || EhProfessorPap()
          || EhProfessorPoei()
          || EhProfessorPoed()
          || EhProfessorPosl();

        public bool EhProfessorPaee()
            => Perfil == Dominio.Perfis.PERFIL_PAEE;

        public bool EhProfessorPap()
            => Perfil == Dominio.Perfis.PERFIL_PAP;

        public bool EhProfessorPoei()
            => Perfil == Dominio.Perfis.PERFIL_POEI;

        public bool EhProfessorPoed()
            => Perfil == Dominio.Perfis.PERFIL_POED;

        public bool EhProfessorPosl()
            => Perfil == Dominio.Perfis.PERFIL_POSL;

        public bool EhProfessor()
        {
            return Perfil == Dominio.Perfis.PERFIL_PROFESSOR
                || Perfil == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL;
        }

        public bool EhProfessorCj()
        {
            return Perfil == Dominio.Perfis.PERFIL_CJ
                || Perfil == Dominio.Perfis.PERFIL_CJ_INFANTIL;
        }


        public bool EhProfessorPoa()
        {
            return Perfil == Dominio.Perfis.PERFIL_POA;
        }

        public bool EhProfessorInfantil()
        {
            return Perfil == Dominio.Perfis.PERFIL_PROFESSOR_INFANTIL;
        }

        public bool EhProfessorCjInfantil()
        {
            return Perfil == Dominio.Perfis.PERFIL_CJ_INFANTIL;
        }
    }
}
