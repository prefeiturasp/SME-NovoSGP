using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAComTurmaPorIdQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterAtendimentoNAAPAComTurmaPorIdQuery(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }

    public class ObterAtendimentoNAAPAComTurmaPorIdQueryValidator : AbstractValidator<ObterAtendimentoNAAPAComTurmaPorIdQuery>
    {
        public ObterAtendimentoNAAPAComTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("O id do atendimento NAAPA é necessário para pesquisa.");
        }
    }
}
