using System;

namespace SME.SGP.Infra
{
    public class TrocaPerfilDto
    {
        public bool EhProfessor { get; set; }
        public bool EhProfessorCj { get; set; }
        public bool EhProfessorPoa { get; set; }
        public DateTime DataHoraExpiracao { get; set; }
        public string Token { get; set; }
        public bool EhProfessorCjInfantil { get; set; }
        public bool EhProfessorInfantil { get; set; }
    }
}