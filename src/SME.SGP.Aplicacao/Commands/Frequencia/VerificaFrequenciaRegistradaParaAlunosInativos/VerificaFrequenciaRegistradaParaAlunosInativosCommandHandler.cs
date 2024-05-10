using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaFrequenciaRegistradaParaAlunosInativosCommandHandler : IRequestHandler<VerificaFrequenciaRegistradaParaAlunosInativosCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaFrequenciaRegistradaParaAlunosInativosCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(VerificaFrequenciaRegistradaParaAlunosInativosCommand request, CancellationToken cancellationToken)
        {
            var frequenciaAlunoParaRecalcular = new List<FrequenciaAulaARecalcularDto>();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = request.TurmaCodigo });
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaCodigo, true));
            var periodosEscolaresTurma = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            foreach (var aluno in alunosDaTurma)
            {
                var registroFrequenciaAluno = await mediator.Send(new ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery(request.TurmaCodigo, aluno.CodigoAluno));
                if (registroFrequenciaAluno.Any())
                {
                    DateTime? dataReferencia = ObterDataReferencia(aluno, turma);
                    if (dataReferencia.NaoEhNulo())
                        frequenciaAlunoParaRecalcular.AddRange(await ExcluirRegistroFrequenciaAluno(registroFrequenciaAluno,
                                                                                                    dataReferencia.Value,
                                                                                                    aluno.CodigoAluno, turma.CodigoTurma,
                                                                                                    periodosEscolaresTurma));
                }
            }
            if (frequenciaAlunoParaRecalcular.NaoPossuiRegistros())
                return false;

            try
            {
                frequenciaAlunoParaRecalcular = frequenciaAlunoParaRecalcular.DistinctBy(f => f.AulaId).ToList();
                await RecalcularFrequenciaTurma(frequenciaAlunoParaRecalcular, request.TurmaCodigo);

                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao recalcular frequência da turma de código {request.TurmaCodigo}", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                return false;
            }
            
        }

        private async Task<IEnumerable<FrequenciaAulaARecalcularDto>> ExcluirRegistroFrequenciaAluno(IEnumerable<FrequenciaAlunoTurmaDto> registroFrequenciaAluno, 
                                                                                                     DateTime dataReferencia, string codigoAluno, string codigoTurma,
                                                                                                     IEnumerable<PeriodoEscolar> periodosEscolaresTurma)
        {
            var registroFrequenciasAExcluir = registroFrequenciaAluno.Where(f => f.DataAula.Date > dataReferencia.Date).Select(r => r.RegistroFrequenciaAlunoId);
            if (registroFrequenciasAExcluir.Any())
            {
                bool excluido = await mediator.Send(new ExcluirRegistroFrequenciaAlunoPorIdCommand(registroFrequenciasAExcluir.ToArray()));
                if (excluido)
                {
                    var registrosFrequenciaValidos = registroFrequenciaAluno.Where(r => !registroFrequenciasAExcluir.Any(rf => rf == r.RegistroFrequenciaAlunoId));
                    await VerificaCompensacoesDeAlunosInativos(registrosFrequenciaValidos, periodosEscolaresTurma, codigoAluno, codigoTurma);

                    return registroFrequenciaAluno
                        .Where(f => f.DataAula.Date > dataReferencia.Date)
                        .Select(r => new FrequenciaAulaARecalcularDto()
                        {
                            AulaId = r.AulaId,
                            DisciplinaCodigo = r.DisciplinaCodigo
                        });
                }
            }
            return Enumerable.Empty<FrequenciaAulaARecalcularDto>();
        }

        private async Task RecalcularFrequenciaTurma(IEnumerable<FrequenciaAulaARecalcularDto> frequenciasAlunoRecalcular, string turmaCodigo)
        {
            foreach (var frequencia in frequenciasAlunoRecalcular)
                await mediator.Send(new RecalcularFrequenciaPorTurmaCommand(turmaCodigo, frequencia.DisciplinaCodigo, frequencia.AulaId));
        }

        private DateTime? ObterDataReferencia(AlunoPorTurmaResposta aluno, Turma turma) =>
            !aluno.PossuiSituacaoAtiva() && aluno.DataSituacao.Year == turma.AnoLetivo ? aluno.DataSituacao : null;

        public async Task VerificaCompensacoesDeAlunosInativos(IEnumerable<FrequenciaAlunoTurmaDto> frequenciasAluno, IEnumerable<PeriodoEscolar> periodosEscolares, string codigoAluno, string turmaCodigo)
        {
            foreach(var frequencias in frequenciasAluno.GroupBy(f=> f.DisciplinaCodigo))
            {
                foreach (var periodo in periodosEscolares)
                {
                    var quantidade = frequencias.Count(f => f.DataAula >= periodo.PeriodoInicio && f.DataAula <= periodo.PeriodoFim && f.Valor == (int)TipoFrequencia.F);
                    var compensacaoAluno = await mediator.Send(new ObterCompensacoesPorAlunoETurmaQuery(periodo.Bimestre, codigoAluno, frequencias.Key, turmaCodigo));

                    if(compensacaoAluno.NaoEhNulo())
                    {
                        if (compensacaoAluno.Quantidade > quantidade && quantidade > 0)
                            await mediator.Send(new AlterarTotalCompensacoesPorCompensacaoAlunoIdCommand() { CompensacaoAlunoId = compensacaoAluno.CompensacaoAlunoId, Quantidade = quantidade });
                        else if (compensacaoAluno.Quantidade > 0 && quantidade == 0)
                            await mediator.Send(new ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommand() { CompensacaoAlunoId = compensacaoAluno.CompensacaoAlunoId });
                    }     
                }
            }
        }
    }
}
