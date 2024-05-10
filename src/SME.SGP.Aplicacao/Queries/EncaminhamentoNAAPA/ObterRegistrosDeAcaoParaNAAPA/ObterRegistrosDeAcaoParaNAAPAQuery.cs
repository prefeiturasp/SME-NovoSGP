using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosDeAcaoParaNAAPAQuery : IRequest<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>>
    {
        public ObterRegistrosDeAcaoParaNAAPAQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; }
    }

    public class ObterRegistrosDeAcaoParaNAAPAQueryValidator : AbstractValidator<ObterRegistrosDeAcaoParaNAAPAQuery>
    {
        public ObterRegistrosDeAcaoParaNAAPAQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para a obter os registros de ação para o naapa");
        }
    }
}
