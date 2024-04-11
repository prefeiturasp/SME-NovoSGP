using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommand : IRequest<bool>
    {
        public CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommand(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }        
    }

    public class CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommandValidator : AbstractValidator<CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommand>
    {
        public CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommandValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("o Id do Tipo de Calendário deve ser informado.");
        }
    }
}
