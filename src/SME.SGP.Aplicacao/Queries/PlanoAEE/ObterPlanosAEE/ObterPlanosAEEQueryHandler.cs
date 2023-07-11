using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEQueryHandler : ConsultasBase, IRequestHandler<ObterPlanosAEEQuery, PaginacaoResultadoDto<PlanoAEEResumoDto>>
    {        
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IMediator mediator;

        public ObterPlanosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioPlanoAEEConsulta repositorioPlanoAEE, 
                                          IConsultasAbrangencia consultasAbrangencia, IMediator mediator) : base(contextoAplicacao)
        {            
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> Handle(ObterPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            bool ehAdmin = usuario.EhAdmGestao();
            bool ehPAEE = usuario.EhProfessorPaee();
            var turmasCodigos = await ObterCodigosTurmas(request.UeId, ehAdmin);

            var anoLetivoConsultaPap = request.TurmaId > 0 ? (await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId))).AnoLetivo : DateTimeExtension.HorarioBrasilia().Year;
            
            return await MapearParaDto(await repositorioPlanoAEE.ListarPaginado(request.DreId,
                                                                          request.UeId,
                                                                          request.TurmaId,
                                                                          request.AlunoCodigo,
                                                                          (int?)request.Situacao,
                                                                          turmasCodigos,
                                                                          ehAdmin,
                                                                          ehPAEE,
                                                                          Paginacao,
                                                                          request.ExibirEncerrados,
                                                                          request.ResponsavelRf,
                                                                          request.PaaiReponsavelRf),anoLetivoConsultaPap);
        }
        
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }

        private async Task<string[]> ObterCodigosTurmas(long ueId, bool ehAdmin)
        {
            if (ueId > 0 && !ehAdmin)
            {
                int[] tipos = new int[0];
                List<string> turmasCodigos = new List<string>();

                var ueCodigo = await mediator.Send(new ObterUePorIdQuery(ueId));
                var turmas =
                    await mediator.Send(
                        new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(ueCodigo.CodigoUe, 0,
                            0, false, DateTime.Now.Year, tipos, true));

                if (turmas != null || turmas.Any())
                {
                    foreach (var item in turmas)
                        turmasCodigos.Add(item.Codigo);
                }

                return turmasCodigos.ToArray();
            }

            return null;
        }

        private async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> MapearParaDto(PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto> resultadoDto, int anoLetivoConsultaPap)
        {
            return new PaginacaoResultadoDto<PlanoAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items,anoLetivoConsultaPap)
            };
        }

        private async Task<IEnumerable<PlanoAEEResumoDto>> MapearParaDto(IEnumerable<PlanoAEEAlunoTurmaDto> planosAEE, int anoLetivoConsultaPap)
        {
            var retorno = new List<PlanoAEEResumoDto>();
            IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPAP = new List<AlunosTurmaProgramaPapDto>();
            if(planosAEE != null && planosAEE.Any())
                matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(planosAEE.Select(x => x.AlunoCodigo).ToArray(), anoLetivoConsultaPap);
            
            foreach (var planoAEE in planosAEE)
            {
                retorno.Add(new PlanoAEEResumoDto()
                {
                    Id = planoAEE.Id,
                    Situacao = planoAEE.SituacaoPlano(),
                    Turma = planoAEE.NomeTurmaFormatado(),
                    Numero = planoAEE.AlunoNumero,
                    Nome = planoAEE.AlunoNome,
                    PossuiEncaminhamentoAEE = planoAEE.PossuiEncaminhamentoAEE,
                    EhAtendidoAEE = planoAEE.EhAtendidoAEE(),
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == planoAEE.AlunoCodigo),
                    CriadoEm = planoAEE.CriadoEm,
                    Versao = planoAEE.ObterVersaoPlano(),
                    RfReponsavel = planoAEE.RfReponsavel,
                    NomeReponsavel = planoAEE.NomeReponsavel,
                    RfPaaiReponsavel = planoAEE.RfPaaiReponsavel,
                    NomePaaiReponsavel = planoAEE.NomePaaiReponsavel,
                    PlanoAeeVersaoId = planoAEE.PlanoAeeVersaoId,
                    Ue = $"{planoAEE.TipoEscola.ShortName()} {planoAEE.UeNome}",
                });
            }

            return retorno;
        }
    }
}
