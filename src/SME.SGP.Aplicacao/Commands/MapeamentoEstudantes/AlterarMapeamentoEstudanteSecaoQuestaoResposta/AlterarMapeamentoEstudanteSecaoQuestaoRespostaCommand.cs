using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommand : IRequest<bool> 
    {
        public AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommand(RespostaMapeamentoEstudante respostaAlterar, MapeamentoEstudanteSecaoQuestaoDto respostaQuestaoDto)
        {
            RespostaMapeamentoEstudante = respostaAlterar;
            RespostaQuestaoDto = respostaQuestaoDto;
        }

        public RespostaMapeamentoEstudante RespostaMapeamentoEstudante { get; set; }
        public MapeamentoEstudanteSecaoQuestaoDto RespostaQuestaoDto { get; set; }
    }

    public class AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommandValidator : AbstractValidator<AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommand>
    {
        public AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(c => c.RespostaMapeamentoEstudante)
            .NotEmpty()
            .WithMessage("A entidade de reposta do mapeamento de estudante deve ser informada para alteração.");

            RuleFor(c => c.RespostaQuestaoDto)
            .NotEmpty()
            .WithMessage("O dto de reposta do mapeamento de estudante deve ser informada para alteração.");
        }
    }
}
