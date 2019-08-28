using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessorTurma : IConsultasProfessorTurma
    {
        public ConsultasProfessorTurma()
        {
        }

        public IEnumerable<ProfessorTurmaDto> Listar(string codigoRf)
        {
            string escola1 = "Professor Caíque Ferreira";
            string escola2 = "Professor Massato Kano";
            string escola3 = "DR. Ruy Pereira Mendonça";
            var lista = new List<ProfessorTurmaDto>();
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "Fundamental",
                Dre = "DRE Campo Limpo",
                Ue = escola1,
                Turma = "1A"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "EJA",
                Semestre = 1,
                Dre = "DRE Campo Limpo",
                Ue = escola1,
                Turma = "2B"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "EJA",
                Semestre = 2,
                Dre = "DRE Campo Limpo",
                Ue = escola1,
                Turma = "2C"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "Fundamental",
                Dre = "DRE Campo Limpo",
                Ue = escola2,
                Turma = "2B"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "Fundamental",
                Dre = "DRE Campo Limpo",
                Ue = escola2,
                Turma = "1A"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "EJA",
                Semestre = 1,
                Dre = "DRE Campo Limpo",
                Ue = escola2,
                Turma = "2B"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "EJA",
                Semestre = 2,
                Dre = "DRE Campo Limpo",
                Ue = escola2,
                Turma = "2B"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "Fundamental",
                Dre = "DRE Campo Limpo",
                Ue = escola2,
                Turma = "2A"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "Fundamental",
                Dre = "DRE São Judas",
                Ue = escola3,
                Turma = "2A"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "Fundamental",
                Dre = "DRE São Judas",
                Ue = escola3,
                Turma = "3A"
            });
            lista.Add(new ProfessorTurmaDto
            {
                Ano = 2019,
                Modalidade = "EJA",
                Semestre = 1,
                Dre = "DRE São Judas",
                Ue = escola3,
                Turma = "1A"
            });
            return lista;
        }
    }
}