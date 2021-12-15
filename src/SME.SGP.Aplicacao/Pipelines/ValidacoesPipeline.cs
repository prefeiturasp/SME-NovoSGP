using FluentValidation;
using MediatR;
using SME.SGP.Infra.Excecoes;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Pipelines
{
    public class ValidacoesPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validadores;

        public ValidacoesPipeline(IEnumerable<IValidator<TRequest>> validadores)
        {
            this.validadores = validadores ?? throw new System.ArgumentNullException(nameof(validadores));
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (validadores.Any())
            {
                var context = new ValidationContext(request);

                var erros = validadores.Select(v => v.Validate(context))
                                       .SelectMany(result => result.Errors)
                                       .Where(f => f != null)
                                       .ToList();

                if (erros != null && erros.Any())
                    throw new ValidacaoException(erros);
            }

            return next();
        }
    }
}
