using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AlunosMatriculadosEolDto
    {
        public long ComponenteCurricularId { get; set; }
        public int Quantidade { get; set; }
        public int Ordem { get; set; }
        public string Modalidade { get; set; }
        public string Turma { get; set; }
        public string Ano { get; set; }
    }
}
