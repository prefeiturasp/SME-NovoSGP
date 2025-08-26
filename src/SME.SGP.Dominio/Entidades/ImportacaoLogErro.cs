using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ImportacaoLogErro : EntidadeBase
    {
        public long ImportacaoLogId {  get; set; }
        public long LinhaArquivo {  get; set; }
        public string MotivoFalha { get; set; }
    }
}
