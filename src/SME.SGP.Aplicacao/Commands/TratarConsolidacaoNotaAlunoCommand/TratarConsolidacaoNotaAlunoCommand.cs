using MediatR;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarConsolidacaoNotaAlunoCommand : IRequest<bool>
    {
        public TratarConsolidacaoNotaAlunoCommand(ConsolidacaoNotaAlunoDto consolidacaoNotaAlunoDto)
        {
            ConsolidacaoNotaAlunoDto = consolidacaoNotaAlunoDto;
        }
        public ConsolidacaoNotaAlunoDto ConsolidacaoNotaAlunoDto { get; set; }
    }
}
