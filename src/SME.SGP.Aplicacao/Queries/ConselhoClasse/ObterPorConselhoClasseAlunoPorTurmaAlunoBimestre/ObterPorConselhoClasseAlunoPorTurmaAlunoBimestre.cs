using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery : IRequest<ConselhoClasseAluno>
    {
        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public int Bimestre { get; set; }
        public bool EhFinal { get; set; }

        public ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery(string codigoTurma, string codigoAluno, int bimestre, bool ehFinal)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
            EhFinal = ehFinal;
        }
        
        public class ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQueryValidator : AbstractValidator<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>
        {
            public ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQueryValidator()
            {
                RuleFor(c => c.CodigoTurma)
                    .NotEmpty()
                    .WithMessage("O código da turma deve ser informado para a busca de conselho de classe do aluno.");
                
                RuleFor(c => c.CodigoAluno)
                    .NotEmpty()
                    .WithMessage("O código do aluno deve ser informado para a busca de conselho de classe do aluno.");
                
                RuleFor(c => c.Bimestre)
                    .ExclusiveBetween(1,4)
                    .When(a=> !a.EhFinal)
                    .WithMessage("O bimestre deve ser informado para a busca de conselho de classe do aluno.");
            }
        }
    }
}
