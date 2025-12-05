using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCabecalhoAtendimentoNAAPAQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterCabecalhoAtendimentoNAAPAQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; set; }
    }

    public class ObterCabecalhoAtendimentoNAAPAQueryValidator : AbstractValidator<ObterCabecalhoAtendimentoNAAPAQuery>
    {
        public ObterCabecalhoAtendimentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId).NotEmpty().WithMessage("O Id do Atendimento NAAPA deve ser informado para busca");
        }
    }
}
