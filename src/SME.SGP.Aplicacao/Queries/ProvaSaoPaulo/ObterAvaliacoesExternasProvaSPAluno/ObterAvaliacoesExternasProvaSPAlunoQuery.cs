using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ProvaSP;
using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterAvaliacoesExternasProvaSPAlunoQuery : IRequest<IEnumerable<AvaliacaoExternaProvaSPDto>>
    {
        public ObterAvaliacoesExternasProvaSPAlunoQuery(string alunoCodigo, int anoLetivo)
        {
            AnoLetivo = anoLetivo;
            AlunoCodigo = alunoCodigo;
        }

        public int AnoLetivo { get; }
        public string AlunoCodigo { get; }
    }

    public class ObterAvaliacoesExternasProvaSPAlunoQueryValidator : AbstractValidator<ObterAvaliacoesExternasProvaSPAlunoQuery>
    {
        public ObterAvaliacoesExternasProvaSPAlunoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("Um ano letivo válido deve ser informado para pesquisa de avaliações externas Prova SP");

            RuleFor(x => x.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para pesquisa de avaliações externas Prova SP");
        }
    }
}
