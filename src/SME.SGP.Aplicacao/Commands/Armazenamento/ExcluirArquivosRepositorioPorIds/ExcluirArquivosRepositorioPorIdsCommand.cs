using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivosRepositorioPorIdsCommand : IRequest<bool>
    {
        public ExcluirArquivosRepositorioPorIdsCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; set; }
    }

    public class ExcluirArquivosRepositorioPorIdsCommandValidator : AbstractValidator<ExcluirArquivosRepositorioPorIdsCommand>
    {
        public ExcluirArquivosRepositorioPorIdsCommandValidator()
        {
            RuleFor(c => c.Ids)
                .NotEmpty()
                .NotNull()
                .WithMessage("Informe pelo menos um Arquivo para exclusão ");
        }
    }
}