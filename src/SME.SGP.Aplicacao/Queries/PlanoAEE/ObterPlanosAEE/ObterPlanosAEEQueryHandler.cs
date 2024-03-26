using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
        private readonly IMediator mediator;

        public ObterPlanosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioPlanoAEEConsulta repositorioPlanoAEE,
                                          IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> Handle(ObterPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            bool ehAdmin = usuario.EhAdmGestao();
            bool ehPAEE = usuario.EhProfessorPaee();
            string[] turmasCodigos = null;
            long[] ues = new long[] { request.UeId };

            if (request.UeId == 0 || request.UeId == -99)
            {
                ues = await ObterUesPorUsuario(request.DreId, request.AnoLetivo, request.ConsideraHistorico, usuario);

                foreach (var ue in ues)
                {
                    var codigosTurmasPorUe = await ObterCodigosTurmas(ue, ehAdmin);

                    foreach (var codigoTurmaPorUe in codigosTurmasPorUe)
                    {
                        if (turmasCodigos is not null && turmasCodigos.Any())
                            turmasCodigos = turmasCodigos.Concat(new string[1] { codigoTurmaPorUe }).ToArray();
                        else
                            turmasCodigos = new string[1] { codigoTurmaPorUe };
                    }
                }
            }
            else
                turmasCodigos = await ObterCodigosTurmas(request.UeId, ehAdmin);

            var anoLetivoConsultaPap = request.TurmaId > 0 ? (await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId))).AnoLetivo : DateTimeExtension.HorarioBrasilia().Year;

            return await MapearParaDto(await repositorioPlanoAEE.ListarPaginado(request.DreId,
                                                                          ues,
                                                                          request.TurmaId,
                                                                          request.AlunoCodigo,
                                                                          (int?)request.Situacao,
                                                                          turmasCodigos,
                                                                          ehAdmin,
                                                                          ehPAEE,
                                                                          Paginacao,
                                                                          request.ExibirEncerrados,
                                                                          request.ResponsavelRf,
                                                                          request.PaaiReponsavelRf), anoLetivoConsultaPap);
        }
        
        private async Task<long[]> ObterUesPorUsuario(long dreId, int anoLetivo, bool consideraHistorico, Usuario usuario)
        {
            var dre = await mediator.Send(new ObterDREPorIdQuery(dreId));

            if (dre.NaoEhNulo())
            {
                var dto = new UEsPorDreDto()
                {
                    CodigoDre = dre.CodigoDre,
                    AnoLetivo = anoLetivo,
                    ConsideraHistorico = consideraHistorico
                };

                var ues = await mediator.Send(new ObterUEsPorDREQuery(dto, usuario.Login, usuario.PerfilAtual));
                if (ues.NaoEhNulo() && ues.Any())
                    return ues.Select(x => x.Id).ToArray();

                return new long[1] { 0 };
            }

            return null;
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

                if (turmas.NaoEhNulo() || turmas.Any())
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
            if(planosAEE.NaoEhNulo() && planosAEE.Any())
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
