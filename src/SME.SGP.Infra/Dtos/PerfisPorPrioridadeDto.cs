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

        public Guid PerfilSelecionado { get; set; }
        public IList<PerfilDto> Perfis { get; set; }
        public bool PossuiPerfilSmeOuDre { get; set; }
    }
}