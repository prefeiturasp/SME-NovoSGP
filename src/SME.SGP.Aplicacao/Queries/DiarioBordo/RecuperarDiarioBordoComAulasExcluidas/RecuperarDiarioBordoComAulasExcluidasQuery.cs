using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class RecuperarDiarioBordoComAulasExcluidasQuery : IRequest<IEnumerable<DiarioBordo>>
    {
        public RecuperarDiarioBordoComAulasExcluidasQuery(string codigoTurma, string codigoDisciplina, long tipoCalendarioId, DateTime[] datasConsideradas)
        {
            CodigoTurma = codigoTurma;
            CodigoDisciplina = codigoDisciplina;
            TipoCalendarioId = tipoCalendarioId;
            DatasConsideradas = datasConsideradas;
        }

        public string CodigoTurma { get; set; }
        public string CodigoDisciplina { get; set; }
        public long TipoCalendarioId { get; set; }
        public DateTime[] DatasConsideradas { get; set; }
    }

    public class RecuperarDiarioBordoComAulasExcluidasCommandValidator : AbstractValidator<RecuperarDiarioBordoComAulasExcluidasQuery>
    {
        public RecuperarDiarioBordoComAulasExcluidasCommandValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.CodigoDisciplina)
                .NotEmpty()
                .WithMessage("O código da disciplina deve ser informado.");

            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O id do tipo de calendário deve ser informado.");

            RuleFor(x => x.DatasConsideradas)
                .NotEmpty()
                .WithMessage("Informe ao menos uma data considerada.");
        }
    }
}
