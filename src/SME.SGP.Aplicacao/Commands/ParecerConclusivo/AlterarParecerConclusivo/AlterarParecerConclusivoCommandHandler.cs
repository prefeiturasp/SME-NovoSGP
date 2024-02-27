using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarParecerConclusivoCommandHandler : IRequestHandler<AlterarParecerConclusivoCommand, ParecerConclusivoDto>
    {
        private readonly IMediator mediator;

        public AlterarParecerConclusivoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ParecerConclusivoDto> Handle(AlterarParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery(
                    request.ConselhoClasseId,
                    request.FechamentoTurmaId,
                    request.AlunoCodigo));

            if (request.ParecerConclusivoId == conselhoClasseAluno.ConselhoClasseParecerId)
                return new ParecerConclusivoDto()
                {
                    Id = request.ParecerConclusivoId ?? 0,
                    EmAprovacao = false
                };

            var emAprovacao = await EnviarParaAprovacao(conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma);

            if (emAprovacao)
                await GerarWFAprovacao(conselhoClasseAluno, request.ParecerConclusivoId);
            else
                await PersistirParecer(conselhoClasseAluno, request.ParecerConclusivoId);

            return new ParecerConclusivoDto()
            {
                Id = request.ParecerConclusivoId ?? 0,
                EmAprovacao = emAprovacao
            }; 
        }

        private async Task PersistirParecer(ConselhoClasseAluno conselhoClasseAluno, long? parecerConclusivoId)
        {
            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var bimestre = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar?.Bimestre ?? null;
            var persistirParecerConclusivoDto = new PersistirParecerConclusivoDto()
            {
                ConselhoClasseAlunoId = conselhoClasseAluno.Id,
                ConselhoClasseAlunoCodigo = conselhoClasseAluno.AlunoCodigo,
                ParecerConclusivoId = parecerConclusivoId,
                TurmaId = turma.Id,
                TurmaCodigo = turma.CodigoTurma,
                Bimestre = bimestre,
                AnoLetivo = turma.AnoLetivo
            };
            await mediator.Send(new PersistirParecerConclusivoCommand(persistirParecerConclusivoDto));
        }

        private async Task GerarWFAprovacao(ConselhoClasseAluno conselhoClasseAluno, long? parecerConclusivoId)
        {
            var solicitanteId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var pareceresDaTurma = await ObterPareceresDaTurma(turma);
            var parecerAnterior = pareceresDaTurma.FirstOrDefault(a => a.Id == conselhoClasseAluno.ConselhoClasseParecerId)?.Nome;
            var parecerNovo = pareceresDaTurma.FirstOrDefault(a => a.Id == parecerConclusivoId)?.Nome;

            var pareceresEmAprovacaoAtual = await mediator.Send(new ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery(conselhoClasseAluno.Id));
            if (!pareceresEmAprovacaoAtual.Any(parecer => parecer.ConselhoClasseParecerId == parecerConclusivoId))
                await mediator.Send(new GerarWFAprovacaoParecerConclusivoCommand(conselhoClasseAluno,
                                                                                 turma,
                                                                                 parecerConclusivoId,
                                                                                 parecerAnterior,
                                                                                 parecerNovo,
                                                                                 solicitanteId));
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterPareceresDaTurma(Turma turma)
        {
            var pareceresConclusivos = await mediator.Send(new ObterPareceresConclusivosPorTurmaQuery(turma));
            if (pareceresConclusivos.EhNulo() || !pareceresConclusivos.Any())
                throw new NegocioException("Não foram encontrados pareceres conclusivos para a turma!");

            return pareceresConclusivos;
        }

        private async Task<bool> EnviarParaAprovacao(Turma turma)
        {
            return turma.AnoLetivo < DateTime.Today.Year && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoParecerConclusivo, anoLetivo));
            if (parametro.EhNulo())
                throw new NegocioException($"Não localizado parametro de aprovação de alteração de parecer conclusivo para o ano {anoLetivo}");

            return parametro.Ativo;
        }
    }
}
