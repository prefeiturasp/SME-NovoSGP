using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommad : IRequest<long>
    {
        public RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommad(long encaminhamentoNAAPASecaoId)
        {
            EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId;
        }

        public long EncaminhamentoNAAPASecaoId { get; }
    }
}
