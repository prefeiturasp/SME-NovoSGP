using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class EventoMatricula : EntidadeBase
    {
        public string CodigoAluno { get; set; }
        public SituacaoMatriculaAluno Tipo { get; set; }
        public DateTime DataEvento { get; set; }
        public string NomeEscola { get; set; }
        public string NomeTurma { get; set; }
    }
}
