using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AuditoriaConselhoClasseAlunoDto: AuditoriaDto
    {
        public long ConselhoClasseId { get; set; }

        public static explicit operator AuditoriaConselhoClasseAlunoDto(ConselhoClasseAluno entidade)
            => new AuditoriaConselhoClasseAlunoDto()
            {
                Id = entidade.Id,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                CriadoRF = entidade.CriadoRF,
                AlteradoEm = entidade.AlteradoEm,
                AlteradoPor = entidade.AlteradoPor,
                AlteradoRF = entidade.AlteradoRF,
                ConselhoClasseId = entidade.ConselhoClasseId
            };
    }
}
