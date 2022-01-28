using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAtividadeAvaliativaQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterPendenciasAtividadeAvaliativaQuery(long dreId, int? anoLetivo = null)
        {
            DreId = dreId;
            AnoLetivo = anoLetivo ?? DateTime.Today.Year;
        }

        public long DreId { get; }
        public int AnoLetivo { get; set; }
    }

    public class ObterPendenciasAtividadeAvaliativaQueryValidator : AbstractValidator<ObterPendenciasAtividadeAvaliativaQuery>
    {
        public ObterPendenciasAtividadeAvaliativaQueryValidator()
        {
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O identificador da DRE deve ser informado para consulta de Pendencias de aula do tipo Avaliação");
        }
    }
}
