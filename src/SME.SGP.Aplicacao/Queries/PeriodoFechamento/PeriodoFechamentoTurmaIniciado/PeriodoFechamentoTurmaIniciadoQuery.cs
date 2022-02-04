using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class PeriodoFechamentoTurmaIniciadoQuery : IRequest<bool>
    {
        public PeriodoFechamentoTurmaIniciadoQuery(Turma turma, int bimestre, DateTime dataReferencia, long? tipoCalendarioId = null)
        {
            Turma = turma;
            Bimestre = bimestre;
            DataReferencia = dataReferencia;
            TipoCalendarioId = tipoCalendarioId;
        }

        public Turma Turma { get; }
        public int Bimestre { get; }
        public DateTime DataReferencia { get; }
        public long? TipoCalendarioId { get; }
    }

    public class PeriodoFechamentoTurmaIniciadoQueryValidator : AbstractValidator<PeriodoFechamentoTurmaIniciadoQuery>
    {
        public PeriodoFechamentoTurmaIniciadoQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta de período de fechamento iniciado");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consulta de período de fechamento iniciado");

            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data deve ser informada para consulta de período de fechamento iniciado");
        }
    }
}
