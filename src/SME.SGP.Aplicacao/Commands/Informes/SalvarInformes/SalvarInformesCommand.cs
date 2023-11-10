using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesCommand : IRequest<Informativo>
    {
        public SalvarInformesCommand(InformesDto informesDto)
        {
            InformesDto = informesDto;
        }

        public InformesDto InformesDto { get; set; }
    }

    public class SalvarInformesCommandValidator : AbstractValidator<SalvarInformesCommand>
    {
        public SalvarInformesCommandValidator()
        {
            RuleFor(a => a.InformesDto.Perfis)
               .NotEmpty()
               .WithMessage("Os perfis devem ser informados para o cadastro do informes.");

            RuleFor(a => a.InformesDto.Titulo)
               .NotEmpty()
               .WithMessage("O titulo deve ser informado para o cadastro do informes.");

            RuleFor(a => a.InformesDto.Texto)
               .NotEmpty()
               .WithMessage("O texto deve ser informado para o cadastro do informes.");
        }
    }
}
