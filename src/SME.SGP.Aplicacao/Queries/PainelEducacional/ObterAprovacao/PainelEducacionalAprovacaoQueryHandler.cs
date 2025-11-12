using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacao
{
    public class PainelEducacionalAprovacaoQueryHandler : IRequestHandler<PainelEducacionalAprovacaoQuery, IEnumerable<PainelEducacionalAprovacaoDto>>
    {
        private readonly IRepositorioPainelEducacionalAprovacao repositorioPainelEducacionalAprovacao;

        public PainelEducacionalAprovacaoQueryHandler(IRepositorioPainelEducacionalAprovacao repositorioPainelEducacionalAprovacao)
        {
            this.repositorioPainelEducacionalAprovacao = repositorioPainelEducacionalAprovacao;
        }

        public async Task<IEnumerable<PainelEducacionalAprovacaoDto>> Handle(PainelEducacionalAprovacaoQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalAprovacao.ObterAprovacao(request.AnoLetivo, request.CodigoDre);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalAprovacaoDto> MapearParaDto(IEnumerable<PainelEducacionalConsolidacaoAprovacao> registros)
        {
            return registros
                .GroupBy(r => r.Modalidade)
                .Select(g => new PainelEducacionalAprovacaoDto
                {
                    Modalidade = g.Key,
                    Indicadores = g
                        .GroupBy(x => x.SerieAno)
                        .Select(a => new IndicadorAprovacaoDto
                        {
                            SerieAno = a.Key,
                            TotalPromocoes = a.Sum(z => z.TotalPromocoes),
                            TotalRetencoesAusencias = a.Sum(z => z.TotalRetencoesAusencias),
                            TotalRetencoesNotas = a.Sum(z => z.TotalRetencoesNotas),
                        })
                        .OrderBy(x => x.SerieAno)
                        .ToList()
                })
                .ToList();
        }
        public class PainelEducacionalAprovacaoQueryValidator : AbstractValidator<PainelEducacionalAprovacaoQuery>
        {
            public PainelEducacionalAprovacaoQueryValidator()
            {
                RuleFor(x => x.AnoLetivo)
                    .NotEmpty().WithMessage("Informe o ano letivo.");
            }
        }
    }
}
