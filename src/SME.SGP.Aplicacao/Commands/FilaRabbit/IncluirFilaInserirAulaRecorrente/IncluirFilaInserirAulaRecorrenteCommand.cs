using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaInserirAulaRecorrenteCommand: IRequest<bool>
    {
        public IncluirFilaInserirAulaRecorrenteCommand(Usuario usuario,
                                       DateTime dataAula,
                                       int quantidade,
                                       string codigoTurma,
                                       long componenteCurricularId,
                                       string nomeComponenteCurricular,
                                       long tipoCalendarioId,
                                       TipoAula tipoAula,
                                       string codigoUe,
                                       bool ehRegencia,
                                       RecorrenciaAula recorrenciaAula)
        {
            Usuario = usuario;
            DataAula = dataAula;
            Quantidade = quantidade;
            CodigoTurma = codigoTurma;
            ComponenteCurricularId = componenteCurricularId;
            NomeComponenteCurricular = nomeComponenteCurricular;
            TipoCalendarioId = tipoCalendarioId;
            TipoAula = tipoAula;
            CodigoUe = codigoUe;
            EhRegencia = ehRegencia;
            RecorrenciaAula = recorrenciaAula;
        }

        public Usuario Usuario { get; set; }
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

    public class IncluirFilaInserirAulaRecorrenteCommandValidator : AbstractValidator<IncluirFilaInserirAulaRecorrenteCommand>
    {
        public IncluirFilaInserirAulaRecorrenteCommandValidator()
        {
            RuleFor(c => c.DataAula)
            .NotEmpty()
            .WithMessage("A data da aula deve ser informada.");

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
