using MediatR;

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
