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
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentosNAAPAQuery, PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }


        public ObterEncaminhamentosNAAPAQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> Handle(ObterEncaminhamentosNAAPAQuery request, CancellationToken cancellationToken)
        {
            var turmas = Enumerable.Empty<AbrangenciaTurmaRetorno>();

            if (!string.IsNullOrEmpty(request.CodigoUe))
            {
                if (request.TurmaId > 0)
                    turmas = new List<AbrangenciaTurmaRetorno>() { new () { Id = request.TurmaId }};
                else
                    turmas = await mediator.Send(new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(request.CodigoUe,
                        0, 0, request.ExibirHistorico, request.AnoLetivo, null, true));
            }
            
            var turmasIds = turmas.NaoEhNulo() || turmas.Any() ? turmas.Select(s => s.Id) : null;

            return await MapearParaDto(await repositorioEncaminhamentoNAAPA.ListarPaginado(request.AnoLetivo, request.DreId, 
                request.CodigoUe,request.NomeAluno, request.DataAberturaQueixaInicio, request.DataAberturaQueixaFim, request.Situacao, 
                request.Prioridade, turmasIds.ToArray(), Paginacao, request.ExibirEncerrados),request.AnoLetivo);
        }

        private async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> MapearParaDto(PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto> resultadoDto,int anoLetivo)
        {
            return new PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items, anoLetivo)
            };
        }
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<IEnumerable<EncaminhamentoNAAPAResumoDto>> MapearParaDto(IEnumerable<EncaminhamentoNAAPAResumoDto> encaminhamentos, int anoLetivo)
        {
            var listaEncaminhamentos = new List<EncaminhamentoNAAPAResumoDto>();
            IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPAP = Enumerable.Empty<AlunosTurmaProgramaPapDto>();
            
            if(encaminhamentos.Any())
                matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(encaminhamentos.Select(x => x.CodigoAluno).ToArray(), anoLetivo);
            
            foreach (var encaminhamento in encaminhamentos)
            {
                listaEncaminhamentos.Add(new EncaminhamentoNAAPAResumoDto()
                {
                    Id = encaminhamento.Id,
                    Ue = $"{encaminhamento.TipoEscola.ShortName()} {encaminhamento.UeNome}",
                    UeNome = encaminhamento.UeNome,
                    TipoEscola = encaminhamento.TipoEscola,
                    CodigoAluno = encaminhamento.CodigoAluno,
                    NomeAluno = encaminhamento.NomeAluno,
                    Prioridade = encaminhamento.Prioridade,
                    Situacao = ((SituacaoNAAPA)int.Parse(encaminhamento.Situacao)).Name(),
                    DataAberturaQueixaInicio = encaminhamento.DataAberturaQueixaInicio,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == encaminhamento.CodigoAluno)
                });
            }

            return listaEncaminhamentos;
        }
    }
}
