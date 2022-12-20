using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesObrigatoriasNaoRespondidasQuery : IRequest<IEnumerable<QuestaoObrigatoriaNaoRespondidaDto>>
    {
       
        public ObterQuestoesObrigatoriasNaoRespondidasQuery(SecaoQuestionarioDto secaoAValidar, 
                                                            IEnumerable<RespostaQuestaoObrigatoriaDto> respostas,
                                                            Func<SecaoQuestionarioDto, IEnumerable<QuestaoDto>, List<QuestaoObrigatoriaNaoRespondidaDto>, Task> addQuestoesObrigatoriasNaoPreenchidasEspecificas = null)
        {
            Secao = secaoAValidar;
            Respostas = respostas;
            AddQuestoesObrigatoriasNaoPreenchidas = addQuestoesObrigatoriasNaoPreenchidasEspecificas;
        }

        public SecaoQuestionarioDto Secao { get; }
        public IEnumerable<RespostaQuestaoObrigatoriaDto> Respostas { get; }
        public Func<SecaoQuestionarioDto, IEnumerable<QuestaoDto>, List<QuestaoObrigatoriaNaoRespondidaDto>, Task> AddQuestoesObrigatoriasNaoPreenchidas { get; }

    }

    public class ObterQuestoesObrigatoriasNaoRespondidasQueryValidator : AbstractValidator<ObterQuestoesObrigatoriasNaoRespondidasQuery>
    {
        public ObterQuestoesObrigatoriasNaoRespondidasQueryValidator()
        {
            RuleFor(a => a.Secao)
                .NotEmpty()
                .WithMessage("As seções a serem validadas devem ser preenchidas para obtenção das questões obrigatórias não respondidas.");
        }
    }
}
