using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AlunoReduzidoDto
    {
        public string CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public string NomeSocialAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public string DataNascimento { get; set; }
        public DateTime DataSituacao { get; set; }
        public DateTime DataMatricula { get; set; }
        public string SituacaoMatricula { get; set; }
        public string TurmaEscola { get; set; }
    }
}
