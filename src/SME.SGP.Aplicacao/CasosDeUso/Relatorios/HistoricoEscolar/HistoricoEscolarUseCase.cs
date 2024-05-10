using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class HistoricoEscolarUseCase : IHistoricoEscolarUseCase
    {
        private readonly IMediator mediator;

        public HistoricoEscolarUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroHistoricoEscolarDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroHistoricoEscolarDto.UeCodigo));
            await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(filtroHistoricoEscolarDto.TurmaCodigo));
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            filtroHistoricoEscolarDto.Usuario = usuarioLogado ?? throw new NegocioException("Não foi possível localizar o usuário.");

            if (!filtroHistoricoEscolarDto.Alunos.Any())
                await ObterAlunosNaTurmaEObservacoesHistoricoEscolarAsync(filtroHistoricoEscolarDto);

            var historicoEscolarObservacoes = filtroHistoricoEscolarDto.Alunos.Select(t => new HistoricoEscolarObservacaoDto(t.AlunoCodigo, t.ObservacaoComplementar));
            if (historicoEscolarObservacoes.Any())
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarGravarObservacaoHistorioEscolar, historicoEscolarObservacoes, Guid.NewGuid()));

            var tipoRelatorio = filtroHistoricoEscolarDto.Modalidade == Modalidade.EJA ? TipoRelatorio.HistoricoEscolarEJARazor : TipoRelatorio.HistoricoEscolarFundamentalRazor;

            return await mediator.Send(new GerarRelatorioCommand(tipoRelatorio, filtroHistoricoEscolarDto, usuarioLogado,rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosHistoricoEscolar));
        }

        private async Task ObterAlunosNaTurmaEObservacoesHistoricoEscolarAsync(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto)
        {
            var dadosAlunos = await mediator.Send(new ObterAlunosSimplesDaTurmaQuery(filtroHistoricoEscolarDto.TurmaCodigo));
            var codigosAlunos = dadosAlunos.Select(aluno => aluno.Codigo).ToArray();
            var observacoes = (await mediator.Send(new ObterObservacoesDosAlunosNoHistoricoEscolarQuery(codigosAlunos))).ToList();
            var novofiltroHistoricoEscolarDto = new List<FiltroHistoricoEscolarAlunosDto>();
            foreach (var dadoAluno in dadosAlunos)
            {
                var filtoHistoricoEscolarAluno = new FiltroHistoricoEscolarAlunosDto()
                {
                    AlunoCodigo = dadoAluno.Codigo,
                    ObservacaoComplementar = observacoes?.FirstOrDefault(x => x.AlunoCodigo == dadoAluno.Codigo)?.Observacao
                };

                novofiltroHistoricoEscolarDto.Add(filtoHistoricoEscolarAluno);
            }
            filtroHistoricoEscolarDto.Alunos = novofiltroHistoricoEscolarDto;
        }
    }
}
