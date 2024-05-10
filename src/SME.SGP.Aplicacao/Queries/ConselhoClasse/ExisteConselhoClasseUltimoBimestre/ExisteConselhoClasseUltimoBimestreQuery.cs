using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExisteConselhoClasseUltimoBimestreQuery : IRequest<bool>
    {
        public ExisteConselhoClasseUltimoBimestreQuery(Turma turma, string alunoCodigo)
        {
            Turma = turma;
            AlunoCodigo = alunoCodigo;
        }

        public Turma Turma { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class ExisteConselhoClasseUltimoBimestreQueryValidator : AbstractValidator<ExisteConselhoClasseUltimoBimestreQuery>
    {

        public ExisteConselhoClasseUltimoBimestreQueryValidator()
        {
            RuleFor(c => c.Turma)
                .NotNull()
                .WithMessage("A turma deve ser informado para a busca do conselho de classe no último bimestre.");
            
            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para a busca do conselho de classe no último bimestre.");
        }
    }
}