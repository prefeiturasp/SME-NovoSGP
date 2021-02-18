using System;

namespace SME.SGP.Infra
{
    public class FiltroBuscaEstudantesAtivoDto
    {
        public string AlunoNome { get; set; }
        public long? AlunoCodigo { get; set; }
        public string UeCodigo { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}