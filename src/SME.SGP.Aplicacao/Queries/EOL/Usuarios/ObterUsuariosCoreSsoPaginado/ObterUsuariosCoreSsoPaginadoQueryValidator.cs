using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPaginado
{
    public class ObterUsuariosCoreSsoPaginadoQueryValidator : AbstractValidator<ObterUsuariosCoreSsoPaginadoQuery>
    {
        public ObterUsuariosCoreSsoPaginadoQueryValidator()
        {
            RuleFor(c => c.Pagina)
                .GreaterThan(0)
                .WithMessage("O número da página deve ser maior que zero.");
            RuleFor(c => c.RegistrosPorPagina)
                .GreaterThan(0)
                .WithMessage("A quantidade de registros por página deve ser maior que zero.");
        }
    }
}
