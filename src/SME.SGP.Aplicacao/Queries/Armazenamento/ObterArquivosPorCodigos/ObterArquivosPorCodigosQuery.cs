using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivosPorCodigosQuery : IRequest<IEnumerable<Arquivo>>
    {
        public ObterArquivosPorCodigosQuery(Guid[] codigos)
        {
            Codigos = codigos;
        }

        public Guid[] Codigos { get; set; }
    }

    public class ObterArquivosPorCodigosQueryValidator : AbstractValidator<ObterArquivosPorCodigosQuery>
    {
        public ObterArquivosPorCodigosQueryValidator()
        {
            RuleFor(c => c.Codigos)
                .NotNull()
                .WithMessage("Os códigos dos arquivos devem ser informados para obter seus dados.");
        }
    }
}
