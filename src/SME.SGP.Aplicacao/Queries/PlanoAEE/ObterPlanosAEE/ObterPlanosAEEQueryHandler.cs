﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEQueryHandler : ConsultasBase, IRequestHandler<ObterPlanosAEEQuery, PaginacaoResultadoDto<PlanoAEEResumoDto>>
    {        
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IMediator mediator;

        public ObterPlanosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioPlanoAEEConsulta repositorioPlanoAEE, IConsultasAbrangencia consultasAbrangencia, IMediator mediator) : base(contextoAplicacao)
        {            
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> Handle(ObterPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            int periodo = 0;
            int[] tipos = new int[0];
            List<string> turmasCodigos = new List<string>();

            var ueCodigo = await mediator.Send(new ObterUePorIdQuery(request.UeId));
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            bool ehAdmin = usuario.EhAdmGestao();

            
            if (!ehAdmin){
                var turmas =
                    await mediator.Send(
                        new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(ueCodigo.CodigoUe, 0,
                            periodo, false, DateTime.Now.Year, tipos, true));

                if (turmas != null || turmas.Any())
                {
                    foreach (var item in turmas)
                        turmasCodigos.Add(item.Codigo);
                }
            }

            return MapearParaDto(await repositorioPlanoAEE.ListarPaginado(request.DreId,
                                                                          request.UeId,
                                                                          request.TurmaId,
                                                                          request.AlunoCodigo,
                                                                          (int?)request.Situacao,
                                                                          turmasCodigos.ToArray(),
                                                                          ehAdmin,
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
                    Versao = planoAEE.ObterVersaoPlano(),
                    RfReponsavel = planoAEE.RfReponsavel,
                    NomeReponsavel = planoAEE.NomeReponsavel,
                    RfPaaiReponsavel = planoAEE.RfPaaiReponsavel,
                    NomePaaiReponsavel = planoAEE.NomePaaiReponsavel 
                };
            }
        }
    }
}
