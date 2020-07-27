using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PerfisPorPrioridadeDto
    {
        public PerfisPorPrioridadeDto()
        {
            Perfis = new List<PerfilDto>();
        }

        public bool EhProfessor { get; set; }
        public bool EhProfessorCj { get; set; }
        public bool EhProfessorCjInfantil { get; set; }
        public bool EhProfessorPoa { get; set; }
        public Guid PerfilSelecionado { get; set; }
        public IList<PerfilDto> Perfis { get; set; }
        public bool PossuiPerfilDre { get; set; }
        public bool PossuiPerfilSme { get; set; }
        public bool PossuiPerfilSmeOuDre { get; set; }
        public bool EhProfessorInfantil { get; set; }
    }
}