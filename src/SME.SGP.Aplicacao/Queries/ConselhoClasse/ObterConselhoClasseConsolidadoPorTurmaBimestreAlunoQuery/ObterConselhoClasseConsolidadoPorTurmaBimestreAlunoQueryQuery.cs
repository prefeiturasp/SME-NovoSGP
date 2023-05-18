using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery : IRequest<ConselhoClasseConsolidadoTurmaAluno>
    {
        public ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery(long turmaId, string alunoCodigo)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQueryValidator : AbstractValidator<ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery>
    {

        public ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma deve ser informado para a busca do conselho de classe consolidado.");
            
            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para a busca do conselho de classe consolidado.");
        }
    }
}