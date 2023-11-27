using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dados.Repositorios;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class GravarConselhoClasseCommadHandler : IRequestHandler<GravarConselhoClasseCommad, ConselhoClasseNotaRetornoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;

        public GravarConselhoClasseCommadHandler(
                        IMediator mediator,
                        IRepositorioCache repositorioCache)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Handle(GravarConselhoClasseCommad request, CancellationToken cancellationToken)
        {
            var conselhoClasseNotaRetorno = request.ConselhoClasseId == 0 ?
                await mediator.Send(new InserirConselhoClasseNotaCommad(
                                            request.FechamentoTurma,
                                            request.CodigoAluno,
                                            request.ConselhoClasseNotaDto,
                                            request.Bimestre,
                                            request.Usuario), cancellationToken) :
                await mediator.Send(new AlterarConselhoClasseCommad(
                                            request.ConselhoClasseId,
                                            request.FechamentoTurma.Id,
                                            request.CodigoAluno,
                                            request.FechamentoTurma.Turma,
                                            request.ConselhoClasseNotaDto,
                                            request.Bimestre,
                                            request.Usuario), cancellationToken);

            var situacaoConselhoAtualizada = await mediator.Send(new AtualizaSituacaoConselhoClasseCommand(conselhoClasseNotaRetorno.ConselhoClasseId, request.FechamentoTurma.Turma.CodigoTurma), cancellationToken);
            if (!situacaoConselhoAtualizada)
                throw new NegocioException(MensagemNegocioConselhoClasse.ERRO_ATUALIZAR_SITUACAO_CONSELHO_CLASSE);

            await RemoverCache(string.Format(NomeChaveCache.NOTA_CONCEITO_FECHAMENTO_TURMA_ALUNO_BIMESTRES_E_FINAL, request.FechamentoTurma.Turma.CodigoTurma, request.CodigoAluno), cancellationToken);
            await RemoverCache(string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO, request.FechamentoTurma.Turma.CodigoTurma, request.Bimestre, request.CodigoAluno), cancellationToken);
            await AtualizarCache(request.ConselhoClasseNotaDto, request.FechamentoTurma.Turma, request.FechamentoTurma, request.CodigoAluno, request.Bimestre);

            return await Task.FromResult(conselhoClasseNotaRetorno);
        }

        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }

        private async Task AtualizarCache(ConselhoClasseNotaDto conselhoClasseNota, Turma turma, FechamentoTurma fechamentoTurma, string codigoAluno, int? bimestre)
        {
            var nomeChaveCache = ObterChaveNotaConceitoConselhoClasseTurmaBimestre(turma.CodigoTurma,(int)Bimestre.Final,codigoAluno);

            var notasConceitosFechamento = await repositorioCache.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(nomeChaveCache);
            if (notasConceitosFechamento.NaoEhNulo())
                await PersistirNotaConceitoConselhoClasseBimestreNoCache(notasConceitosFechamento, conselhoClasseNota, codigoAluno, turma.CodigoTurma, fechamentoTurma, bimestre);  
        }
        private static string ObterChaveNotaConceitoConselhoClasseTurmaBimestre(string codigoTurma, int bimestre, string alunoCodigo)
        {
            return string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO,codigoTurma, bimestre,alunoCodigo);
        }

        private async Task PersistirNotaConceitoConselhoClasseBimestreNoCache(List<NotaConceitoBimestreComponenteDto> notasConceitosFechamento,
          ConselhoClasseNotaDto conselhoClasseNota, string codigoAluno, string codigoTurma, FechamentoTurma fechamentoTurma, int? bimestre)
        {
            var notaConceitoFechamentoAluno = notasConceitosFechamento.FirstOrDefault(c => c.AlunoCodigo == codigoAluno &&
                c.ComponenteCurricularCodigo == conselhoClasseNota.CodigoComponenteCurricular && c.Bimestre == bimestre);

            if (notaConceitoFechamentoAluno.EhNulo())
                notasConceitosFechamento.Add( await ObterNotaConceitoBimestreAluno(codigoAluno, conselhoClasseNota.CodigoComponenteCurricular, codigoTurma, conselhoClasseNota, fechamentoTurma, bimestre));
            else
            {
                notaConceitoFechamentoAluno.Nota = conselhoClasseNota.Nota;
                notaConceitoFechamentoAluno.ConceitoId = conselhoClasseNota.Conceito;
            }

            await mediator.Send(new SalvarCachePorValorObjetoCommand(ObterChaveNotaConceitoConselhoClasseTurmaBimestre(codigoTurma,(int)Bimestre.Final,codigoAluno), notasConceitosFechamento));
        }

        private async Task<NotaConceitoBimestreComponenteDto> ObterNotaConceitoBimestreAluno(string codigoAluno,
                                                                                 long codigoDisciplina,
                                                                                 string codigoTurma,
                                                                                 ConselhoClasseNotaDto conselhoClasseNota,
                                                                                 FechamentoTurma fechamentoTurma,
                                                                                 int? bimestre)
        {
           var conselhosClasseAlunos = await mediator.Send(new ObterConselhoClasseAlunosNotaPorFechamentoIdQuery(fechamentoTurma.Id));
            var conselho = conselhosClasseAlunos.ToList().Find(ca => ca.AlunoCodigo == codigoAluno &&
                                                               ca.ComponenteCurricularCodigo == codigoDisciplina);
            return new NotaConceitoBimestreComponenteDto
            {
                AlunoCodigo = codigoAluno,
                Nota = conselhoClasseNota.Nota,
                ConceitoId = conselhoClasseNota.Conceito,
                ComponenteCurricularCodigo = codigoDisciplina,
                TurmaCodigo = codigoTurma,
                Bimestre = bimestre,
                ConselhoClasseNotaId = conselho.NaoEhNulo() ? conselho.ConselhoClasseNotaId : 0,
                ConselhoClasseId = conselho.NaoEhNulo() ? conselho.ConselhoClasseId : 0
            };
        }

    }
}
