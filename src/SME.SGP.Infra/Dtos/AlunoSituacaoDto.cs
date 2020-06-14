using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunoSituacaoDto
    {
        public string Codigo { get; set; }

        public int NumeroChamada { get; set; }

        public string Nome { get; set; }

        public SituacaoMatriculaAluno Situacao { get; set; }
    }
}
