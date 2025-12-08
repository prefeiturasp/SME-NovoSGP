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
    public class ObterAtendimentosNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentosNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>>
    {
        public IMediator mediator { get; }
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }


        public ObterAtendimentosNAAPAQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>> Handle(ObterAtendimentosNAAPAQuery request, CancellationToken cancellationToken)
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
                request.CodigoUe,request.CodigoNomeAluno, request.DataAberturaQueixaInicio, request.DataAberturaQueixaFim, request.Situacao, 
                turmasIds.ToArray(), Paginacao, request.ExibirEncerrados, request.Ordenacao),request.AnoLetivo);
        }

        private async Task<PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>> MapearParaDto(PaginacaoResultadoDto<AtendimentoNAAPAResumoDto> resultadoDto,int anoLetivo)
        {
            return new PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>()
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
        private async Task<IEnumerable<AtendimentoNAAPAResumoDto>> MapearParaDto(IEnumerable<AtendimentoNAAPAResumoDto> encaminhamentos, int anoLetivo)
        {
            var listaEncaminhamentos = new List<AtendimentoNAAPAResumoDto>();
            IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPAP = Enumerable.Empty<AlunosTurmaProgramaPapDto>();
            
            if(encaminhamentos.Any())
                matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(encaminhamentos.Select(x => x.CodigoAluno).ToArray(), anoLetivo);
            
            foreach (var encaminhamento in encaminhamentos)
            {
                listaEncaminhamentos.Add(new AtendimentoNAAPAResumoDto()
                {
                    Id = encaminhamento.Id,
                    Ue = $"{encaminhamento.TipoEscola.ShortName()} {encaminhamento.UeNome}",
                    UeNome = encaminhamento.UeNome,
                    TipoEscola = encaminhamento.TipoEscola,
                    CodigoAluno = encaminhamento.CodigoAluno,
                    NomeAluno = encaminhamento.NomeAluno,
                    SuspeitaViolencia = encaminhamento.SuspeitaViolencia,
                    Situacao = ((SituacaoNAAPA)int.Parse(encaminhamento.Situacao)).Name(),
                    DataAberturaQueixaInicio = encaminhamento.DataAberturaQueixaInicio,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == encaminhamento.CodigoAluno),
                    Turma = $"{encaminhamento.TurmaModalidade.ObterNomeCurto()}-{encaminhamento.TurmaNome}",
                    TurmaNome = encaminhamento.TurmaNome,
                    TurmaModalidade = encaminhamento.TurmaModalidade,
                    DataUltimoAtendimento = encaminhamento.DataUltimoAtendimento,
            });
            }

            return listaEncaminhamentos;
        }
    }
}
