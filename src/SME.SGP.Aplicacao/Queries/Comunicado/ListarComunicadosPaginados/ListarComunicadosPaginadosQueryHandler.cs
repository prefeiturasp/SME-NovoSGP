using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ListarComunicadosPaginadosQueryHandler : ConsultasBase, IRequestHandler<ListarComunicadosPaginadosQuery, PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IMediator mediator;

        public ListarComunicadosPaginadosQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioComunicado repositorioComunicado, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new System.ArgumentNullException(nameof(repositorioComunicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator)); ;
        }
        public async Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> Handle(ListarComunicadosPaginadosQuery request, CancellationToken cancellationToken)
        {
            return await MapearParaDtoAsync(await repositorioComunicado.ObterComunicadosReduzidos(request.DRECodigo, request.UECodigo, request.TurmaCodigo, request.AlunoCodigo, Paginacao), request.AlunoCodigo);
        }

        private async Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> MapearParaDtoAsync(PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto> resultadoDto, string alunoCodigo)
        {
            return new PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearComunicados(resultadoDto.Items, alunoCodigo)
            };
        }

        private async Task<IEnumerable<ComunicadoAlunoReduzidoDto>> MapearComunicados(IEnumerable<ComunicadoAlunoReduzidoDto> comunicados, string alunoCodigo)
        {
            var listaComunicados = new List<ComunicadoAlunoReduzidoDto>();
            if (comunicados.Count() == 0)
                return listaComunicados;

            var obterComunicados = await mediator.Send(new ObterSituacaoComunicadosEscolaAquiQuery(alunoCodigo, comunicados.Select(c => c.ComunicadoId).ToArray())); ;

            foreach (var comunicado in comunicados)
            {
                listaComunicados.Add(new ComunicadoAlunoReduzidoDto()
                {
                    DataEnvio = comunicado.DataEnvio.Date,
                    Titulo = comunicado.Titulo,
                    Categoria = comunicado.Categoria,
                    CategoriaNome = comunicado.Categoria.Name(),
                    StatusLeitura = obterComunicados.FirstOrDefault(c => c.NotificacaoId == comunicado.ComunicadoId).Situacao
                });
            }

            return listaComunicados;
        }
    }
}
