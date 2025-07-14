using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoFrequencia : EntidadeBase
    {
        public TipoNotificacaoFrequencia Tipo { get; set; }
        public long NotificacaoCodigo { get; set; }
        public string DisciplinaCodigo { get; set; }
        public long AulaId { get; set; }
        public Aula Aula { get; set; }
        public bool Excluido { get; set; }
    }
}
