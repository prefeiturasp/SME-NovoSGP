using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterSePossuiParecerEmAprovacaoQuery : IRequest<WFAprovacaoParecerConclusivo>
    {
        public ObterSePossuiParecerEmAprovacaoQuery(long? conselhoClasseAlunoId)
        {
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
        }

        public long? ConselhoClasseAlunoId { get; }


        public class ObterSePossuiParecerEmAprovacaoQueryValidator : AbstractValidator<ObterSePossuiParecerEmAprovacaoQuery>
        {
            public ObterSePossuiParecerEmAprovacaoQueryValidator()
            {
                RuleFor(a => a.ConselhoClasseAlunoId)
                    .NotEmpty()
                    .WithMessage("O identificador do conselho de classe do aluno precisa ser informado");
            }
        }
    }
}
