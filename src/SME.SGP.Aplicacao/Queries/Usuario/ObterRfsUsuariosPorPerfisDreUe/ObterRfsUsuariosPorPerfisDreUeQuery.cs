using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterRfsUsuariosPorPerfisDreUeQuery : IRequest<string[]>
    {
        public ObterRfsUsuariosPorPerfisDreUeQuery(string ue, string dre, string[] perfis)
        {
            Ue = ue;
            Dre = dre;
            Perfis = perfis;
        }

        public string Ue { get; set; }
        public string Dre { get; set; }
        public string[] Perfis { get; set; }
    }

    public class ObterRfsUsuariosPorPerfisDreUeQueryValidator : AbstractValidator<ObterRfsUsuariosPorPerfisDreUeQuery>
    {
        public ObterRfsUsuariosPorPerfisDreUeQueryValidator()
        {

            RuleFor(c => c.Perfis)
                .NotEmpty()
                .WithMessage("O perfis devem ser informados para busca dos usuários.");
        }
    }

}
