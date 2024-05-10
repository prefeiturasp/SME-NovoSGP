using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmaEvasaoUseCase : AbstractUseCase, IConsolidarFrequenciaTurmaEvasaoUseCase
    {
        public readonly IUnitOfWork unitOfWork;

        public ConsolidarFrequenciaTurmaEvasaoUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurmaEvasao>();            
            var quantidadeAlunosAbaixo50Porcento = 0;
            var quantidadeAlunox0Porcento = 0;
            var consolidacoesFrequenciaAlunoMensal = await mediator.Send(new ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery(filtro.TurmaId, filtro.Mes));
            consolidacoesFrequenciaAlunoMensal = consolidacoesFrequenciaAlunoMensal.DistinctBy(c => c.AlunoCodigo);

            foreach (var consolidacao in consolidacoesFrequenciaAlunoMensal)
            {
                if (consolidacao.Percentual < 50 && consolidacao.Percentual > decimal.Zero)
                {
                    quantidadeAlunosAbaixo50Porcento++;
                }
            }

            foreach (var consolidacao in consolidacoesFrequenciaAlunoMensal)
            {
                if (consolidacao.Percentual == decimal.Zero)
                {
                    quantidadeAlunox0Porcento++;
                }
            }

            unitOfWork.IniciarTransacao();
            try
            {
                await mediator.Send(new LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand(new long[] { filtro.TurmaId }, new int[] { filtro.Mes }));
                await mediator.Send(new LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand(new long[] { filtro.TurmaId }, new int[] { filtro.Mes }));
                var id = await RegistrarFrequenciaTurmaEvasao(filtro.TurmaId, filtro.Mes, quantidadeAlunosAbaixo50Porcento, quantidadeAlunox0Porcento);
                await RegistrarFrequenciaTurmaEvasaoAluno(id, await ObterAlunosFrequenciaInferior50(filtro.TurmaId, consolidacoesFrequenciaAlunoMensal));
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return true;
        }

        private async Task<IEnumerable<(string AlunoCodigo,
                             string AlunoNome,
                             double PercentualFrequencia)>> ObterAlunosFrequenciaInferior50(long idTurma, IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto> frequenciasMensais)
        {
            var retorno = new List<(string AlunoCodigo, string AlunoNome, double PercentualFrequencia)>();
            var codigoTurma = await mediator.Send(new ObterTurmaCodigoPorIdQuery(idTurma));
            var alunosTurma = await mediator.Send(new ObterAlunosEolPorTurmaQuery(codigoTurma, true));
            foreach (var frequencia in frequenciasMensais.Where(a => a.Percentual < 50))
            {
                var aluno = alunosTurma.FirstOrDefault(a => a.CodigoAluno.Equals(frequencia.AlunoCodigo));
                retorno.Add((aluno?.CodigoAluno, aluno?.NomeAluno, (double)frequencia.Percentual));
            }
            return retorno;
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
