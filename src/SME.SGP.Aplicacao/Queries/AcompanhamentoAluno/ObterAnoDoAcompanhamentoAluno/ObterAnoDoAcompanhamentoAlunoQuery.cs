using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAnoDoAcompanhamentoAlunoQuery : IRequest<int>
    {
        public ObterAnoDoAcompanhamentoAlunoQuery(long acompanhamentoAlunoSemestreId)
        {
            AcompanhamentoAlunoSemestreId = acompanhamentoAlunoSemestreId;
        }

        public long AcompanhamentoAlunoSemestreId { get; }
    }

    public class ObterAnoDoAcompanhamentoAlunoQueryValidator : AbstractValidator<ObterAnoDoAcompanhamentoAlunoQuery>
    {
        public ObterAnoDoAcompanhamentoAlunoQueryValidator()
        {
            RuleFor(a => a.AcompanhamentoAlunoSemestreId)
                .NotEmpty()
                .WithMessage("O id do acompanhamento estudante/criança no semestre deve ser informado para consulta de seu ano");
        }
    }
}
