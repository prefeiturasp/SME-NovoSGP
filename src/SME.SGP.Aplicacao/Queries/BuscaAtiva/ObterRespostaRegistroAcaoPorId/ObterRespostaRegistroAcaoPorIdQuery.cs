using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaRegistroAcaoPorIdQuery : IRequest<RespostaRegistroAcaoBuscaAtiva>
    {
        public ObterRespostaRegistroAcaoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterRespostaRegistroAcaoPorIdQueryValidator : AbstractValidator<ObterRespostaRegistroAcaoPorIdQuery>
    {
        public ObterRespostaRegistroAcaoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O Id da resposta do registro de acao deve ser informado para a pesquisa");

        }
    }
}
