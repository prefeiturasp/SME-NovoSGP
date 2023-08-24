using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQuery : IRequest<IEnumerable<string>>
    {
        public ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQuery(long turmaId, long periodoRelatorioPAPId)
        {
            TurmaId = turmaId;
            PeriodoRelatorioPAPId = periodoRelatorioPAPId;
        }

        public long TurmaId {  get; set; }
        public long PeriodoRelatorioPAPId { get; set; }
    }

    public class ObterCodigosDeAlunosComRelatorioJaPreenchidoQueryValidator : AbstractValidator<ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQuery>
    {
        public ObterCodigosDeAlunosComRelatorioJaPreenchidoQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para obter codigos alunos relatório pap.");

            RuleFor(x => x.PeriodoRelatorioPAPId)
                .NotEmpty()
                .WithMessage("O id do período do relatório pap deve ser obter codigos alunos relatório pap.");

        }
    }
}
