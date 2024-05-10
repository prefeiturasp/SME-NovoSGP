using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaAtualizacaoPorProcessoQueryValidator : AbstractValidator<ObterUltimaAtualizacaoPorProcessoQuery>
    {
        public ObterUltimaAtualizacaoPorProcessoQueryValidator()
        {
            RuleFor(x => x.NomeProcesso).NotEmpty().WithMessage("O nome do processo é obrigatório");
        }
    }
}
