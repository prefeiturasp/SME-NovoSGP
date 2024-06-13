using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarSecaoCommand : IRequest<ResultadoRelatorioPAPSecaoDto>
    {
        public SalvarSecaoCommand(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            Secao = secao;
            RelatorioAlunoId = relatorioAlunoId;
        }

        public  RelatorioPAPSecaoDto Secao { get; set; }
        public long RelatorioAlunoId { get; set; }
    }

    public class SalvarSecaoCommandValidator : AbstractValidator<SalvarSecaoCommand>
    {
        public SalvarSecaoCommandValidator()
        {
            RuleFor(x => x.RelatorioAlunoId).GreaterThan(0).WithMessage("Informe o id do aluno");
            RuleFor(x => x.Secao).NotEmpty().WithMessage("Informe a seção");
        }
    }
}