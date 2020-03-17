using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dto
{
    public class AbrangenciaCargoRetornoEolDTO
    {
        public Guid GrupoID { get; set; }
        public List<int> CargosId { get; set; }
        public GruposSGP Grupo { get; set; }
        public int? TipoFuncaoAtividade { get; set; }
        public Abrangencia Abrangencia { get; set; }
        public int CdTipoFuncaoAtividade { get; set; }
    }
}
