using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterUeDetalhesParaSincronizacaoInstitucionalQuery : IRequest<UeDetalhesParaSincronizacaoInstituicionalDto>
    {
        public ObterUeDetalhesParaSincronizacaoInstitucionalQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }
    public class ObterUeDetalhesParaSincronizacaoInstitucionalQueryValidator : AbstractValidator<ObterUeDetalhesParaSincronizacaoInstitucionalQuery>
    {
        public ObterUeDetalhesParaSincronizacaoInstitucionalQueryValidator()
        {

            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
        }
    }
}
