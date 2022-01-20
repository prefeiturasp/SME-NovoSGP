using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaNotaPorAtividadeAvaliativaQuery : IRequest<bool>
    {
        public ObterPendenciaNotaPorAtividadeAvaliativaQuery(string turmaCodigo, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int tipoAtividadeAvaliativa)
        {
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            InicioPeriodo = inicioPeriodo;
            FimPeriodo = fimPeriodo;
            TipoAtividadeAvaliativa = tipoAtividadeAvaliativa;
        }

        public string TurmaCodigo { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime InicioPeriodo { get; set; }
        public DateTime FimPeriodo { get; set; }
        public int TipoAtividadeAvaliativa { get; set; }
    }

    public class ObterPendenciaNotaPorAtividadeAvaliativaQueryValidator : AbstractValidator<ObterPendenciaNotaPorAtividadeAvaliativaQuery>
    {
        public ObterPendenciaNotaPorAtividadeAvaliativaQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
             .NotEmpty()
             .WithMessage("O codigo da turma deve ser informado.");

            RuleFor(x => x.DisciplinaId)
             .NotEmpty()
             .WithMessage("O id da disciplina deve ser informado.");

            RuleFor(x => x.InicioPeriodo)
             .NotEmpty()
             .WithMessage("A data inicio deve ser informada.");

            RuleFor(x => x.FimPeriodo)
             .NotEmpty()
             .WithMessage("A data fim deve ser informada.");

            RuleFor(x => x.TipoAtividadeAvaliativa)
             .NotEmpty()
             .WithMessage("O tipo de atividade avaliativa deve ser informada.");
        }
    }
}
