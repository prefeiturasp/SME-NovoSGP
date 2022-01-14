using MediatR;
using SME.SGP.Dominio.Enumerados;
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
        public IMediator mediator { get; }
        public IRepositorioPlanoAEEConsulta repositorioPlanoAEE { get; }

        public ObterPlanosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioPlanoAEEConsulta repositorioPlanoAEE) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> Handle(ObterPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            return await MapearParaDto(await repositorioPlanoAEE.ListarPaginado(request.DreId,
                                                                     request.UeId,
                                                                     request.TurmaId,
                                                                     request.AlunoCodigo,
                                                                     (int?)request.Situacao,
                                                                     Paginacao));
        }

        private async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> MapearParaDto(PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<PlanoAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items)
            };
        }

        private async Task<IEnumerable<PlanoAEEResumoDto>> MapearParaDto(IEnumerable<PlanoAEEAlunoTurmaDto> planosAEE)
        {
            var listaPlanosAEE = new List<PlanoAEEResumoDto>();

            foreach (var planoAEE in planosAEE)
            {
                try
                {
                    listaPlanosAEE.Add(new PlanoAEEResumoDto()
                    {
                        Id = planoAEE.Id,
                        Situacao = planoAEE.Situacao != 0 ? planoAEE.Situacao.Name() : "",
                        Turma = $"{planoAEE.TurmaModalidade.ShortName()} - {planoAEE.TurmaNome}",
                        Numero = planoAEE.AlunoNumero,
                        Nome = planoAEE.AlunoNome,
                        PossuiEncaminhamentoAEE = planoAEE.PossuiEncaminhamentoAEE,
                        EhAtendidoAEE = (planoAEE.Situacao != SituacaoPlanoAEE.Encerrado && planoAEE.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamento),
                        CriadoEm = planoAEE.CriadoEm,
                        Versao = $"v{planoAEE.Versao} ({planoAEE.DataVersao:dd/MM/yyyy})"
                    });

                }
                catch (Exception e)
                {

                    throw;
                }            
            }

            return listaPlanosAEE;
        }
    }
}
