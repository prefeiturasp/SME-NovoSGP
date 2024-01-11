using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using static SME.SGP.Dominio.DateTimeExtension;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaConselhoClasseUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseUseCase
    {
        public ExecutarConsolidacaoTurmaConselhoClasseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurmaConselhoClasse = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurmaConselhoClasse.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma. O id da turma e o bimestre não foram informados", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }

            if (consolidacaoTurmaConselhoClasse.TurmaId == 0)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma. O id da turma não foi informado", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }
            
            var turma = await mediator
                .Send(new ObterTurmaComUeEDrePorIdQuery(consolidacaoTurmaConselhoClasse.TurmaId));

            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, DateTime.Today));

            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException($"Não foram encontrados alunos para a turma {turma.CodigoTurma} no Eol");

            var tipoCalendarioId = await mediator
                .Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            if (tipoCalendarioId == 0)
                throw new NegocioException("Não foi possível obter o tipo calendario.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares.EhNulo())
                throw new NegocioException("Não foi possivel obter o período escolar.");

            foreach (var aluno in alunos)
            {
                var ultimoBimestreAtivo = aluno.Inativo ?
                    periodosEscolares.FirstOrDefault(p => p.PeriodoInicio.Date <= aluno.DataSituacao && p.PeriodoFim.Date >= aluno.DataSituacao)?.Bimestre : 4;
                
                if (ultimoBimestreAtivo.EhNulo())
                {
                    await VerificaSeHaConsolidacaoErrada(aluno.CodigoAluno, turma.Id);
                    continue;
                }

                if (aluno.Inativo && consolidacaoTurmaConselhoClasse.Bimestre > ultimoBimestreAtivo)
                {
                    await VerificaSeHaConsolidacaoErrada(aluno.CodigoAluno, turma.Id, consolidacaoTurmaConselhoClasse.Bimestre ?? 0);
                    continue;
                }

                var matriculadoDepois = (int?)null;

                if (aluno.Ativo)
                {
                    var matriculasAlunoTurma = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(turma.CodigoTurma, aluno.CodigoAluno));

                    matriculadoDepois = (from m in matriculasAlunoTurma
                                         from p in periodosEscolares
                                         where (m.DataMatricula.Equals(DateTime.MinValue) ? aluno.DataMatricula.Date : m.DataMatricula.Date) < p.PeriodoFim.Date
                                         orderby p.Bimestre
                                         select (int?)p.Bimestre).FirstOrDefault();                    
                }

                if (matriculadoDepois.NaoEhNulo() && consolidacaoTurmaConselhoClasse.Bimestre > 0 && consolidacaoTurmaConselhoClasse.Bimestre < matriculadoDepois)
                {
                    await VerificaSeHaConsolidacaoErrada(aluno.CodigoAluno, turma.Id, consolidacaoTurmaConselhoClasse.Bimestre ?? 0);
                    continue;
                }

                await PublicarMensagem(aluno, consolidacaoTurmaConselhoClasse, 0, mensagemRabbit.CodigoCorrelacao);                 
            }

            return true;
        }

        private async Task VerificaSeHaConsolidacaoErrada(string codigoAluno, long turmaId, int bimestreVigente = 0)
        {
            var consolidacoesConselhoId = await mediator.Send(new ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery(codigoAluno, turmaId));

            if (consolidacoesConselhoId.Any())
            {
                var consolidacoesNotaIds = await mediator.Send(new ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery(consolidacoesConselhoId.ToArray(), bimestreVigente));

                if (consolidacoesNotaIds.Any())
                    await mediator.Send(new ExcluirConsolidacaoConselhoPorIdBimestreCommand(consolidacoesNotaIds.ToArray(), bimestreVigente == 0 ? consolidacoesConselhoId.ToArray() : new long[] { }));
            }

        }

        private async Task<bool> PublicarMensagem(AlunoPorTurmaResposta aluno, ConsolidacaoTurmaDto consolidacaoTurmaConselhoClasse, long codigoComponenteCurricular, Guid CodigoCorrelacao)
        {
            try
            {
                var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(aluno.CodigoAluno,
                                                                                                             consolidacaoTurmaConselhoClasse.TurmaId,
                                                                                                             consolidacaoTurmaConselhoClasse.Bimestre,
                                                                                                             aluno.Inativo,
                                                                                                             componenteCurricularId: codigoComponenteCurricular);

                var mensagemParaPublicar = JsonConvert.SerializeObject(mensagemConsolidacaoConselhoClasseAluno);

                var publicarFilaConsolidacaoConselhoClasseAluno = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemParaPublicar, CodigoCorrelacao, null));
                if (!publicarFilaConsolidacaoConselhoClasseAluno)
                {
                    var mensagem = $"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.";
                    await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, LogNivel.Critico, LogContexto.ConselhoClasse));
                    throw new NegocioException(mensagem);
                }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                throw;
            }
        }
    }
}
