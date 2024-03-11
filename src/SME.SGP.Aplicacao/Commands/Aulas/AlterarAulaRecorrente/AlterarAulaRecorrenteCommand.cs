using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaRecorrenteCommand : IRequest<bool>
    {
        public AlterarAulaRecorrenteCommand() { }

        public AlterarAulaRecorrenteCommand(IncluirFilaAlteracaoAulaRecorrenteCommand aulaRecorrente)
        {
            Usuario = aulaRecorrente.Usuario;
            AulaId = aulaRecorrente.AulaId;
            DataAula = aulaRecorrente.DataAula;
            Quantidade = aulaRecorrente.Quantidade;
            CodigoTurma = aulaRecorrente.CodigoTurma;
            ComponenteCurricularId = aulaRecorrente.ComponenteCurricularId;
            NomeComponenteCurricular = aulaRecorrente.NomeComponenteCurricular;
            TipoCalendarioId = aulaRecorrente.TipoCalendarioId;
            TipoAula = aulaRecorrente.TipoAula;
            CodigoUe = aulaRecorrente.CodigoUe;
            EhRegencia = aulaRecorrente.EhRegencia;
            RecorrenciaAula = aulaRecorrente.RecorrenciaAula;
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

    public class AlterarAulaRecorrenteCommandValidator : AbstractValidator<AlterarAulaRecorrenteCommand>
    {
        public AlterarAulaRecorrenteCommandValidator()
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
