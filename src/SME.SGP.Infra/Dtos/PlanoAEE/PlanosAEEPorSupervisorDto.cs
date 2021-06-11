using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanosAEEPorSupervisorDto
    {
        public PlanosAEEPorSupervisorDto(string supervisor, string ueCodigo, List<PlanoAEEReduzidoDto> planosUe)
        {
            Supervisor = supervisor;
            Planos = new List<PlanosAEEPorUEDto>() { new PlanosAEEPorUEDto(ueCodigo, planosUe) };
        }

        public PlanosAEEPorSupervisorDto()
        {
            Planos = new List<PlanosAEEPorUEDto>();
        }

        public string Supervisor { get; set; }
        public List<PlanosAEEPorUEDto> Planos { get; set; }
    }
}
