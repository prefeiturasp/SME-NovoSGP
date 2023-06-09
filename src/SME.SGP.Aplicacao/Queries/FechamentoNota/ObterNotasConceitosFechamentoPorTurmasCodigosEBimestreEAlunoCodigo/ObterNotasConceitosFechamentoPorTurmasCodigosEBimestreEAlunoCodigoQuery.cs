using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery : IRequest<IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        public ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery(string[] turmasCodigos, int bimestre, string alunoCodigo)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
        }

        public string[] TurmasCodigos { get; }
        public string AlunoCodigo { get; }
        public int Bimestre { get; }
    }

    public class ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQueryValidator : AbstractValidator<ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery>
    {
        public ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQueryValidator()
        {
            RuleFor(t => t.TurmasCodigos)
                .NotEmpty()
                .WithMessage("Os Códigos das turmas devem ser informados para obter as notas do fechamento");

            RuleFor(t => t.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do aluno devem ser informados para obter as notas do fechamento");

            RuleFor(t => t.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O bimestre devem ser informados para obter as notas do fechamento");
        }
    }
}
