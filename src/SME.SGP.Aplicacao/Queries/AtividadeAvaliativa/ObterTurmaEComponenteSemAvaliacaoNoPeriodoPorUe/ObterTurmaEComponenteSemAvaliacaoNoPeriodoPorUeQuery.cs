using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery : IRequest<IEnumerable<TurmaEComponenteDto>>
    {
        public ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery(long tipoCalendarioId, DateTime dataInicio, DateTime datafim)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataInicio = dataInicio;
            DataFim = datafim;
        }

        public long TipoCalendarioId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQueryValidator : AbstractValidator<ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery>
    {
        public ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQueryValidator()
        {           
            RuleFor(c => c.TipoCalendarioId)
               .NotEmpty()
               .WithMessage("O id do tipo de calendario deve ser informado para busca de turmas sem avaliação.");

            RuleFor(c => c.DataInicio)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data de início do período deve ser informada para busca de turmas sem avaliação.");

            RuleFor(c => c.DataFim)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data de fim do período deve ser informada para busca de turmas sem avaliação.");
        }
    }
}
