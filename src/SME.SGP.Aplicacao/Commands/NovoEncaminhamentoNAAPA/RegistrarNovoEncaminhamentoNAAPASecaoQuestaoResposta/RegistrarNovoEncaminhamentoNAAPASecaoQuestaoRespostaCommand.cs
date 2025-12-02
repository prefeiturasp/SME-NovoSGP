using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecaoQuestaoResposta
{
    public class RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand : IRequest<long>
    {
        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand(string resposta, long questaoId, TipoQuestao tipoQuestao)
        {
            Resposta = resposta;
            QuestaoId = questaoId;
            TipoQuestao = tipoQuestao;
        }
    }

    public class RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator : AbstractValidator<RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand>
    {
        public RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento NAAPA deve ser informada!");
        }
    }
}