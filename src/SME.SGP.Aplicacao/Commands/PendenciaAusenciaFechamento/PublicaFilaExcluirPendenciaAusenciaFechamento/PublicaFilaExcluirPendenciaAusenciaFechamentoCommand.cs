using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaExcluirPendenciaAusenciaFechamentoCommand : IRequest<bool>
    {

        public PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(long disciplinaId, long periodoEscolarId, string turmaId, Usuario usuarioLogado)
        {
            this.DisciplinaId = disciplinaId;
            this.PeriodoEscolarId = periodoEscolarId;
            this.TurmaCodigo = turmaId;
            this.UsuarioLogado = usuarioLogado;
        }

        public long DisciplinaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public string TurmaCodigo { get; set; }
        public Usuario UsuarioLogado { get; set; }
    }

    public class PublicaFilaExcluirPendenciaAusenciaFechamentoCommandValidator : AbstractValidator<PublicaFilaExcluirPendenciaAusenciaFechamentoCommand>
    {
        public PublicaFilaExcluirPendenciaAusenciaFechamentoCommandValidator()
        {
            RuleFor(c => c.UsuarioLogado)
               .NotEmpty()
               .WithMessage("O usuário precisa ser informado para verificação de exclusão de pendencia de ausencia de fechamento.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O PeriodoEscolar precisa ser informado para verificação de exclusão de pendencia de ausencia de fechamento.");

            RuleFor(c => c.DisciplinaId)
               .NotEmpty()
               .WithMessage("A DisciplinaId precisa ser informado para verificação de exclusão de pendencia de ausencia de fechamento.");

            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("A TurmaCodigo precisa ser informado para verificação de exclusão de pendencia de ausencia de fechamento.");
        }
    }
}
