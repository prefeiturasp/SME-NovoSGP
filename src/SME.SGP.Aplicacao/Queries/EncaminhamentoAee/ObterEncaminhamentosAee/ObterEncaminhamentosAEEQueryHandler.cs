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
    public class ObterEncaminhamentosAEEQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentosAEEQuery, PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }


        public ObterEncaminhamentosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>> Handle(ObterEncaminhamentosAEEQuery request, CancellationToken cancellationToken)
        {
            var turmasCodigos = await ObterTurmasCodigos(request.UeId, request.AnoLetivo);
            var anoLetivoConsultaPap = request.TurmaId > 0 ? (await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId))).AnoLetivo : DateTimeExtension.HorarioBrasilia().Year;
            return await MapearParaDto(await repositorioEncaminhamentoAEE.ListarPaginado(request.DreId,
                                                                     request.UeId,
                                                                     request.TurmaId,
                                                                     request.AlunoCodigo,
                                                                     (int?)request.Situacao,
                                                                     request.ResponsavelRf,
                                                                     request.AnoLetivo,
                                                                     turmasCodigos,
                                                                     Paginacao,
                                                                     request.ExibirEncerrados),anoLetivoConsultaPap);
        }

        private async Task<string[]> ObterTurmasCodigos(long ueId, int anoLetivo)
        {
            if (ueId > 0)
            {
                List<string> turmasCodigos = new List<string>();
                bool ehTurmaHistorica = anoLetivo < DateTime.Now.Year;
                var tipos = new int[0];
                var ueCodigo = await mediator.Send(new ObterUePorIdQuery(ueId));
                var turmas = await mediator.Send(new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(ueCodigo.CodigoUe, 0, 0, ehTurmaHistorica, anoLetivo, tipos, true));

                if (turmas.NaoEhNulo() || turmas.Any())
                {
                    foreach (var item in turmas)
                        turmasCodigos.Add(item.Codigo);
                }

                return turmasCodigos.ToArray();
            }

            return null;
        }

        private async Task<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>> MapearParaDto(PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto> resultadoDto, int anoLetivoConsultaPap)
        {
            return new PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items,anoLetivoConsultaPap)
            };
        }
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<IEnumerable<EncaminhamentoAEEResumoDto>> MapearParaDto(IEnumerable<EncaminhamentoAEEAlunoTurmaDto> encaminhamentos,int anoLetivoConsultaPap)
        {
            var listaEncaminhamentos = new List<EncaminhamentoAEEResumoDto>();
            IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPAP = Enumerable.Empty<AlunosTurmaProgramaPapDto>();
            
            if(encaminhamentos.Any())
                matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(encaminhamentos.Select(x => x.AlunoCodigo).ToArray(), anoLetivoConsultaPap);
            
            foreach (var encaminhamento in encaminhamentos)
            {
                var retorno = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(encaminhamento.AlunoCodigo, encaminhamento.TurmaAno, false));
                var aluno = retorno.OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();                

                var ehAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, encaminhamento.TurmaAno));
                listaEncaminhamentos.Add(new EncaminhamentoAEEResumoDto()
                {
                    Id = encaminhamento.Id,
                    Situacao = encaminhamento.Situacao != 0 ? encaminhamento.Situacao.Name() : "",
                    Turma = $"{encaminhamento.TurmaModalidade.ShortName()} - {encaminhamento.TurmaNome}",
                    Numero = aluno?.NumeroAlunoChamada ?? 0,
                    Nome = aluno?.NomeAluno,
                    Responsavel = encaminhamento.Responsavel,
                    EhAtendidoAEE = ehAtendidoAEE,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno?.CodigoAluno),
                    Ue = $"{encaminhamento.TipoEscola.ShortName()} {encaminhamento.UeNome}",
                });
            }

            return listaEncaminhamentos;
        }
    }
}
