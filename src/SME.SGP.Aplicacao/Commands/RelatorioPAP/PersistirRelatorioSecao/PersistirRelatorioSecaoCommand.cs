using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class PersistirRelatorioSecaoCommand : IRequest<RelatorioPeriodicoPAPSecao>
    {
        public RelatorioPAPSecaoDto Secao { get; set; }

        public PersistirRelatorioSecaoCommand(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            Secao = secao;
            RelatorioAlunoId = relatorioAlunoId;
        }

        public long RelatorioAlunoId { get; set; }
    }

    public class PersistirRelatorioSecaoCommandValidator : AbstractValidator<PersistirRelatorioSecaoCommand>
    {
        public PersistirRelatorioSecaoCommandValidator()
        {
            RuleFor(x => x.Secao).NotEmpty().WithMessage("Informe a seção");
            RuleFor(x => x.RelatorioAlunoId).GreaterThan(0).WithMessage("Informe o id do aluno");
        }
    }
}