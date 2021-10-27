using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterValidacaoPodePersistirTurmaNasDatasQuery : IRequest<List<PodePersistirNaDataRetornoEolDto>>
    {
        public ObterValidacaoPodePersistirTurmaNasDatasQuery(string codigoRf, string turmaCodigo, DateTime[] dateTimes, long componenteCurricularCodigo)
        {
            CodigoRf = codigoRf;
            TurmaCodigo = turmaCodigo;
            DateTimes = dateTimes;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string CodigoRf { get; }
        public string TurmaCodigo { get; }
        public DateTime[] DateTimes { get; }
        public long ComponenteCurricularCodigo { get; }
    }
    public class ObterValidacaoPodePersistirTurmaNasDatasQueryValidator : AbstractValidator<ObterValidacaoPodePersistirTurmaNasDatasQuery>
    {
        public ObterValidacaoPodePersistirTurmaNasDatasQueryValidator()
        {
            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("O código Rf deve ser informado");

            RuleFor(a => a.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado");

            RuleFor(a => a.DateTimes)
                .NotEmpty()
                .WithMessage("Pelo menos uma data deve ser informada");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado");
        }
    }
}