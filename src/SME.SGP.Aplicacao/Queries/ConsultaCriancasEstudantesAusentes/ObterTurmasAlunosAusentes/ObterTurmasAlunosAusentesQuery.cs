using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunosAusentesQuery : IRequest<IEnumerable<AlunosAusentesDto>>
    {
        public ObterTurmasAlunosAusentesQuery(FiltroObterAlunosAusentesDto filtro)
        {
            Filtro = filtro;
        }

        public FiltroObterAlunosAusentesDto Filtro { get; set; }
    }

    public class ObterTurmasAlunosAusentesQueryValidator : AbstractValidator<ObterTurmasAlunosAusentesQuery>
    {
        public ObterTurmasAlunosAusentesQueryValidator()
        {
            RuleFor(c => c.Filtro)
                .NotEmpty()
                .WithMessage("O filtro deve ser informado para a pesquisa das turmas alunos ausentes.");

            RuleFor(c => c.Filtro.AnoLetivo)
                .NotEmpty()
                .When(x => x.Filtro.NaoEhNulo())
                .WithMessage("O ano letivo deve ser informado para a pesquisa das turmas alunos ausentes.");

            RuleFor(c => c.Filtro.CodigoUe)
                .NotEmpty()
                .When(x => x.Filtro.NaoEhNulo())
                .WithMessage("O código da ue deve ser informado para a pesquisa das turmas alunos ausentes.");

            RuleFor(c => c.Filtro.CodigoTurma)
                .NotEmpty()
                .When(x => x.Filtro.NaoEhNulo())
                .WithMessage("O código da turma deve ser informado para a pesquisa das turmas alunos ausentes.");
        }
    }
}
