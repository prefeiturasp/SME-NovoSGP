using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroAcaoSecaoQuestaoRespostaCommand : IRequest<bool> 
    {
        public AlterarRegistroAcaoSecaoQuestaoRespostaCommand(RespostaRegistroAcaoBuscaAtiva respostaAlterar, RegistroAcaoBuscaAtivaSecaoQuestaoDto respostaQuestaoDto)
        {
            RespostaRegistroAcao = respostaAlterar;
            RespostaQuestaoDto = respostaQuestaoDto;
        }

        public RespostaRegistroAcaoBuscaAtiva RespostaRegistroAcao { get; set; }
        public RegistroAcaoBuscaAtivaSecaoQuestaoDto RespostaQuestaoDto { get; set; }
    }

    public class AlterarRegistroAcaoSecaoQuestaoRespostaCommandValidator : AbstractValidator<AlterarRegistroAcaoSecaoQuestaoRespostaCommand>
    {
        public AlterarRegistroAcaoSecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(c => c.RespostaRegistroAcao)
            .NotEmpty()
            .WithMessage("A entidade de reposta do registro de ação deve ser informada para alteração.");

            RuleFor(c => c.RespostaQuestaoDto)
            .NotEmpty()
            .WithMessage("O dto de reposta do registro de ação deve ser informada para alteração.");
        }
    }
}
