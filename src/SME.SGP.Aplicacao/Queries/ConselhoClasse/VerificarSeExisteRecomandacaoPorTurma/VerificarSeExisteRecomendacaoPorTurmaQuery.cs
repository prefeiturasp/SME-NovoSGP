using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificarSeExisteRecomendacaoPorTurmaQuery : IRequest<IEnumerable<AlunoTemRecomandacaoDto>>
    {
        public VerificarSeExisteRecomendacaoPorTurmaQuery(string[] turmaCodigo,int bimestre)
        {
            TurmasCodigo = turmaCodigo;
            Bimestre = bimestre;
        }

        public string[] TurmasCodigo { get; set; }
        public int Bimestre { get; set; }
    }

    public class VerificarSeExisteRecomandacaoPorTurmaQueryValidator : AbstractValidator<VerificarSeExisteRecomendacaoPorTurmaQuery>
    {
        public VerificarSeExisteRecomandacaoPorTurmaQueryValidator()
        {
            RuleFor(f => f.TurmasCodigo)
                .NotNull()
                .WithMessage("Informe o Id da Turma para verificar se existe recomendação");
            RuleFor(f => f.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Informe o Bimestre  para verificar se existe recomendação");
        }
    }
}