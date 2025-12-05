using MediatR;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommad : IRequest<long>
    {
        public RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommad(long encaminhamentoNAAPASecaoId)
        {
            EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId;
        }

        public long EncaminhamentoNAAPASecaoId { get; }
    }
}
