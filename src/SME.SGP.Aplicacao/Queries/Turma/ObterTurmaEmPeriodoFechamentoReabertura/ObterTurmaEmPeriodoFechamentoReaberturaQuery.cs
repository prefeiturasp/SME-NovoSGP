using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEmPeriodoFechamentoReaberturaQuery : IRequest<FechamentoReabertura>
    {
        public ObterTurmaEmPeriodoFechamentoReaberturaQuery(int bimestre, DateTime dataReferencia, long tipoCalendarioId, string ueCodigo, string dreCodigo)
        {
            Bimestre = bimestre;
            DataReferencia = dataReferencia;
            TipoCalendarioId = tipoCalendarioId;
            UeCodigo = ueCodigo;
            DreCodigo = dreCodigo;
        }

        public int Bimestre { get; set; }
        public DateTime DataReferencia { get; set; }
        public long TipoCalendarioId { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
    }

    public class ObterTurmaEmPeriodoFechamentoQueryValidator : AbstractValidator<ObterTurmaEmPeriodoFechamentoReaberturaQuery>
    {
        public ObterTurmaEmPeriodoFechamentoQueryValidator()
        {
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre é necessário");

            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referencia");

            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo calendario é necessário");
        }
    }
}
