using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery : IRequest<bool>
    {
        public long EncaminhamentoId { get; set; }

        public VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId; 
        }
    }

}
