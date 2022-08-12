using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEQueryHandler : ConsultasBase, IRequestHandler<ObterPlanosAEEQuery, PaginacaoResultadoDto<PlanoAEEResumoDto>>
    {        
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterPlanosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioPlanoAEEConsulta repositorioPlanoAEE) : base(contextoAplicacao)
        {            
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> Handle(ObterPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            return MapearParaDto(await repositorioPlanoAEE.ListarPaginado(request.DreId,
                                                                          request.UeId,
                                                                          request.TurmaId,
                                                                          request.AlunoCodigo,
                                                                          (int?)request.Situacao,
                                                                          Paginacao));
        }

        private PaginacaoResultadoDto<PlanoAEEResumoDto> MapearParaDto(PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<PlanoAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = MapearParaDto(resultadoDto.Items)
            };
        }

        private IEnumerable<PlanoAEEResumoDto> MapearParaDto(IEnumerable<PlanoAEEAlunoTurmaDto> planosAEE)
        {
            foreach (var planoAEE in planosAEE)
            {
                yield return new PlanoAEEResumoDto()
                {
                    Id = planoAEE.Id,
                    Situacao = planoAEE.SituacaoPlano(),
                    Turma = planoAEE.NomeTurmaFormatado(),
                    Numero = planoAEE.AlunoNumero,
                    Nome = planoAEE.AlunoNome,
                    PossuiEncaminhamentoAEE = planoAEE.PossuiEncaminhamentoAEE,
                    EhAtendidoAEE = planoAEE.EhAtendidoAEE(),
                    CriadoEm = planoAEE.CriadoEm,
                    Versao = planoAEE.ObterVersaoPlano()
                };
            }
        }
    }
}
