using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterSugestaoTopicoRegistroIndividualPorMesQuery : IRequest<SugestaoTopicoRegistroIndividualDto>
    {
        public ObterSugestaoTopicoRegistroIndividualPorMesQuery(int mes)
        {
            Mes = mes;
        }

        public int Mes { get; set; }
    }
    public class ObterSugestaoTopicoRegistroIndividualPorMesQueryValidator : AbstractValidator<ObterSugestaoTopicoRegistroIndividualPorMesQuery>
    {
        public ObterSugestaoTopicoRegistroIndividualPorMesQueryValidator()
        {
            RuleFor(a => a.Mes)
                .NotEmpty()
                .WithMessage("O mês deve ser informado.");

            RuleFor(a => a.Mes)
                .InclusiveBetween(2,12)
                .WithMessage("O mês deve está no intervalo de 2 a 12.");
        }
    }
}
