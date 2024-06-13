using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.MapeamentoEstudantes
{
    public class ParecerConclusivoAlunoTurmaAnoAnteriorDto
    {
        public ParecerConclusivoAlunoTurmaAnoAnteriorDto()
        {
        }
        public Modalidade Modalidade { get; set; }
        public string NomeTurma { get; set; }
        public string Turma() => string.IsNullOrEmpty(NomeTurma) 
                                          ? string.Empty
                                          : $"{Modalidade.ObterNomeCurto()}-{NomeTurma}";
        public long? IdParecerConclusivo { get; set; }
        public string DescricaoParecerConclusivo { get; set; }
    }
}
