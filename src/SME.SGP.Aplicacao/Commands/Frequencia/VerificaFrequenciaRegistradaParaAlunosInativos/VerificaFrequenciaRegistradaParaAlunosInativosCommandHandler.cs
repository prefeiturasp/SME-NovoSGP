using MediatR;
using SME.SGP.Dominio;
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
            var alunosComRegistroExcluido = new List<string>();
            var frequenciaAlunoParaRecalcular = new List<FrequenciaAulaARecalcularDto>();
            var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = request.TurmaCodigo });
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaCodigo, true));

            foreach (var aluno in alunosDaTurma)
            {
                //var frequenciasDoAluno = await mediator.Send(new ObterFrequenciaBimestresQuery() { CodigoAluno = aluno.CodigoAluno, CodigoTurma = request.TurmaCodigo, 
                //                                              TipoFrequencia = TipoFrequenciaAluno.PorDisciplina });

                //agrupar aula e componente afetado juntos

                var registroFrequenciaAluno = await mediator.Send(new ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery(request.TurmaCodigo, aluno.CodigoAluno));

                if (registroFrequenciaAluno.Any())
                {
                    DateTime dataReferencia = aluno.PossuiSituacaoAtiva() ? aluno.DataMatricula : aluno.DataSituacao;

                    var registroFrequenciasAExcluir = registroFrequenciaAluno.Where(f => f.DataAula.Date > dataReferencia.Date).Select(r => r.RegistroFrequenciaAlunoId);

                    if (registroFrequenciasAExcluir.Any())
                    {
                        bool excluido = await mediator.Send(new ExcluirRegistroFrequenciaAlunoPorIdCommand(registroFrequenciasAExcluir.ToArray()));

                        if (!excluido)
                            return false;
                        else
                        {
                            //frequenciaAlunoParaRecalcular.AddRange(registroFrequenciaAluno.Select(r=> new FrequenciaAulaARecalcularDto(){
                            //    AulaId = r.AulaId,
                            //    DisciplinaCodigo = r.DisciplinaCodigo
                            //    }))
                            //alunosComRegistroExcluido.Add(aluno.CodigoAluno);
                        }
                            
                    }
                }
            }
            //await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunosComRegistroExcluido, ))
            return true;
        }
    }
}
