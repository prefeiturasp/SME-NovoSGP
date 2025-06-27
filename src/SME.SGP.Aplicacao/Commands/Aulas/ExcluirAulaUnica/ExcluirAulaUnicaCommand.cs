using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUnicaCommand: IRequest<RetornoBaseDto>
    {
        public ExcluirAulaUnicaCommand(Usuario usuario, Aula aula)
        {
            Usuario = usuario;
            Aula = aula;
        }

        public Usuario Usuario { get; set; }
        public Aula Aula { get; set; } = new Aula();
    }

    public class ExcluirAulaUnicaCommandValidator: AbstractValidator<ExcluirAulaUnicaCommand>
    {
        public ExcluirAulaUnicaCommandValidator()
        {
        }
    }
}
