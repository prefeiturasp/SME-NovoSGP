using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQdadeRegistrosAcaoAlunoMesQuery : IRequest<int>
    {
        public ObterQdadeRegistrosAcaoAlunoMesQuery(string codigoAluno, int mes, int ano)
        {
            CodigoAluno = codigoAluno;
            Mes = mes;
            Ano = ano;
        }
        public string CodigoAluno { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }

    }

    public class ObterQdadeRegistrosAcaoAlunoMesQueryValidator : AbstractValidator<ObterQdadeRegistrosAcaoAlunoMesQuery>
    {
        public ObterQdadeRegistrosAcaoAlunoMesQueryValidator()
        {
            RuleFor(c => c.CodigoAluno).NotEmpty().WithMessage("O código do aluno deve ser informado para pesquisa de Registros de Ação");
            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido precisa ser informado para pesquisa de Registros de Ação");
            RuleFor(c => c.Ano)
                .GreaterThan(0)
                .WithMessage("Um ano válido precisa ser informado para pesquisa de Registros de Ação");
        }
    }
}
