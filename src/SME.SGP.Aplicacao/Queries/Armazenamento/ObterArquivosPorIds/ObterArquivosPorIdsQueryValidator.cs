using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivosPorIdsQueryValidator : AbstractValidator<ObterArquivosPorIdsQuery>
    {
        public ObterArquivosPorIdsQueryValidator()
        {
            RuleFor(c => c.Ids)
                .NotNull()
                .WithMessage("Os Ids dos arquivos devem ser informados para obter seus dados.");
        }
    }
}