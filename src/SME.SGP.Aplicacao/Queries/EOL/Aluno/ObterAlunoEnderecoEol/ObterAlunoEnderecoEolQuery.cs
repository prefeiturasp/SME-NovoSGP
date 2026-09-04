using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoEnderecoEolQuery : IRequest<AlunoEnderecoRespostaDto>
    {
        public ObterAlunoEnderecoEolQuery(string codigoAluno)
        {
            this.CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; }
    }

    public class ObterAlunoEnderecoEolQueryValidator : AbstractValidator<ObterAlunoEnderecoEolQuery>
    {
        public ObterAlunoEnderecoEolQueryValidator()
        {
            RuleFor(x => x.CodigoAluno).NotEmpty().WithMessage("Informe o código do aluno para obter as informações do EOL");
        }
    }
}