using System.Collections.Generic;

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
