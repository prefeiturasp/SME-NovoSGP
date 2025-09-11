using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComponenteCurricularGrupoAreaOrdenacao
    {
        public long GrupoMatrizId { get; set; }
        public long AreaConhecimentoId { get; set; }
        public int Ordem { get; set; }
    }
}
