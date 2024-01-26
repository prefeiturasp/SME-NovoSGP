using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDadosResponsavelAlunoProdamCommand : IRequest<bool>
    {
        public AtualizarDadosResponsavelAlunoProdamCommand(DadosResponsavelAlunoProdamDto dadosResponsavelAluno)
        {
            DadosResponsavelAluno = dadosResponsavelAluno;
        }

        public DadosResponsavelAlunoProdamDto DadosResponsavelAluno { get; set; }
    }

    public class AtualizarDadosResponsavelAlunoProdamCommandValidator : AbstractValidator<AtualizarDadosResponsavelAlunoProdamCommand>
    {
        public AtualizarDadosResponsavelAlunoProdamCommandValidator()
        {
            RuleFor(c => c.DadosResponsavelAluno.CPF)
            .NotEmpty()
            .When(x => x.DadosResponsavelAluno.NaoEhNulo())
            .WithMessage("O cpf deve ser informado para atualização dos dados responsável do aluno.");

            RuleFor(c => c.DadosResponsavelAluno.CodigoAluno)
            .NotEmpty()
            .When(x => x.DadosResponsavelAluno.NaoEhNulo())
            .WithMessage("O código do aluno deve ser informado para atualização dos dados responsável do aluno.");
        }
    }
}
