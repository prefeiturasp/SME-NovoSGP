using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class AbrangenciaCargoRetornoEolDTO
    {
        public Abrangencia Abrangencia { get; set; }
        public List<int> CargosId { get; set; }
        public int CdTipoFuncaoAtividade { get; set; }
        public GruposSGP Grupo { get; set; }
        public Guid GrupoID { get; set; }
        public int? TipoFuncaoAtividade { get; set; }
    }
}