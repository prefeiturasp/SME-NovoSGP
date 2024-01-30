using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotificarCompensacaoDto
    {
        public NotificarCompensacaoDto(
                            MeusDadosDto professor, 
                            string disciplina, 
                            Turma turma, 
                            CompensacaoAusencia compensacao,
                            List<CompensacaoAusenciaAlunoQtdDto> alunos, 
                            Cargo[] cargos)
        {
            Professor = professor.Nome;
            ProfessorRf = professor.CodigoRf;
            Disciplina = disciplina;
            CodigoTurma = turma.CodigoTurma;
            Turma = turma.Nome;
            Modalidade = turma.ModalidadeCodigo.ObterNomeCurto();
            CodigoUe = turma.Ue.CodigoUe;
            Escola = turma.Ue.Nome;
            TipoEscola = turma.Ue.TipoEscola.ObterNomeCurto();
            CodigoDre = turma.Ue.Dre.CodigoDre;
            Dre = turma.Ue.Dre.Nome;
            Bimestre = compensacao.Bimestre;
            Atividade = compensacao.Nome;
            Alunos = alunos;
            Cargos = cargos;
        }

        public string Professor { get; set; }
        public string ProfessorRf { get; set; }
        public string Disciplina { get; set; }
        public string CodigoTurma { get; set; }
        public string Turma { get; set; }
        public string Modalidade { get; set; }
        public string CodigoUe { get; set; }
        public string Escola { get; set; }
        public string TipoEscola { get; set; }
        public string CodigoDre { get; set; }
        public string Dre { get; set; }
        public int Bimestre { get; set; }
        public string Atividade { get; set; }
        public List<CompensacaoAusenciaAlunoQtdDto> Alunos { get; set; }
        public Cargo[] Cargos { get; set; }
    }
}
