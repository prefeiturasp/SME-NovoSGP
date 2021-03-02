using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AlunoReduzidoDto
    {
        public string CodigoAluno { get; set; }
        public string Nome { get; set; }        
        public int NumeroAlunoChamada { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataSituacao { get; set; }        
        public string Situacao { get; set; }
        public string TurmaEscola { get; set; }
        public string NomeResponsavel { get; set; }
        public string TipoResponsavel { get; set; }
        public string CelularResponsavel { get; set; }
        public DateTime DataAtualizacaoContato { get; set; }
        public bool EhAtendidoAEE { get; set; }
    }
}
