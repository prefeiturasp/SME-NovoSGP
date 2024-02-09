using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class TurmaEhTecnicaQuery : IRequest<bool>
    {
        public TurmaEhTecnicaQuery(Turma turma, DateTime dataReferencia)
        {
            Turma = turma;
            DataReferencia = dataReferencia == DateTime.MinValue ? DateTimeExtension.HorarioBrasilia().Date : dataReferencia;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
    }   

    public class TurmaEhTecnicaQueryValidator : AbstractValidator<TurmaEhTecnicaQuery>
    {
        public TurmaEhTecnicaQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotNull()
                .WithMessage("A turma deve ser informada");
        }
    }
}

