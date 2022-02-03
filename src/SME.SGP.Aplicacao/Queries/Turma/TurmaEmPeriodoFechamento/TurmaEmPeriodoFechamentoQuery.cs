using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoFechamentoQuery : IRequest<bool>
    {
        public TurmaEmPeriodoFechamentoQuery(Turma turma, int bimestre, DateTime dataReferencia, long tipoCalendarioId = 0)
        {
            Turma = turma;
            Bimestre = bimestre;
            DataReferencia = dataReferencia;
            TipoCalendarioId = tipoCalendarioId;
        }

        public Turma Turma { get; }
        public int Bimestre { get; }
        public DateTime DataReferencia { get; }
        public long TipoCalendarioId { get; }
    }

    public class TurmaEmPeriodoFechamentoQueryValidator : AbstractValidator<TurmaEmPeriodoFechamentoQuery>
    {
        public TurmaEmPeriodoFechamentoQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta de periodo de fechamento em aberto");

            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referencia");
        }
    }
}
