using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery : IRequest<IEnumerable<AvaliacoesPorTurmaComponenteDto>>
    {
        public ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery(long? ueId, DateTime dataInicio, DateTime dataFim)
        {
            UeId = ueId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long? UeId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQueryValidator : AbstractValidator<ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery>
    {
        public ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQueryValidator()
        {
            RuleFor(a => a.DataInicio)
               .NotEmpty()
               .WithMessage("A data de inicio deve ser informada para consulta de avaliações de turmas e componentes no período.");

            RuleFor(a => a.DataFim)
               .NotEmpty()
               .WithMessage("A data de fim deve ser informada para consulta de avaliações de turmas e componentes no período.");
        }
    }
}
