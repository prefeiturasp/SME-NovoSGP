using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaSituacaoAtendimentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery : IRequest<bool>
    {
        public long EncaminhamentoId { get; set; }

        public VerificaSituacaoAtendimentoNAAPASeEstaAguardandoAtendimentoIndevidamenteQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId; 
        }
    }

}
