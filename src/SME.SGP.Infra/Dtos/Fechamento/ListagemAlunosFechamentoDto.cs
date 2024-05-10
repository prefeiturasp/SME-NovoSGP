using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ListagemAlunosFechamentoDto
    {
        public ListagemAlunosFechamentoDto(IEnumerable<FechamentoTurmaDisciplina> fechamentosTurma, 
                                           Turma turma, 
                                           string componenteCurricularCodigo,
                                           DisciplinaDto disciplina, 
                                           IEnumerable<PeriodoEscolar> periodosEscolares,
                                           Usuario usuarioAtual, 
                                           IEnumerable<string> alunosComAnotacao)
        {
            FechamentosTurma = fechamentosTurma;
            Turma = turma;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            Disciplina = disciplina;
            PeriodosEscolares = periodosEscolares;
            UsuarioAtual = usuarioAtual;
            AlunosComAnotacao = alunosComAnotacao;
        }

        public IEnumerable< FechamentoTurmaDisciplina> FechamentosTurma { get; set; }
        public Turma Turma { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public DisciplinaDto Disciplina { get; set; }
        public IEnumerable<PeriodoEscolar> PeriodosEscolares { get; set; }
        public Usuario UsuarioAtual { get; set; }
        public IEnumerable<string> AlunosComAnotacao { get; set; }
    }
}
