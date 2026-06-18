using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class EventoMatriculaDto
    {
        public string CodigoAluno { get; set; }
        public SituacaoMatriculaAluno Tipo { get; set; }
        public DateTime DataEvento { get; set; }
        public string NomeEscola { get; set; }
        public string NomeTurma { get; set; }
    }
}
