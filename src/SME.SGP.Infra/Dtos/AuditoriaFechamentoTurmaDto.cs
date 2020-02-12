using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AuditoriaFechamentoTurmaDto: AuditoriaDto
    {
        public AuditoriaFechamentoTurmaDto(): base()
        {
            Sucesso = true;
        }

        public bool Sucesso { get; set; }
        public string MensagemConsistencia { get; set; }

        public static explicit operator AuditoriaFechamentoTurmaDto(EntidadeBase entidade)
            => new AuditoriaFechamentoTurmaDto()
            {
                Id = entidade.Id,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                CriadoRF = entidade.CriadoRF,
                AlteradoEm = entidade.AlteradoEm,
                AlteradoPor = entidade.AlteradoPor,
                AlteradoRF = entidade.AlteradoRF
            };
    }
}
