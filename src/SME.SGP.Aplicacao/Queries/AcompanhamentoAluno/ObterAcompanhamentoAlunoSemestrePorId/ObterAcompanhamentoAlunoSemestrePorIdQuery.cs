using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoSemestrePorIdQuery : IRequest<AcompanhamentoAlunoSemestre>
    {
        public ObterAcompanhamentoAlunoSemestrePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterAcompanhamentoAlunoSemestrePorIdQueryValidator : AbstractValidator<ObterAcompanhamentoAlunoSemestrePorIdQuery>
    {
        public ObterAcompanhamentoAlunoSemestrePorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do acompanhamento no semestre deve ser informado para consulta.");
        }
    }
}
