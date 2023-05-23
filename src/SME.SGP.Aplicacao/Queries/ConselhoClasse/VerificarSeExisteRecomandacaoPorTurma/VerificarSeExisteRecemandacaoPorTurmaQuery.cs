using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificarSeExisteRecemandacaoPorTurmaQuery : IRequest<IEnumerable<AlunoTemRecomandacaoDto>>
    {
        public VerificarSeExisteRecemandacaoPorTurmaQuery(long turmaId,int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }

    public class VerificarSeExisteRecomandacaoPorTurmaQueryValidator : AbstractValidator<VerificarSeExisteRecemandacaoPorTurmaQuery>
    {
        public VerificarSeExisteRecomandacaoPorTurmaQueryValidator()
        {
            RuleFor(f => f.TurmaId).GreaterThan(0).WithMessage("Informe o Id da Turma para verificar se existe recomandação");
            RuleFor(f => f.Bimestre).GreaterThan(0).WithMessage("Informe o Bimestre  para verificar se existe recomandação");
        }
    }
}