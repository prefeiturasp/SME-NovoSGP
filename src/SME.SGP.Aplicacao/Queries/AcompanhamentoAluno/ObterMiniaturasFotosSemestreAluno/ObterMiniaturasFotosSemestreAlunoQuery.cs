using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturasFotosSemestreAlunoQuery : IRequest<IEnumerable<Arquivo>>
    {
        public ObterMiniaturasFotosSemestreAlunoQuery(long acompanhamentoSemestreId)
        {
            AcompanhamentoSemestreId = acompanhamentoSemestreId;
        }

        public long AcompanhamentoSemestreId { get; }
    }

    public class ObterMiniaturasFotosSemestreAlunoQueryValidator : AbstractValidator<ObterMiniaturasFotosSemestreAlunoQuery>
    {
        public ObterMiniaturasFotosSemestreAlunoQueryValidator()
        {
            RuleFor(a => a.AcompanhamentoSemestreId)
                .NotEmpty()
                .WithMessage("O id do acompanhamento do estudante/criança no semestre deve ser informado para consulta de suas fotos");
        }
    }
}
