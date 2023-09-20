using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = request.TurmaCodigo });
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaCodigo, true));
            var periodosEscolaresTurma = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(dadosTurma.ModalidadeCodigo, dadosTurma.AnoLetivo, dadosTurma.Semestre));

            foreach (var aluno in alunosDaTurma)
            {
                var registroFrequenciaAluno = await mediator.Send(new ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery(request.TurmaCodigo, aluno.CodigoAluno));

                if (registroFrequenciaAluno.Any())
                {
                    DateTime? dataReferencia = !aluno.PossuiSituacaoAtiva() && aluno.DataSituacao.Year == dadosTurma.AnoLetivo ? aluno.DataSituacao : null;

                    if(dataReferencia.NaoEhNulo())
                    {
                        var registroFrequenciasAExcluir = registroFrequenciaAluno.Where(f => f.DataAula.Date > dataReferencia.Value.Date).Select(r => r.RegistroFrequenciaAlunoId);

                        if (registroFrequenciasAExcluir.Any())
                        {
                            bool excluido = await mediator.Send(new ExcluirRegistroFrequenciaAlunoPorIdCommand(registroFrequenciasAExcluir.ToArray()));

                            if (!excluido)
                                return false;
                            else
                            {
                                var registrosFrequenciaValidos = registroFrequenciaAluno.Where(r => !registroFrequenciasAExcluir.Any(rf=> rf == r.RegistroFrequenciaAlunoId));
                                await VerificaCompensacoesDeAlunosInativos(registrosFrequenciaValidos, periodosEscolaresTurma, aluno.CodigoAluno, dadosTurma.CodigoTurma);

                                frequenciaAlunoParaRecalcular.AddRange(registroFrequenciaAluno
                                    .Where(f => f.DataAula.Date > dataReferencia.Value.Date)
                                    .Select(r => new FrequenciaAulaARecalcularDto()
                                    {
                                        AulaId = r.AulaId,
                                        DisciplinaCodigo = r.DisciplinaCodigo
                                    }));
                            }

                        }
                    }     
                }
            }

            try
            {
                frequenciaAlunoParaRecalcular = frequenciaAlunoParaRecalcular.DistinctBy(f => f.AulaId).ToList();

                foreach (var frequencia in frequenciaAlunoParaRecalcular)
                   await mediator.Send(new RecalcularFrequenciaPorTurmaCommand(request.TurmaCodigo, frequencia.DisciplinaCodigo, frequencia.AulaId));

                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao recalcular frequência da turma de código {request.TurmaCodigo}", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                return false;
            }
            
        }

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
