using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesModalidadeCommand : IRequest<long>
    {
        public SalvarInformesModalidadeCommand(long informesId, Modalidade modalidade)
        {
            InformesId = informesId;
            Modalidade = modalidade;
        }

        public long InformesId { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class SalvarInformesModalidadesCommandValidator : AbstractValidator<SalvarInformesModalidadeCommand>
    {
        public SalvarInformesModalidadesCommandValidator()
        {
            RuleFor(a => a.InformesId)
               .NotEmpty()
               .WithMessage("O id informes deve ser informado para o cadastro da modalidade informes.");

            RuleFor(a => a.Modalidade)
               .NotEmpty()
               .WithMessage("A modalidade deve ser informada para o cadastro da modalidade informes.");
        }
    }
}
