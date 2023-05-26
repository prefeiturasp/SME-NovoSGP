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
            
            return await MapearParaDto(await repositorioEncaminhamentoAEE.ListarPaginado(request.DreId,
                                                                     request.UeId,
                                                                     request.TurmaId,
                                                                     request.AlunoCodigo,
                                                                     (int?)request.Situacao,
                                                                     request.ResponsavelRf,
                                                                     request.AnoLetivo,
                                                                     turmasCodigos,
                                                                     Paginacao,
                                                                     request.ExibirEncerrados));
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

                if (turmas != null || turmas.Any())
                {
                    foreach (var item in turmas)
                        turmasCodigos.Add(item.Codigo);
                }

                return turmasCodigos.ToArray();
            }

            return null;
        }

        private async Task<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>> MapearParaDto(PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items)
            };
        }

        private async Task<IEnumerable<EncaminhamentoAEEResumoDto>> MapearParaDto(IEnumerable<EncaminhamentoAEEAlunoTurmaDto> encaminhamentos)
        {
            var listaEncaminhamentos = new List<EncaminhamentoAEEResumoDto>();

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
                    Ue = $"{encaminhamento.TipoEscola.ShortName()} {encaminhamento.UeNome}",
                });
            }

            return listaEncaminhamentos;
        }
    }
}
