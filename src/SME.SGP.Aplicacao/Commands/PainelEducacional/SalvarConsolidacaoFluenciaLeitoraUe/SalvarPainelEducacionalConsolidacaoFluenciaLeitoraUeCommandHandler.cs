using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoFluenciaLeitoraUe
{
    public class SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe repositorio;

        public SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommandHandler(IRepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe repositorio)
        {
            this.repositorio= repositorio;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            await repositorio.LimparConsolidacao(request.AnoLetivo);

            await repositorio.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static List<PainelEducacionalConsolidacaoFluenciaLeitoraUe> MapearParaEntidade(IEnumerable<ConsolidacaoFluenciaLeitoraUeDto> consolidacaoFluenciaLeitoraUeDto)
        {
            return consolidacaoFluenciaLeitoraUeDto
                .Select(dto => new PainelEducacionalConsolidacaoFluenciaLeitoraUe
                {
                    AnoLetivo = dto.AnoLetivo,
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    CodigoTurma = dto.CodigoTurma,
                    Turma = dto.Turma,
                    AlunosPrevistos = dto.AlunosPrevistos,
                    AlunosAvaliados = dto.AlunosAvaliados,
                    TipoAvaliacao = dto.TipoAvaliacao,
                    PreLeitorTotal = dto.PreLeitorTotal,
                    Fluencia = dto.Fluencia,
                    QuantidadeAlunoFluencia = dto.QuantidadeAlunoFluencia,
                    PercentualFluencia = dto.PercentualFluencia

                })
                .ToList();
        }
    }
}
