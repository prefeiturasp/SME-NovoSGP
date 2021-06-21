using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMediaRegistrosIndividuaisPorAnoQuery : IRequest<IEnumerable<GraficoBaseQuantidadeDoubleDto>>
    {
        public ObterMediaRegistrosIndividuaisPorAnoQuery(int anoLetivo, long dreId, Modalidade modalidade = Modalidade.InfantilPreEscola)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public Modalidade Modalidade { get; set; }

        public class ObterMediaRegistrosIndividuaisPorAnoQueryValidator : AbstractValidator<ObterMediaRegistrosIndividuaisPorAnoQuery>
        {
            public ObterMediaRegistrosIndividuaisPorAnoQueryValidator()
            {
                RuleFor(a => a.AnoLetivo)
                    .NotEmpty()
                    .WithMessage("O ano letivo deve ser informado para obter os dados de média de registros individuais");
                RuleFor(a => a.Modalidade)
                    .NotEmpty()
                    .WithMessage("A modalidade deve ser informada para obter os dados de média de registros individuais");
            }
        }
    }
}
