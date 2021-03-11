using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoComunicadosEscolaAquiQueryValidator : AbstractValidator<ObterSituacaoComunicadosEscolaAquiQuery>
    {
        public ObterSituacaoComunicadosEscolaAquiQueryValidator()
        {
            RuleFor(x => x.AlunoCodigo).NotEmpty().WithMessage("O código do aluno é obrigatório");
            RuleFor(x => x.ComunicadosIds).NotEmpty().WithMessage("Os Ids dos Comunicados precisam ser informados");
        }
    }
}