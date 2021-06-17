using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanosAEEPorUEDto
    {
        public PlanosAEEPorUEDto(string ueCodigo, List<PlanoAEEReduzidoDto> planos)
        {
            UeCodigo = ueCodigo;
            Planos = planos;
        }

        public PlanosAEEPorUEDto()
        {
            Planos = new List<PlanoAEEReduzidoDto>();
        }

        public string UeCodigo { get; set; }
        public List<PlanoAEEReduzidoDto> Planos { get; set; }
    }
}
