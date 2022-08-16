using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheAsyncQuery : IRequest<string>
    {
        public ObterCacheAsyncQuery(string nomeChave, string telemetriaNome = "", bool utilizarGZip = false)
        {
            NomeChave = nomeChave;
            TelemetriaNome = telemetriaNome;
            UtilizarGzip = utilizarGZip;
        }

        public string NomeChave { get; }
        public string TelemetriaNome { get; }
        public bool UtilizarGzip { get; }
    }
    public class ObterCacheAsyncQueryValidator : AbstractValidator<ObterCacheAsyncQuery>
    {
        public ObterCacheAsyncQueryValidator()
        {
            RuleFor(x => x.NomeChave).NotEmpty().WithMessage("Informe o nome da chave do Cache");
        }
    }
}