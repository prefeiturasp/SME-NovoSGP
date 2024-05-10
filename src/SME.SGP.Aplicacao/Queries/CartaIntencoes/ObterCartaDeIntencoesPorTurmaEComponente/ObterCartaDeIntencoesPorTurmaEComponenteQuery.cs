using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaDeIntencoesPorTurmaEComponenteQuery: IRequest<IEnumerable<CartaIntencoes>>
    {
        public ObterCartaDeIntencoesPorTurmaEComponenteQuery(string turmaCodigo, long componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ObterCartaDeIntencoesPorTurmaEComponenteQueryValidator : AbstractValidator<ObterCartaDeIntencoesPorTurmaEComponenteQuery>
    {
        public ObterCartaDeIntencoesPorTurmaEComponenteQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta das cartas de intenções");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para consulta das cartas de intenções");
        }
    }
}
