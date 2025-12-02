using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class ResultadoNovoEncaminhamentoNAAPADto
    {
        public ResultadoNovoEncaminhamentoNAAPADto() { }

        public ResultadoNovoEncaminhamentoNAAPADto(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}