using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEmPeriodoDeFechamentoQuery : IRequest<bool>
    {
        public ObterTurmaEmPeriodoDeFechamentoQuery(Turma turma, DateTime dataReferencia, int bimestre = 0, int bimestreAlteracao = 0)
        {
            Turma = turma;
            Bimestre = bimestre;
            BimestreAlteracao = bimestreAlteracao;
            Bimestre = bimestre;
            DataReferencia = dataReferencia;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
        public int Bimestre { get; set; }

        public int BimestreAlteracao { get; set; }
    }

    public class ObterTurmaEmPeriodoDeFechamentoQueryValidator : AbstractValidator<ObterTurmaEmPeriodoDeFechamentoQuery>
    {
        public ObterTurmaEmPeriodoDeFechamentoQueryValidator()
        {
            RuleFor(a => a.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O bimestre é necessário para a pesquisa de período de fechamento");

            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referencia deve ser informada para a pesquisa de período de fechamento");
        }
    }
}
