using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQuery(int bimestre, string codigoTurma)
        {
            Bimestre = bimestre;
            CodigoTurma = codigoTurma;
        }

        public int Bimestre { get; set; }
        public string CodigoTurma { get; set; }
    }

    public class ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreValidator : AbstractValidator<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQuery>
    {
        public ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty().WithMessage("Informe o Código da turma para consultar o fechamento da turma");

            RuleFor(x => x.Bimestre)
                .GreaterThanOrEqualTo(0).WithMessage("Informe um bimestre maior ou igual a zero para consultar o fechamento da turma");
        }
    }
}