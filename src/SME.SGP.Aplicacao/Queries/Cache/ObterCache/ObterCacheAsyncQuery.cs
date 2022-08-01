using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheAsyncQuery : IRequest<string>
    {
        public ObterCacheAsyncQuery(string nomeChave, bool utilizarGZip = false)
        {
            NomeChave = nomeChave;
            UtilizarGzip = utilizarGZip;
        }

        public string NomeChave { get; set; }
        public bool UtilizarGzip { get; set; }
    }
    public class ObterCacheAsyncQueryValidator : AbstractValidator<ObterCacheAsyncQuery>
    {
        public ObterCacheAsyncQueryValidator()
        {
            RuleFor(x => x.NomeChave).NotEmpty().WithMessage("Informe o nome da chave do Cache");
        }
    }
}