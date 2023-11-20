using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAcaoCriancaEstudanteAusenteQueryHandler : ConsultasBase, IRequestHandler<ObterRegistrosAcaoCriancaEstudanteAusenteQuery, PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao { get; }


        public ObterRegistrosAcaoCriancaEstudanteAusenteQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroAcao = repositorioRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcao));
        }

        public async Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>> Handle(ObterRegistrosAcaoCriancaEstudanteAusenteQuery request, CancellationToken cancellationToken)
        {
            return MapearParaDto(await repositorioRegistroAcao.ListarPaginadoCriancasEstudantesAusentes(request.CodigoAluno, request.TurmaId, Paginacao));
        }

        private PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto> MapearParaDto(PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = MapearParaDto(resultadoDto.Items)
            };
        }
        private IEnumerable<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto> MapearParaDto(IEnumerable<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto> registrosAcao)
            => registrosAcao.Select(ra => new RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto()
            {
                Id = ra.Id,
                DataRegistro = ra.DataRegistro,
                Usuario = ra.Usuario
            });
    }
}
