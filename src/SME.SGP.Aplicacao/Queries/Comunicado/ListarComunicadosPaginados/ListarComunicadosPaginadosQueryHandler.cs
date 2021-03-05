using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ListarComunicadosPaginadosQueryHandler : ConsultasBase, IRequestHandler<ListarComunicadosPaginadosQuery, PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ListarComunicadosPaginadosQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioComunicado repositorioComunicado) : base(contextoAplicacao)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new System.ArgumentNullException(nameof(repositorioComunicado));
        }
        public async Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> Handle(ListarComunicadosPaginadosQuery request, CancellationToken cancellationToken)
        {
            return MapearParaDto(await repositorioComunicado.ObterComunicadosReduzidos(request.DRECodigo, request.UECodigo, request.TurmaCodigo, request.AlunoCodigo, Paginacao));
        }

        private PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto> MapearParaDto(PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = MapearComunicados(resultadoDto.Items)
            };
        }

        private IEnumerable<ComunicadoAlunoReduzidoDto> MapearComunicados(IEnumerable<ComunicadoAlunoReduzidoDto> comunicados)
        {
            var listaComunicados = new List<ComunicadoAlunoReduzidoDto>();

            foreach (var comunicado in comunicados)
            {
                listaComunicados.Add(new ComunicadoAlunoReduzidoDto()
                {
                    DataEnvio = comunicado.DataEnvio.Date,
                    Titulo = comunicado.Titulo,
                    Categoria = comunicado.Categoria,
                    CategoriaNome = comunicado.Categoria.Name(),
                    StatusLeitura = "Não Lida"
                }); ;
            }

            return listaComunicados;
        }
    }
}
