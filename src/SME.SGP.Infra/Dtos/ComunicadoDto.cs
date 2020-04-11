using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class ComunicadoDto
    {
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public List<GrupoComunicacaoDto> Grupos { get; set; }
        public long Id { get; set; }
        public string Titulo { get; set; }
    }
}