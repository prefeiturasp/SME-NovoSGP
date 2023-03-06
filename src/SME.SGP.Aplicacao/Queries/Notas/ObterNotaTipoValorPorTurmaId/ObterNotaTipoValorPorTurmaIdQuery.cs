using System.Data;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoValorPorTurmaIdQuery : IRequest<NotaTipoValor>
    {
        public ObterNotaTipoValorPorTurmaIdQuery(long turmaId, TipoTurma tipoTurma = TipoTurma.Regular)
        {
            TurmaId = turmaId;
            TipoTurma = tipoTurma;
        }
        public long TurmaId { get; set; }
        public TipoTurma TipoTurma { get; set; }
    }

    public class ObterNotaTipoValorPorTurmaIdQueryValidator : AbstractValidator<ObterNotaTipoValorPorTurmaIdQuery>
    {
        public ObterNotaTipoValorPorTurmaIdQueryValidator()
        {
            RuleFor(x => x.TurmaId).NotEmpty().WithMessage("Informe o Id da Turma para Obter a Nota Tipo Valor Por Turma");
        }
    }
}