using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SolicitaRelatorioDevolutivasCommand : IRequest<string>
    {
        public SolicitaRelatorioDevolutivasCommand(FiltroRelatorioDevolutivasSincrono filtro)
        {
            Filtro = filtro;
        }
        public FiltroRelatorioDevolutivasSincrono Filtro { get; set; }

        public class SolicitaRelatorioDevolutivasCommandValidator : AbstractValidator<SolicitaRelatorioDevolutivasCommand>
        {
            public SolicitaRelatorioDevolutivasCommandValidator()
            {
                RuleFor(c => c.Filtro)
                   .NotEmpty()
                   .WithMessage("O filtro deve ser informado para solicitação do relatório.");

            }
        }
    }
}
