using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery : IRequest<QuestaoEncaminhamentoNAAPA>
    {
        public ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterEnderecoAlunoEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterEnderecoAlunoEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para a busca do endereço residencial do aluno.");
        }
    }
}
