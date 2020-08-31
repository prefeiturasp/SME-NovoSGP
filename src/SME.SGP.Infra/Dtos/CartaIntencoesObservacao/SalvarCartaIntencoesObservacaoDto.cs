using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos
{
    public class SalvarCartaIntencoesObservacaoDto
    {
        public string Observacao { get; set; }
    }


    //public class SalvarCartaIntencoesObservacaoDtoValidator : AbstractValidator<SalvarCartaIntencoesObservacaoDto>
    //{
    //    public SalvarCartaIntencoesObservacaoDtoValidator()
    //    {

    //        RuleFor(c => c.Observacao)
    //            .NotEmpty()
    //            .WithMessage("O campo observação deve ser informado.");
    //    }
    //}
}
