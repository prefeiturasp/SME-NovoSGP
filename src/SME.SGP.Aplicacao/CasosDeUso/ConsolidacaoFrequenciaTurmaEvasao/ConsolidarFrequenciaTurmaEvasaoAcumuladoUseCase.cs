using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
                            await GerarFrequenciaTurmaEvasao(turma.TurmaCodigo, turma.TurmaId, dataConsulta);
                    }
                }
            }
            else
            {
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));
                await GerarFrequenciaTurmaEvasao(turma.CodigoTurma, turma.Id, dataConsulta);
            }
            return true;
        }

        private IEnumerable<(string AlunoCodigo, 
                             string AlunoNome, 
                             double PercentualFrequencia)> ObterAlunosFrequenciaInferior50(IEnumerable<AlunoPorTurmaResposta> alunos, 
                                                                                           IEnumerable<FrequenciaAluno> frequencias)
        {
            foreach (var frequencia in frequencias.Where(a => a.PercentualFrequencia < 50))
            {
                var aluno = alunos.FirstOrDefault(a => a.CodigoAluno.Equals(frequencia.CodigoAluno));
                yield return (aluno?.CodigoAluno, aluno?.NomeAluno, frequencia.PercentualFrequencia);
            }
        }

        private async Task GerarFrequenciaTurmaEvasao(string codigoTurma, long idTurma, DateTime dataConsulta)
        {
            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(codigoTurma, dataConsulta));
            alunos = alunos.Where(s => s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Rematriculado ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.PendenteRematricula ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.SemContinuidade ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException($"Não foram encontrados alunos para a turma {codigoTurma} no Eol");

            var alunosFrequencia = await mediator.Send(new ObterFrequenciaGeralPorAlunosTurmaQuery(alunos.Select(a => a.CodigoAluno).ToArray(), codigoTurma));

            int quantidadeAlunosAbaixo50Porcento = alunosFrequencia.Count(a => a.PercentualFrequencia < 50);
            int quantidadeAlunox0Porcento = alunosFrequencia.Count(a => a.PercentualFrequencia == 0);

            unitOfWork.IniciarTransacao();
            try
            {
                await mediator.Send(new LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand(new long[] { idTurma }, new int[] { 0 }));
                await mediator.Send(new LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand(new long[] { idTurma }, new int[] { 0 }));
                var id = await RegistrarFrequenciaTurmaEvasao(idTurma, 0, quantidadeAlunosAbaixo50Porcento, quantidadeAlunox0Porcento);
                await RegistrarFrequenciaTurmaEvasaoAluno(id, ObterAlunosFrequenciaInferior50(alunos, alunosFrequencia));
                unitOfWork.PersistirTransacao();
            }
            catch 
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task<long> RegistrarFrequenciaTurmaEvasao(long turmaId, int mes, int quantidadeAlunosAbaixo50Porcento, int quantidadeAlunos0Porcento)
        {
            return await mediator.Send(new RegistrarFrequenciaTurmaEvasaoCommand(turmaId, mes, quantidadeAlunosAbaixo50Porcento, quantidadeAlunos0Porcento));
        }

        private async Task RegistrarFrequenciaTurmaEvasaoAluno(long frequenciaTurmaEvasaoId, IEnumerable<(string AlunoCodigo, string AlunoNome, double PercentualFrequencia)> frequenciasAlunos)
        {
            foreach (var freqAluno in frequenciasAlunos)
                await mediator.Send(new RegistrarFrequenciaTurmaEvasaoAlunoCommand(frequenciaTurmaEvasaoId, freqAluno.AlunoCodigo, freqAluno.AlunoNome, freqAluno.PercentualFrequencia));
        }
    }
}
