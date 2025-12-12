using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarNovoEncaminhamentoNAAPASecaoQuestaoResposta
{
    public class AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand : IRequest<bool>
    {
        public AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand(RespostaEncaminhamentoEscolar respostaAlterar, NovoEncaminhamentoNAAPASecaoQuestaoDto novoEncaminhamentoNAAPASecaoQuestaoDto)
        {
            RespostaEncaminhamento = respostaAlterar;
            RespostaQuestaoDto = novoEncaminhamentoNAAPASecaoQuestaoDto;
        }

        public RespostaEncaminhamentoEscolar RespostaEncaminhamento { get; set; }
        public NovoEncaminhamentoNAAPASecaoQuestaoDto RespostaQuestaoDto { get; set; }
    }

    public class AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator : AbstractValidator<AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand>
    {
        public AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(c => c.RespostaEncaminhamento)
            .NotEmpty()
            .WithMessage("A entidade de reposta do encaminhamento NAAPA deve ser informada para alteração.");

            RuleFor(c => c.RespostaQuestaoDto)
            .NotEmpty()
            .WithMessage("O dto de reposta do encaminhamento NAAPA deve ser informada para alteração.");
        }
    }
}