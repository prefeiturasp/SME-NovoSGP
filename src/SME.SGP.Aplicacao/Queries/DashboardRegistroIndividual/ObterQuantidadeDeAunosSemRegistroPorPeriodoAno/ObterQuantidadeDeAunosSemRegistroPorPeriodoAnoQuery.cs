using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery : IRequest<IEnumerable<GraficoBaseDto>>
    {
        public ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery(int anoLetivo, Modalidade modalidade, DateTime dataInicial, long dreId)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DataInicicial = dataInicial;
            DreId = dreId;
        }

        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataInicicial { get; set; }
        public long DreId { get; set; }
    }
    public class ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQueryValidator : AbstractValidator<ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery>
    {
        public ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQueryValidator()
        {

            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
            RuleFor(c => c.Modalidade)
                .NotNull()
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");
            RuleFor(c => c.DataInicicial)
                .NotNull()
                .WithMessage("A data incial deve ser informada.");
        }
    }
}
