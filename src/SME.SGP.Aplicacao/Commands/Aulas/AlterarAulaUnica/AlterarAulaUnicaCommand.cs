using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaUnicaCommand: IRequest<RetornoBaseDto>
    {
        public AlterarAulaUnicaCommand(Usuario usuario,
                                       long id,
                                       DateTime dataAula,
                                       int quantidade,
                                       string codigoTurma,
                                       long componenteCurricularCodigo,
                                       string componenteCurricularNome,
                                       long tipoCalendarioId,
                                       TipoAula tipoAula,
                                       string codigoUe,
                                       bool ehRegencia)
        {
            Usuario = usuario;
            Id = id;
            DataAula = dataAula;
            CodigoTurma = codigoTurma;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            ComponenteCurricularNome = componenteCurricularNome;
            TipoCalendarioId = tipoCalendarioId;
            TipoAula = tipoAula;
            CodigoUe = codigoUe;
            EhRegencia = ehRegencia;
            Quantidade = quantidade;
        }

        public Usuario Usuario { get; set; }
        public long Id { get; set; }
        public DateTime DataAula { get; set; }
        public string CodigoTurma { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public string ComponenteCurricularNome { get; set; }
        public long TipoCalendarioId { get; set; }
        public TipoAula TipoAula { get; set; }
        public string CodigoUe { get; set; }
        public bool EhRegencia { get; set; }
        public int Quantidade { get; set; }
    }

    public class AlterarAulaUnicaCommandValidator : AbstractValidator<AlterarAulaUnicaCommand>
    {
        public AlterarAulaUnicaCommandValidator()
        {
            RuleFor(c => c.DataAula)
            .NotEmpty()
            .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.CodigoTurma)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.ComponenteCurricularCodigo)
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

            RuleFor(c => c.ComponenteCurricularNome)
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
