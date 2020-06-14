using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosSimplesDaTurmaQuery: IRequest<IEnumerable<AlunoSituacaoDto>>
    {
        public ObterAlunosAtivosSimplesDaTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }

    public class ObterAlunosAtivosSimplesDaTurmaQueryValidator : AbstractValidator<ObterAlunosAtivosSimplesDaTurmaQuery>
    {
        public ObterAlunosAtivosSimplesDaTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
