using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoAcompanhamentoAlunoCommand : IRequest<AuditoriaDto>
    {
        public ExcluirFotoAcompanhamentoAlunoCommand(AcompanhamentoAlunoFoto acompanhamento)
        {
            Acompanhamento = acompanhamento;
        }

        public AcompanhamentoAlunoFoto Acompanhamento { get; }
    }

    public class ExcluirFotoAcompanhamentoAlunoCommandValidator : AbstractValidator<ExcluirFotoAcompanhamentoAlunoCommand>
    {
        public ExcluirFotoAcompanhamentoAlunoCommandValidator()
        {
            RuleFor(a => a.Acompanhamento)
                .NotEmpty()
                .WithMessage("A foto de acompanhamento do estudante/criança no semestre deve ser informada para exclusão");
        }
    }
}
