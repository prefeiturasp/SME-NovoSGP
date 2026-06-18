using MediatR;

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
