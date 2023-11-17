using System.Data;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoValorPorTurmaIdQuery : IRequest<NotaTipoValor>
    {
        public ObterNotaTipoValorPorTurmaIdQuery(Turma turma)
        {
            Turma = turma;
        }
        public Turma Turma { get; set; }
    }

    public class ObterNotaTipoValorPorTurmaIdQueryValidator : AbstractValidator<ObterNotaTipoValorPorTurmaIdQuery>
    {
        public ObterNotaTipoValorPorTurmaIdQueryValidator()
        {
            RuleFor(x => x.Turma.Id)
                .NotEmpty()
                .When(x => x.Turma.NaoEhNulo())
                .WithMessage("Informe o Id da Turma para Obter a Nota Tipo Valor Por Turma");
        }
    }
}