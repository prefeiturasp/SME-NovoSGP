using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPeriodicoSecaoPAPQuery : IRequest<RelatorioPeriodicoPAPSecao>
    {
        public ObterRelatorioPeriodicoSecaoPAPQuery(long relatorioSecaoId)
        {
            RelatorioSecaoId = relatorioSecaoId;
        }

        public long RelatorioSecaoId {  get; set; }
    }

    public class ObterRelatorioPeriodicoSecaoPAPQueryValidator : AbstractValidator<ObterRelatorioPeriodicoSecaoPAPQuery>
    {
        public ObterRelatorioPeriodicoSecaoPAPQueryValidator()
        {
            RuleFor(x => x.RelatorioSecaoId)
                .NotEmpty()
                .WithMessage("O id relatório periodico seção pap deve ser informado para busca.");
        }
    }
}
