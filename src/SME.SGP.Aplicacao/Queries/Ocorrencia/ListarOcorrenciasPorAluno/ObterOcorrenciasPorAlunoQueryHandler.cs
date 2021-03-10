using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasPorAlunoQueryHandler : ConsultasBase, IRequestHandler<ObterOcorrenciasPorAlunoQuery, PaginacaoResultadoDto<OcorrenciasPorAlunoDto>>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;

        public ObterOcorrenciasPorAlunoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioOcorrencia repositorioOcorrencia) : base(contextoAplicacao)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
        }

        public async Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> Handle(ObterOcorrenciasPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return await MapearParaDto(await repositorioOcorrencia.ObterOcorrenciasPorTurmaAlunoEPeriodoPaginadas(request.TurmaId, 
                                                                                                                  request.AlunoCodigo, 
                                                                                                                  request.PeriodoInicio, 
                                                                                                                  request.PeriodoFim,
                                                                                                                  Paginacao));
        }

        private async Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> MapearParaDto(PaginacaoResultadoDto<OcorrenciasPorAlunoDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<OcorrenciasPorAlunoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items)
            };
        }

        private async Task<IEnumerable<OcorrenciasPorAlunoDto>> MapearParaDto(IEnumerable<OcorrenciasPorAlunoDto> ocorrencias)
        {
            var listaOcorrenciasDto = new List<OcorrenciasPorAlunoDto>();
            foreach (var ocorrencia in ocorrencias)
            {
                try
                {
                    listaOcorrenciasDto.Add(new OcorrenciasPorAlunoDto()
                    {
                        DataOcorrencia = ocorrencia.DataOcorrencia,
                        RegistradoPor = ocorrencia.RegistradoPor,
                        Titulo = ocorrencia.Titulo
                    });
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return listaOcorrenciasDto;
        }
    }
}
