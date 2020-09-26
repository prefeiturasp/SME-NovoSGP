using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class ExcluirNotificacaoDiarioBordoDto
    {
        public ExcluirNotificacaoDiarioBordoDto(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }

        public long ObservacaoId { get; set; }
    }
}
