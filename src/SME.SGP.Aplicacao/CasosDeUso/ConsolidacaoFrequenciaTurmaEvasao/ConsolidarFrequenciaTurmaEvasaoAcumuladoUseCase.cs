using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase : AbstractUseCase, IConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase
    {
        public readonly IUnitOfWork unitOfWork;

        public ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado>();
            int quantidadeAlunosAbaixo50Porcento;
            int quantidadeAlunox0Porcento;
            var dataConsulta = filtro.Ano == DateTime.Now.Year ? DateTime.Today : new DateTime(filtro.Ano, 12, 31);

            if (filtro.TurmaId == 0)
            {
                var dres = await mediator.Send(ObterIdsDresQuery.Instance);

                foreach (var dre in dres)
                {
                    var ues = await mediator.Send(new ObterUEsIdsPorDreQuery(dre));                    

                    foreach (var ue in ues)
                    {
                        var turmas = await mediator.Send(new ObterTurmasComModalidadePorAnoUEQuery(filtro.Ano, ue));

                        foreach (var turma in turmas)
                        {                            
                            quantidadeAlunosAbaixo50Porcento = 0;
                            quantidadeAlunox0Porcento = 0;

                            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.TurmaCodigo, dataConsulta));
                            alunos = alunos.Where(s => s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo ||
                                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Rematriculado ||
                                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.PendenteRematricula ||
                                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.SemContinuidade ||
                                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
                            if (alunos.EhNulo() || !alunos.Any())
                                continue;

                            var alunosFrequencia = await mediator.Send(new ObterFrequenciaGeralPorAlunosTurmaQuery(alunos.Select(a => a.CodigoAluno).ToArray(), turma.TurmaCodigo));

                            quantidadeAlunosAbaixo50Porcento = alunosFrequencia.Count(a => a.PercentualFrequencia < 50);
                            quantidadeAlunox0Porcento = alunosFrequencia.Count(a => a.PercentualFrequencia == 0);                           

                            unitOfWork.IniciarTransacao();
                            try
                            {
                                await mediator.Send(new LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand(new long[] { turma.TurmaId }, new int[] { 0 }));
                                await RegistrarFrequenciaTurmaEvasao(turma.TurmaId, 0, quantidadeAlunosAbaixo50Porcento, quantidadeAlunox0Porcento);

                                unitOfWork.PersistirTransacao();
                            }
                            catch
                            {
                                unitOfWork.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            else
            {
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

                var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, dataConsulta));
                alunos = alunos.Where(s => s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo ||
                                        s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Rematriculado ||
                                        s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.PendenteRematricula ||
                                        s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.SemContinuidade ||
                                        s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
                if (alunos.EhNulo() || !alunos.Any())
                    throw new NegocioException($"Não foram encontrados alunos para a turma {turma.CodigoTurma} no Eol");

                var alunosFrequencia = await mediator.Send(new ObterFrequenciaGeralPorAlunosTurmaQuery(alunos.Select(a => a.CodigoAluno).ToArray(), turma.CodigoTurma));

                quantidadeAlunosAbaixo50Porcento = alunosFrequencia.Count(a => a.PercentualFrequencia < 50);
                quantidadeAlunox0Porcento = alunosFrequencia.Count(a => a.PercentualFrequencia == 0);
               
                unitOfWork.IniciarTransacao();
                try
                {
                    await mediator.Send(new LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand(new long[] { filtro.TurmaId }, new int[] { 0 }));
                    await RegistrarFrequenciaTurmaEvasao(filtro.TurmaId, 0, quantidadeAlunosAbaixo50Porcento, quantidadeAlunox0Porcento);

                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return true;
        }

        private async Task RegistrarFrequenciaTurmaEvasao(long turmaId, int mes, int quantidadeAlunosAbaixo50Porcento, int quantidadeAlunos0Porcento)
        {
            await mediator.Send(new RegistrarFrequenciaTurmaEvasaoCommand(turmaId, mes, quantidadeAlunosAbaixo50Porcento, quantidadeAlunos0Porcento));
        }
    }
}
