using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterQuestoesObrigatoriasNaoPreechidasQuery : IRequest<IEnumerable<QuestaoObrigatoriaDto>>
    {
        public ObterQuestoesObrigatoriasNaoPreechidasQuery(SecaoQuestionarioDto secao, IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> questoes)
        {
            Secao = secao;
            Questoes = questoes;
        }

        public SecaoQuestionarioDto Secao { get; set; }
        public IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> Questoes { get; set; }
    }

    public class ObterQuestoesObrigatoriasNaoPreechidasQueryValidator : AbstractValidator<ObterQuestoesObrigatoriasNaoPreechidasQuery>
    {
        public ObterQuestoesObrigatoriasNaoPreechidasQueryValidator()
        {
            RuleFor(c => c.Secao)
                .NotEmpty()
                .WithMessage("A seção do questionário deve ser informada");

            RuleFor(c => c.Questoes)
                .NotEmpty()
                .WithMessage("As questões devem ser informadas");
        }
    }
}
