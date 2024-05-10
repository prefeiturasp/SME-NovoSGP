using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class PersistirRelatorioPAPCommand : IRequest<ResultadoRelatorioPAPDto>
    {
        public PersistirRelatorioPAPCommand(RelatorioPAPDto relatorioPapDto)
        {
            RelatorioPAPDto = relatorioPapDto;
        }

        public RelatorioPAPDto RelatorioPAPDto { get; set; }
    }


    public class PersistirRelatorioPAPCommandValidator : AbstractValidator<PersistirRelatorioPAPCommand>
    {
        public PersistirRelatorioPAPCommandValidator()
        {
            RuleFor(x => x.RelatorioPAPDto).NotEmpty().WithMessage("Informe o relatório para  alteração ");
        }
    }
}