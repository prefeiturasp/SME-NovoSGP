using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoEducacaoIntegral
{
    public class SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoEducacaoIntegral repositorio;

        public SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommandHandler(IRepositorioPainelEducacionalConsolidacaoEducacaoIntegral repositorio)
        {
            this.repositorio= repositorio;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            await repositorio.LimparConsolidacao(request.AnoLetivo);

            await repositorio.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static List<PainelEducacionalConsolidacaoEducacaoIntegral> MapearParaEntidade(IEnumerable<DadosParaConsolidarEducacaoIntegralDto> consolidacaoEducacaoIntegralDto)
        {
            return consolidacaoEducacaoIntegralDto
                .Select(dto => new PainelEducacionalConsolidacaoEducacaoIntegral
                {
                    AnoLetivo = dto.AnoLetivo,
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    ModalidadeTurma = dto.ModalidadeTurma,
                    Ano = dto.Ano,
                    QuantidadeAlunosIntegral = dto.QuantidadeAlunosIntegral,
                    QuantidadeAlunosParcial = dto.QuantidadeAlunosParcial,
                })
                .ToList();
        }
    }
}
