using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery : IRequest<IEnumerable<GraficoBaseDto>>
    {
        public ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery(int anoLetivo, Modalidade modalidade, DateTime dataInicial, long ueId)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DataInicial = dataInicial;
            UeId = ueId;
        }

        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataInicial { get; set; }
        public long UeId { get; set; }
    }
    public class ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQueryValidator : AbstractValidator<ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery>
    {
        public ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQueryValidator()
        {

            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
            RuleFor(c => c.Modalidade)
                .NotNull()
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");
            RuleFor(c => c.DataInicial)
                .NotNull()
                .WithMessage("A data incial deve ser informada.");
        }
    }
}
