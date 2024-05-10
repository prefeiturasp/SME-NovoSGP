using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries
{
    public class VerificaNotasTodosComponentesCurricularesQuery : IRequest<bool>
    {
        public VerificaNotasTodosComponentesCurricularesQuery(string alunoCodigo, Turma turma, int? bimestre = null, bool? historico = false, Dominio.PeriodoEscolar periodoEscolar = null)
        {
            AlunoCodigo = alunoCodigo;
            Turma = turma;
            Bimestre = bimestre;
            Historico = historico;
            PeriodoEscolar = periodoEscolar;
        }
        public Turma Turma { get; set; }
        public string AlunoCodigo { get; set; }
        public int? Bimestre { get; set; }
        public bool? Historico { get; set; }
        public Dominio.PeriodoEscolar PeriodoEscolar { get; set; }
    }

    public class VerificaNotasTodosComponentesCurricularesQueryValidator : AbstractValidator<VerificaNotasTodosComponentesCurricularesQuery>
    {
        public VerificaNotasTodosComponentesCurricularesQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotNull().WithMessage("Informe a turma para verificar notas de todos os componentes curriculares");
            
            RuleFor(x => x.AlunoCodigo)
                .NotEmpty().WithMessage("Informe o código do aluno para verificar notas de todos os componentes curriculares");
        }
    }
}