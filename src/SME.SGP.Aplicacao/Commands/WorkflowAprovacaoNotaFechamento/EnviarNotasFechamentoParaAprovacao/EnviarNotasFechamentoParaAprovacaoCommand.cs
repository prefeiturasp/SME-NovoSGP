using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotasFechamentoParaAprovacaoCommand : IRequest
    {
        public EnviarNotasFechamentoParaAprovacaoCommand(List<FechamentoNotaDto> notasAprovacao, long fechamentoTurmaDisciplinaId, PeriodoEscolar periodoEscolar, Usuario usuarioLogado, DisciplinaDto componenteCurricular, Turma turma)
        {
            NotasAprovacao = notasAprovacao;
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            PeriodoEscolar = periodoEscolar;
            UsuarioLogado = usuarioLogado;
            ComponenteCurricular = componenteCurricular;
            Turma = turma;
        }

        public List<FechamentoNotaDto> NotasAprovacao { get; }
        public long FechamentoTurmaDisciplinaId { get; }
        public PeriodoEscolar PeriodoEscolar { get; }
        public Usuario UsuarioLogado { get; }
        public DisciplinaDto ComponenteCurricular { get; }
        public Turma Turma { get; }
    }

    public class EnviarNotasFechamentoParaAprovacaoCommandValidator : AbstractValidator<EnviarNotasFechamentoParaAprovacaoCommand>
    {
        public EnviarNotasFechamentoParaAprovacaoCommandValidator()
        {
            RuleFor(a => a.NotasAprovacao)
                .NotEmpty()
                .WithMessage("Necessário informar as notas para envio para aprovação");

            RuleFor(a => a.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("O id do fechamento turma disciplina deve ser informado para envio das notas de fechamento para aprovação");

            RuleFor(a => a.ComponenteCurricular)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para envio das notas de fechamento para aprovação");

            RuleFor(a => a.ComponenteCurricular)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para envio das notas de fechamento para aprovação");
        }
    }
}
