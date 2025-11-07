using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
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
            var registros = await repositorioPainelEducacionalAprovacao.ObterAprovacao(request.AnoLetivo, request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalAprovacaoDto> MapearParaDto(IEnumerable<PainelEducacionalAprovacao> registros)
        {
            var aprovacao = new List<PainelEducacionalAprovacaoDto>();
            foreach (var item in registros)
            {
                aprovacao.Add(new PainelEducacionalAprovacaoDto()
                {
                    Modalidade = item.Modalidade,
                    PercentualFrequencia = item.PercentualFrequencia,
                    TotalAlunos = item.TotalAlunos,
                    TotalAulas = item.TotalAulas,
                    TotalAusencias = item.TotalAusencias
                });
            }

            return aprovacao;
        }
    }
}
