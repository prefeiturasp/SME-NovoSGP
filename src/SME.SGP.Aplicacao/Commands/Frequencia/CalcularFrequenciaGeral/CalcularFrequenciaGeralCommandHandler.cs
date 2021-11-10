using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaGeralCommandHandler : IRequestHandler<CalcularFrequenciaGeralCommand, bool>
    {
        private readonly IMediator mediator;

        public CalcularFrequenciaGeralCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CalcularFrequenciaGeralCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var registroFrequenciaAlunos = (await mediator.Send(new ObterFrequenciaAlunosGeralPorAnoQuery(request.Ano))).ToList();

                var alunosEmTurmasDisciplinasData = registroFrequenciaAlunos.GroupBy(g => new { g.DisciplinaId, g.TurmaId, g.DataAula }, (key, group) => 
                new { key.DisciplinaId, key.TurmaId, key.DataAula, Alunos = group.Select(s=> s.AlunoCodigo).ToList()});                

                foreach (var alunoEmTurmaDisciplinaData in alunosEmTurmasDisciplinasData)
                {
                    var comando = new CalcularFrequenciaPorTurmaCommand(alunoEmTurmaDisciplinaData.Alunos, alunoEmTurmaDisciplinaData.DataAula, alunoEmTurmaDisciplinaData.TurmaId.ToString(), alunoEmTurmaDisciplinaData.DisciplinaId);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null));
                };

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro no Calculo Geral de Frequência.", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                throw;
            }
        } 
    }
}
