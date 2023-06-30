using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarCacheFechamentoNotaCommandHandler : IRequestHandler<AtualizarCacheFechamentoNotaCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;

        public AtualizarCacheFechamentoNotaCommandHandler(IRepositorioCache repositorioCache, IMediator mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(AtualizarCacheFechamentoNotaCommand request, CancellationToken cancellationToken)
        {
            var notasFechamentoFinaisNoCache = await ObterFechamentosNotasDoCache(request);
            if (notasFechamentoFinaisNoCache != null)
                await PersistirNotasFinaisNoCache(notasFechamentoFinaisNoCache, request);

            var notasConceitosFechamento = await ObterNotasConceitosCache(request);
            if (notasConceitosFechamento != null)
                await PersistirNotaConceitoBimestreNoCache(notasConceitosFechamento, request);

            return true;
        }

        private async Task<List<FechamentoNotaAlunoAprovacaoDto>> ObterFechamentosNotasDoCache(AtualizarCacheFechamentoNotaCommand request)
        {
            var nomeChaveCache = ObterChaveFechamentoNotaFinalComponenteTurma(request.DisciplinaId.ToString(), request.CodigoTurma, request.CodigoAluno);

            return await repositorioCache.ObterObjetoAsync<List<FechamentoNotaAlunoAprovacaoDto>>(nomeChaveCache);
        }

        private async Task<List<NotaConceitoBimestreComponenteDto>> ObterNotasConceitosCache(AtualizarCacheFechamentoNotaCommand request)
        {
            var nomeChaveCache = ObterChaveNotaConceitoFechamentoTurmaBimestreFinal(request.CodigoTurma, request.CodigoAluno);

            return await repositorioCache.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(nomeChaveCache);
        }

        private async Task PersistirNotasFinaisNoCache(
                                List<FechamentoNotaAlunoAprovacaoDto> notasFinais,
                                AtualizarCacheFechamentoNotaCommand request)
        {
            var notaFinalAluno = notasFinais.FirstOrDefault(c => c.AlunoCodigo == request.CodigoAluno && c.ComponenteCurricularId == request.FechamentoNota.DisciplinaId && c.Bimestre is 0 or null);

            if (notaFinalAluno == null)
            {
                notasFinais.Add(new FechamentoNotaAlunoAprovacaoDto
                {
                    Bimestre = null,
                    Nota = request.FechamentoNota.Nota,
                    AlunoCodigo = request.CodigoAluno,
                    ConceitoId = request.FechamentoNota.ConceitoId,
                    EmAprovacao = request.EmAprovacao,
                    ComponenteCurricularId = request.FechamentoNota.DisciplinaId
                });
            }
            else
            {
                notaFinalAluno.Nota = request.FechamentoNota.Nota;
                notaFinalAluno.ConceitoId = request.FechamentoNota.ConceitoId;
                notaFinalAluno.EmAprovacao = request.EmAprovacao;
            }

            await SalvarCache(ObterChaveFechamentoNotaFinalComponenteTurma(request.DisciplinaId.ToString(), request.CodigoTurma, request.CodigoAluno), notasFinais);
        }

        private async Task PersistirNotaConceitoBimestreNoCache(
                                List<NotaConceitoBimestreComponenteDto> notasConceitosFechamento,
                                AtualizarCacheFechamentoNotaCommand request)
        {
            var notaConceitoFechamentoAluno = notasConceitosFechamento.FirstOrDefault(c => c.AlunoCodigo == request.CodigoAluno &&
                c.ComponenteCurricularCodigo == request.FechamentoNota.DisciplinaId && c.Bimestre is 0 or null);

            if (notaConceitoFechamentoAluno == null)
                notasConceitosFechamento.Add(ObterNotaConceitoBimestreAluno(request));
            else
            {
                notaConceitoFechamentoAluno.Nota = request.FechamentoNota.Nota;
                notaConceitoFechamentoAluno.ConceitoId = request.FechamentoNota.ConceitoId;
            }

            await SalvarCache(ObterChaveNotaConceitoFechamentoTurmaBimestreFinal(request.CodigoTurma, request.CodigoAluno), notasConceitosFechamento);
        }

        private NotaConceitoBimestreComponenteDto ObterNotaConceitoBimestreAluno(AtualizarCacheFechamentoNotaCommand request)
        {
            return new NotaConceitoBimestreComponenteDto
            {
                AlunoCodigo = request.CodigoAluno,
                Nota = request.FechamentoNota.Nota,
                ConceitoId = request.FechamentoNota.ConceitoId,
                ComponenteCurricularCodigo = request.FechamentoNota.DisciplinaId,
                TurmaCodigo = request.CodigoTurma,
                Bimestre = null,
                ConselhoClasseNotaId = request.ConselhosClasseAlunos != null ? request.ConselhosClasseAlunos.ConselhoClasseNotaId : 0,
                ConselhoClasseId = request.ConselhosClasseAlunos != null ? request.ConselhosClasseAlunos.ConselhoClasseId : 0
            };
        }

        private async Task SalvarCache(string chave, object valor)
        {
            await mediator.Send(new SalvarCachePorValorObjetoCommand(chave, valor));
        }

        private string ObterChaveFechamentoNotaFinalComponenteTurma(string codigoDisciplina, string codigoTurma, string alunoCodigo)
            => string.Format(NomeChaveCache.CHAVE_FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA, codigoDisciplina, codigoTurma, alunoCodigo);

        private string ObterChaveNotaConceitoFechamentoTurmaBimestreFinal(string codigoTurma, string alunoCodigo)
            => string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_FECHAMENTO_TURMA_ALUNO_BIMESTRES_E_FINAL, codigoTurma, alunoCodigo);
    }
}
