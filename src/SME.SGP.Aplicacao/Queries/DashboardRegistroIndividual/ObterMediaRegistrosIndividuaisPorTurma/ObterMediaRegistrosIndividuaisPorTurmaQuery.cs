using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMediaRegistrosIndividuaisPorTurmaQuery : IRequest<IEnumerable<GraficoBaseQuantidadeDoubleDto>>
    {
        public ObterMediaRegistrosIndividuaisPorTurmaQuery(int anoLetivo, long dreId, long ueId, Modalidade modalidade = Modalidade.InfantilPreEscola)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public Modalidade Modalidade { get; set; }

        public class ObterMediaRegistrosIndividuaisPorTurmaQueryValidator : AbstractValidator<ObterMediaRegistrosIndividuaisPorTurmaQuery>
        {
            public ObterMediaRegistrosIndividuaisPorTurmaQueryValidator()
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
