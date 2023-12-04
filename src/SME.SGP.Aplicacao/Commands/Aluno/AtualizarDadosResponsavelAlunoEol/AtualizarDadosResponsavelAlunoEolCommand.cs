using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDadosResponsavelAlunoEolCommand : IRequest<bool>
    {
        public AtualizarDadosResponsavelAlunoEolCommand(DadosResponsavelAlunoBuscaAtivaDto dadosResponsavelAluno)
        {
            DadosResponsavelAluno = dadosResponsavelAluno;
        }

        public DadosResponsavelAlunoBuscaAtivaDto DadosResponsavelAluno { get; set; }
    }

    public class AtualizarDadosResponsavelAlunoEolCommandValidator : AbstractValidator<AtualizarDadosResponsavelAlunoEolCommand>
    {
        public AtualizarDadosResponsavelAlunoEolCommandValidator()
        {
            RuleFor(c => c.DadosResponsavelAluno.Cpf)
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
