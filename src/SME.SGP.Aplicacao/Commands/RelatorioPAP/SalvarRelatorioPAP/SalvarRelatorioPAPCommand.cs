using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPAPCommand : IRequest<ResultadoRelatorioPAPDto>
    {
        public SalvarRelatorioPAPCommand(RelatorioPAPDto relatorioPapDto)
        {
            RelatorioPAPDto = relatorioPapDto;
        }

        public RelatorioPAPDto RelatorioPAPDto { get; set; }
    }

    public class SalvarRelatorioPAPCommandValidator : AbstractValidator<SalvarRelatorioPAPCommand>
    {
        public SalvarRelatorioPAPCommandValidator()
        {
            RuleFor(x => x.RelatorioPAPDto).NotEmpty().WithMessage("Informe o relatório  pap para poder salvar");
        }
    }
}