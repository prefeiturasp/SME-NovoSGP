using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao.Commands.Aulas.InserirAula
{
    public class InserirAulaUnicaCommand : IRequest<RetornoBaseDto>
    {
        public InserirAulaUnicaCommand(Usuario usuario,
                                       DateTime dataAula,
                                       int quantidade,
                                       string codigoTurma,
                                       long componenteCurricularId,
                                       string nomeComponenteCurricular,
                                       long tipoCalendarioId,
                                       TipoAula tipoAula,
                                       string codigoUe,
                                       bool ehRegencia)
        {
            Usuario = usuario;
            DataAula = dataAula;
            Quantidade = quantidade;
            CodigoComponenteCurricular = componenteCurricularId;
            NomeComponenteCurricular = nomeComponenteCurricular;
            TipoCalendarioId = tipoCalendarioId;
            TipoAula = tipoAula;
            CodigoUe = codigoUe;
            EhRegencia = ehRegencia;
            CodigoTurma = codigoTurma;
        }

        public DateTime DataAula { get; private set; }
        public long CodigoComponenteCurricular { get; private set; }
        public string CodigoUe { get; private set; }
        public Usuario Usuario { get; private set; }
        public string NomeComponenteCurricular { get; private set; }
        public long TipoCalendarioId { get; }
        public int Quantidade { get; private set; }
        public TipoAula TipoAula { get; private set; }
        public string CodigoTurma { get; private set; }
        public bool EhRegencia { get; internal set; }
    }

    public class InserirAulaUnicaCommandValidator : AbstractValidator<InserirAulaUnicaCommand>
    {
        public InserirAulaUnicaCommandValidator()
        {
            RuleFor(c => c.DataAula)
            .NotEmpty()
            .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.CodigoTurma)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado.");


            RuleFor(c => c.CodigoComponenteCurricular)
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
