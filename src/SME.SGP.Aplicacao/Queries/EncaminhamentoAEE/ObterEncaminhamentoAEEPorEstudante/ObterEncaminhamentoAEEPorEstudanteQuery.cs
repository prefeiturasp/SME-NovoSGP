using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEPorEstudanteQuery : IRequest<EncaminhamentoAEEResumoDto>
    {
        public ObterEncaminhamentoAEEPorEstudanteQuery(string estudanteCodigo, string ueCodigo)
        {
            EstudanteCodigo = estudanteCodigo;
            UeCodigo = ueCodigo;
        }

        public string EstudanteCodigo { get; }
        public string UeCodigo { get;  }
    }

    public class ObterEncaminhamentoAEEPorEstudanteQueryValidator : AbstractValidator<ObterEncaminhamentoAEEPorEstudanteQuery>
    {
        public ObterEncaminhamentoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.EstudanteCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Encaminhamento AEE");

            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da UE do estudante deve ser informado para consulta de seu Encaminhamento AEE.");
        }
    }
}
