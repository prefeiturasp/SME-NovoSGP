using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaPorIdQuery : IRequest<CompensacaoAusencia>
    {
        public ObterCompensacaoAusenciaPorIdQuery(long compensacaoAusenciaId)
        {
            CompensacaoAusenciaId = compensacaoAusenciaId;
        }

        public long CompensacaoAusenciaId { get; set; }   
    }

    public class ObterCompensacaoAusenciaPorIdQueryValidator : AbstractValidator<ObterCompensacaoAusenciaPorIdQuery>
    {
        public ObterCompensacaoAusenciaPorIdQueryValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaId).GreaterThan(0).WithMessage("Deve ser informado o Id da Compensação ausência para realizar a consulta");
        }
    }
}