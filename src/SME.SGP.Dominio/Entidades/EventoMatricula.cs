using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class EventoMatricula : EntidadeBase
    {
        public string CodigoAluno { get; set; }
        public SituacaoMatriculaAluno Tipo { get; set; }
        public DateTime DataEvento { get; set; }
        public string NomeEscola { get; set; }
        public string NomeTurma { get; set; }
    }
}
