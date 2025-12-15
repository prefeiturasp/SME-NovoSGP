using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente
{
    public class VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery : IRequest<bool>
    {
        public long EncaminhamentoId { get; set; }

        public VerificaSituacaoNovoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }
    }
}