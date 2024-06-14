using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoMapeamentoEstudanteCommand : IRequest<bool>
    {
        public AlterarQuestaoMapeamentoEstudanteCommand(QuestaoMapeamentoEstudante questao)
        {
            Questao = questao;
        }

        public QuestaoMapeamentoEstudante Questao { get; }
    }

    public class AlterarQuestaoMapeamentoEstudanteCommandValidator : AbstractValidator<AlterarQuestaoMapeamentoEstudanteCommand>
    {
        public AlterarQuestaoMapeamentoEstudanteCommandValidator()
        {
            RuleFor(c => c.Questao)
            .NotEmpty()
            .WithMessage("A questão do mapeamento de estudante deve ser informada para alteração.");
        }
    }
}
