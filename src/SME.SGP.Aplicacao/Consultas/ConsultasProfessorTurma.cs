using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessor : IConsultasProfessor
    {
        public ConsultasProfessor()
        {
        }

        public IEnumerable<ProfessorTurmaDto> Listar(string codigoRf)
        {
            string escola1 = "Professor Caíque Ferreira";
            string escola1Abrev = "Prof. Caíque F.";
            string escola2 = "Professor Massato Kano";
            string escola2Abrev = "Prof.r MK";
            string escola3 = "DR. Ruy Pereira Mendonça";
            string escola3Abrev = "Prof. PM";
            var lista = new List<ProfessorTurmaDto>
            {
                new ProfessorTurmaDto
                {
                    Ano = 1,
                    AnoLetivo = 2019,
                    CodModalidade = 5,
                    Modalidade = "Fundamental",
                    TipoSemestre = 2,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola1,
                    UeAbrev = escola1Abrev,
                    NomeTurma = "1A",
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 3,
                    Modalidade = "EJA",
                    TipoSemestre = 2,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "Dre CL",
                    Ue = escola1,
                    UeAbrev = escola1Abrev,
                    NomeTurma = "2B"
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 3,
                    Modalidade = "EJA",
                    TipoSemestre = 2,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola1,
                    UeAbrev = escola1Abrev,
                    NomeTurma = "2C"
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 5,
                    Modalidade = "Fundamental",
                    TipoSemestre = 1,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola2,
                    UeAbrev = escola2Abrev,
                    NomeTurma = "2B"
                },
                new ProfessorTurmaDto
                {
                    Ano = 1,
                    AnoLetivo = 2019,
                    CodModalidade = 5,
                    Modalidade = "Fundamental",
                    TipoSemestre = 1,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola2,
                    UeAbrev = escola2Abrev,
                    NomeTurma = "1A"
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 3,
                    Modalidade = "EJA",
                    TipoSemestre = 2,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola2,
                    UeAbrev = escola2Abrev,
                    NomeTurma = "2B"
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 3,
                    Modalidade = "EJA",
                    TipoSemestre = 2,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola2,
                    UeAbrev = escola2Abrev,
                    NomeTurma = "2B"
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 5,
                    Modalidade = "Fundamental",
                    TipoSemestre = 1,
                    Dre = "DRE Campo Limpo",
                    DreAbrev = "DRE CL",
                    Ue = escola2,
                    UeAbrev = escola2Abrev,
                    NomeTurma = "2A"
                },
                new ProfessorTurmaDto
                {
                    Ano = 2,
                    AnoLetivo = 2019,
                    CodModalidade = 5,
                    Modalidade = "Fundamental",
                    TipoSemestre = 1,
                    Dre = "DRE São Judas",
                    DreAbrev = "DRE SJ",
                    Ue = escola3,
                    UeAbrev = escola3Abrev,
                    NomeTurma = "2A"
                },
                new ProfessorTurmaDto
                {
                    Ano = 3,
                    AnoLetivo = 2019,
                    CodModalidade = 5,
                    Modalidade = "Fundamental",
                    TipoSemestre = 1,
                    Dre = "DRE São Judas",
                    DreAbrev = "DRE SJ",
                    Ue = escola3,
                    UeAbrev = escola3Abrev,
                    NomeTurma = "3A"
                },
                new ProfessorTurmaDto
                {
                    Ano = 1,
                    AnoLetivo = 2019,
                    CodModalidade = 3,
                    Modalidade = "EJA",
                    TipoSemestre = 2,
                    Dre = "DRE São Judas",
                    DreAbrev = "DRE SJ",
                    Ue = escola3,
                    UeAbrev = escola3Abrev,
                    NomeTurma = "1A"
                }
            };
            return lista;
        }
    }
}