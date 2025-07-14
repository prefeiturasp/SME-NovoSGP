using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasseRecomendacao : EntidadeBase
    {
        public bool Excluido { get; set; }
        public string Recomendacao { get; set; }
        public ConselhoClasseRecomendacaoTipo Tipo { get; set; }

        public ConselhoClasseRecomendacao()
        {

        }
    }
}