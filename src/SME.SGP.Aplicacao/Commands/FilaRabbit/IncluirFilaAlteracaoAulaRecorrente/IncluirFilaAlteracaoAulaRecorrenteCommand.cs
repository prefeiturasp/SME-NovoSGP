using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaAlteracaoAulaRecorrenteCommand: IRequest<bool>
    {
        public IncluirFilaAlteracaoAulaRecorrenteCommand(Usuario usuario, PersistirAulaDto aulaDto)
        {
            Usuario = usuario;
            AulaId = aulaDto.Id;
            DataAula = aulaDto.DataAula;
            Quantidade = aulaDto.Quantidade;
            CodigoTurma = aulaDto.CodigoTurma;
            ComponenteCurricularId = aulaDto.CodigoComponenteCurricular;
            NomeComponenteCurricular = aulaDto.NomeComponenteCurricular;
            TipoCalendarioId = aulaDto.TipoCalendarioId;
            TipoAula = aulaDto.TipoAula;
            CodigoUe = aulaDto.CodigoUe;
            EhRegencia = aulaDto.EhRegencia;
            RecorrenciaAula = aulaDto.RecorrenciaAula;
        }

        public Usuario Usuario { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public int Quantidade { get; set; }
        public string CodigoTurma { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string NomeComponenteCurricular { get; set; }
        public long TipoCalendarioId { get; set; }
        public TipoAula TipoAula { get; set; }
        public string CodigoUe { get; set; }
        public bool EhRegencia { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
    }

    public class IncluirFilaAlteracaoAulaRecorrenteCommandValidator : AbstractValidator<IncluirFilaAlteracaoAulaRecorrenteCommand>
    {
        public IncluirFilaAlteracaoAulaRecorrenteCommandValidator()
        {
            RuleFor(c => c.DataAula)
            .NotEmpty()
            .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O Id da aula deve ser informado.");

            RuleFor(c => c.CodigoTurma)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.ComponenteCurricularId)
            .NotEmpty()
            .WithMessage("O código do componente curricular deve ser informado.");

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.Usuario)
           .NotEmpty()
           .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O código do tipo de calendário deve ser informado.");

            RuleFor(c => c.NomeComponenteCurricular)
            .NotEmpty()
            .WithMessage("O nome do componente curricular deve ser informado.");

            RuleFor(c => c.Quantidade)
            .NotEmpty()
            .WithMessage("A quantidade de aulas deve ser informada.");

            RuleFor(c => c.TipoAula)
                .IsInEnum()
                .WithMessage("O tipo de aula deve ser informado.");
        }
    }
}
