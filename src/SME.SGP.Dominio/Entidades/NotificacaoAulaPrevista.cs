using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoAulaPrevista :EntidadeBase
    {
        public long NotificacaoCodigo { get; set; }
        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public int Bimestre { get; set; }
        public bool Excluido { get; set; }
    }
}
