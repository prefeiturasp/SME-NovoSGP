using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaExcluirPendenciaAusenciaFechamentoCommand : IRequest<bool>
    {
        public PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(FechamentoTurmaDisciplinaDto fechamentoTurmaDisciplinaDto, Usuario usuario)
        {
            FechamentoTurmaDisciplinaDto = fechamentoTurmaDisciplinaDto;
            Usuario = usuario;
        }

        public FechamentoTurmaDisciplinaDto FechamentoTurmaDisciplinaDto { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class PublicaFilaExcluirPendenciaAusenciaFechamentoCommandValidator : AbstractValidator<PublicaFilaExcluirPendenciaAusenciaFechamentoCommand>
    {
        public PublicaFilaExcluirPendenciaAusenciaFechamentoCommandValidator()
        {
            RuleFor(c => c.Usuario)
               .NotEmpty()
               .WithMessage("O usuário precisa ser informado para verificação de exclusão de pendencia de ausencia de fechamento.");

            RuleFor(c => c.FechamentoTurmaDisciplinaDto)
               .NotEmpty()
               .WithMessage("O FechamentoTurmaDisciplina precisa ser informado para verificação de exclusão de pendencia de ausencia de fechamento.");
        }
    }
}
