using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoInformacoesEducacionais
{
    public class SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais repositorio;

        public SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommandHandler(IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;
            
            await repositorio.LimparConsolidacao();

            await repositorio.BulkInsertAsync(MapearParaEntidade(request.Indicadores));

            return true;
        }

        private static List<PainelEducacionalConsolidacaoInformacoesEducacionais> MapearParaEntidade(IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto> consolidacaoInformacoesEducacionaisDto)
        {
            return consolidacaoInformacoesEducacionaisDto
                .Select(dto => new PainelEducacionalConsolidacaoInformacoesEducacionais
                {
                    AnoLetivo = dto.AnoLetivo,
                    CodigoDre = dto.CodigoDre,
                    CodigoUe = dto.CodigoUe,
                    Ue = dto.Ue,
                    IdepAnosIniciais = dto.IdepAnosIniciais,
                    IdepAnosFinais = dto.IdepAnosFinais,
                    IdebAnosIniciais = dto.IdebAnosIniciais,
                    IdebAnosFinais = dto.IdebAnosFinais,
                    IdebEnsinoMedio = dto.IdebEnsinoMedio,
                    PercentualFrequenciaGlobal = dto.PercentualFrequenciaGlobal,
                    QuantidadeTurmasPap = dto.QuantidadeTurmasPap,
                    PercentualFrequenciaAlunosPap = dto.PercentualFrequenciaAlunosPap,
                    QuantidadeAlunosDesistentesAbandono = dto.QuantidadeAlunosDesistentesAbandono,
                    QuantidadePromocoes = dto.QuantidadePromocoes,
                    QuantidadeRetencoesFrequencia = dto.QuantidadeRetencoesFrequencia,
                    QuantidadeRetencoesNota = dto.QuantidadeRetencoesNota,
                    QuantidadeNotasAbaixoMedia = dto.QuantidadeNotasAbaixoMedia,
                    QuantidadeNotasAcimaMedia = dto.QuantidadeNotasAcimaMedia
                })
                .ToList();
        }
    }
}
