using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaComPeriodoEscolarQuery : IRequest<FechamentoTurmaPeriodoEscolarDto>
    {
        public ObterFechamentoTurmaComPeriodoEscolarQuery(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; }
        public int Bimestre { get; }
    }

    public class ObterFechamentoTurmaComPeriodoEscolarQueryValidator : AbstractValidator<ObterFechamentoTurmaComPeriodoEscolarQuery>
    {
        public ObterFechamentoTurmaComPeriodoEscolarQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma é necessário para consultar seu fechamento");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre é necessário para consultar o fechamento da turma");
        }
    }
}
