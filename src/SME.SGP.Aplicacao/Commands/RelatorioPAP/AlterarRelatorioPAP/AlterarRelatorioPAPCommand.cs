using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPAPCommand : IRequest<ResultadoRelatorioPAPDto>
    {
        public AlterarRelatorioPAPCommand(RelatorioPAPDto relatorioPapDto)
        {
            RelatorioPAPDto = relatorioPapDto;
        }

        public RelatorioPAPDto RelatorioPAPDto { get; set; }
    }

    public class AlterarRelatorioPAPCommandValidator : AbstractValidator<AlterarRelatorioPAPCommand>
    {
        public AlterarRelatorioPAPCommandValidator()
        {
            RuleFor(x => x.RelatorioPAPDto).NotEmpty().WithMessage("Informe o relatório para  alteração ");
        }
    }
}