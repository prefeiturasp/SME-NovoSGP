using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheQuery : IRequest<string>
    {
        public ObterCacheQuery(string nomeChave, bool utilizarGZip = false)
        {
            NomeChave = nomeChave;
            UtilizarGzip = utilizarGZip;
        }

        public string NomeChave { get; set; }
        public bool UtilizarGzip { get; set; }
    }

    public class ObterCacheQueryValidator : AbstractValidator<ObterCacheQuery>
    {
        public ObterCacheQueryValidator()
        {
            RuleFor(x => x.NomeChave).NotEmpty().WithMessage("Informe o nome da chave do Cache");
        }
    }

}