using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class InatividadeAtendimentoNAAPANotificacao : EntidadeBase
    {
        public long EncaminhamentoNAAPAId { get; set; }
        public long NotificacaoId { get; set; }
        public bool Excluido { get; set; }
    }
}
