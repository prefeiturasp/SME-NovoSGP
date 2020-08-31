using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos
{
    public class SalvarObservacaoCartaIntencoesDto
    {
        public string Descricao { get; set; }
    }


    public class SalvarObservacaoCartaIntencoesDtoValidator : AbstractValidator<SalvarObservacaoCartaIntencoesDto>
    {
        public SalvarObservacaoCartaIntencoesDtoValidator()
        {

            RuleFor(c => c.Descricao)
                .NotEmpty()
                .WithMessage("O campo descrição deve ser informado.");
        }
    }
}
