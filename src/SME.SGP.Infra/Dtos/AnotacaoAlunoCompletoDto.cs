using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AnotacaoAlunoCompletoDto: AuditoriaDto
    {
        public AlunoDadosBasicosDto Aluno { get; set; }
        public string Anotacao { get; set; }
    }
}
