using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao.Commands.Aulas.InserirAula
{
    public class InserirAulaUnicaCommand : IRequest<RetornoBaseDto>
    {
        public InserirAulaUnicaCommand(Usuario usuario, DateTime dataAula, int quantidade, Turma turma, long componenteCurricularId, string nomeComponenteCurricular, TipoCalendario tipoCalendario, TipoAula tipoAula, string codigoUe)
        {
            Usuario = usuario;
            DataAula = dataAula;
            Quantidade = quantidade;
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
            NomeComponenteCurricular = nomeComponenteCurricular;
            TipoCalendario = tipoCalendario;
            TipoAula = tipoAula;
            CodigoUe = codigoUe;
        }

        public DateTime DataAula { get; private set; }
        public Turma Turma { get; private set; }
        public long ComponenteCurricularId { get; private set; }
        public string CodigoUe { get; private set; }
        public Usuario Usuario { get; private set; }
        public TipoCalendario TipoCalendario { get; private set; }
        public string NomeComponenteCurricular { get; private set; }
        public int Quantidade { get; private set; }
        public TipoAula TipoAula { get; private set; }
    }


    public class InserirAulaUnicaCommandValidator : AbstractValidator<InserirAulaUnicaCommand>
    {
        public InserirAulaUnicaCommandValidator()
        {
            RuleFor(c => c.DataAula)
            .NotEmpty()
            .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.Turma)
            .NotEmpty()
            .WithMessage("A turma deve ser informada.");


            RuleFor(c => c.ComponenteCurricularId)
            .NotEmpty()
            .WithMessage("O código do componente curricular deve ser informado.");


            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.Usuario)
           .NotEmpty()
           .WithMessage("O usuário deve ser informado.");


            RuleFor(c => c.TipoCalendario)
            .NotEmpty()
            .WithMessage("O tipo de calendário deve ser informado.");


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
