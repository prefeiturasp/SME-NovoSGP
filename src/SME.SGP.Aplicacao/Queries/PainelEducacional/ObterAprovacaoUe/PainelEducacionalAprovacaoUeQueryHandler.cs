using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQueryHandler
        : ConsultasBase, IRequestHandler<PainelEducacionalAprovacaoUeQuery, PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe;

        public PainelEducacionalAprovacaoUeQueryHandler(
            IContextoAplicacao contextoAplicacao,
            IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe) : base(contextoAplicacao)
        {
            this.repositorioPainelEducacionalAprovacaoUe = repositorioPainelEducacionalAprovacaoUe
                ?? throw new ArgumentNullException(nameof(repositorioPainelEducacionalAprovacaoUe));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> Handle(
            PainelEducacionalAprovacaoUeQuery request,
            CancellationToken cancellationToken)
        {
            var resultado = await repositorioPainelEducacionalAprovacaoUe.ObterAprovacao(
                request.AnoLetivo,
                request.CodigoUe,
                request.ModalidadeId,
                Paginacao);

            return MapearParaDto(resultado);
        }

        private PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto> MapearParaDto(
            PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = MapearParaDto(resultadoDto.Items)
            };
        }

        private IEnumerable<PainelEducacionalAprovacaoUeDto> MapearParaDto(
            IEnumerable<PainelEducacionalAprovacaoUeDto> registros)
        {
            var listaDto = new List<PainelEducacionalAprovacaoUeDto>();
            foreach (var item in registros)
            {
                try
                {
                    listaDto.Add(new PainelEducacionalAprovacaoUeDto()
                    {
                        CodigoDre = item.CodigoDre,
                        CodigoUe = item.CodigoUe,
                        Turma = item.Turma,
                        Modalidade = item.Modalidade,
                        TotalPromocoes = item.TotalPromocoes,
                        TotalRetencoesAusencias = item.TotalRetencoesAusencias,
                        TotalRetencoesNotas = item.TotalRetencoesNotas,
                        AnoLetivo = item.AnoLetivo
                    });
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return listaDto;
        }
    }
}
