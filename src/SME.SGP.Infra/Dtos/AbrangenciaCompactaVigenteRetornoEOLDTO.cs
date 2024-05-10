using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dto
{
    public class AbrangenciaCompactaVigenteRetornoEOLDTO
    {
        public string Login { get; set; }
        public AbrangenciaCargoRetornoEolDTO Abrangencia { get; set; }
        public string[] IdDres { get; set; }
        public string[] IdUes { get; set; }
        public string[] IdTurmas { get; set; }
    }
}
