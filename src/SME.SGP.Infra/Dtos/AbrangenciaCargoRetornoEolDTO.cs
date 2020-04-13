using SME.SGP.Infra.Enumerados;
using System;

namespace SME.SGP.Dto
{
    public class AbrangenciaCargoRetornoEolDTO
    {
        public Abrangencia Abrangencia { get; set; }
        public string[] CargosId { get; set; }
        public int CdTipoFuncaoAtividade { get; set; }
        public GruposSGP Grupo { get; set; }
        public Guid GrupoID { get; set; }
        public int? TipoFuncaoAtividade { get; set; }
    }
}