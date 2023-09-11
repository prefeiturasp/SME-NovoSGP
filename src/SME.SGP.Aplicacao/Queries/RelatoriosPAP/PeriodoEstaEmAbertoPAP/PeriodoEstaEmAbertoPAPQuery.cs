using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class PeriodoEstaEmAbertoPAPQuery : IRequest<bool>
    {
        public PeriodoEstaEmAbertoPAPQuery(long periodoRelatorioId, Turma turma)
        {
            PeriodoRelatorioId = periodoRelatorioId;
            Turma = turma;
        }

        public long PeriodoRelatorioId { get; set; }
        public Turma Turma { get; set; }
    }

    public class PeriodoEstaEmAbertoPAPQueryValidator : AbstractValidator<PeriodoEstaEmAbertoPAPQuery>
    {
        public PeriodoEstaEmAbertoPAPQueryValidator()
        {
            RuleFor(x => x.PeriodoRelatorioId)
                .NotEmpty()
                .WithMessage("O periódo de relatório pap id deve ser informado para verificar o periódo de abertura.");

            RuleFor(x => x.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informado para verificar o periódo de abertura.");
        }
    }
}
