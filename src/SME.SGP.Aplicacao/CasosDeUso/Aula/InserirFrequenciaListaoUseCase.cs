using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciaListaoUseCase : AbstractUseCase, IInserirFrequenciaListaoUseCase
    {
        public InserirFrequenciaListaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<FrequenciaAuditoriaDto> Executar(IEnumerable<FrequenciaSalvarAulaAlunosDto> frequencias)
        {
            var frequenciaAuditoria = new FrequenciaAuditoriaDto();
            var datasAulas = new List<DateTime>();
            var alunos = new List<string>();
            var turmaId = string.Empty;
            var disciplinaId = string.Empty;

            foreach (var frequenciaAula in frequencias)
            {
                var frequencia = new FrequenciaDto(frequenciaAula.AulaId);

                var alunosComAulasInvalidas = frequenciaAula.Alunos.Where(w => w.Desabilitado).Select(s => s.CodigoAluno).ToList();
                if (alunosComAulasInvalidas.Any())
                    await mediator.Send(new ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand(frequenciaAula.AulaId, alunosComAulasInvalidas.ToArray()));

                foreach (var frequenciaAluno in frequenciaAula.Alunos.Where(w => !w.Desabilitado))
                {
                    frequencia.ListaFrequencia.Add(new RegistroFrequenciaAlunoDto()
                    {
                        CodigoAluno = frequenciaAluno.CodigoAluno,
                        Aulas = frequenciaAluno.Frequencias.ToList()
                    });
                }

                if (frequencia.ListaFrequencia.Any())
                {
                    var frequenciaAuditoriaAulaDto = await mediator.Send(new InserirFrequenciasAulaCommand(frequencia, false));
                    if (frequenciaAuditoriaAulaDto.DataAula.HasValue)
                    {
                        datasAulas.Add(frequenciaAuditoriaAulaDto.DataAula ?? DateTimeExtension.HorarioBrasilia().Date);
                        turmaId = frequenciaAuditoriaAulaDto.TurmaId;
                        disciplinaId = frequenciaAuditoriaAulaDto.DisciplinaId;
                        alunos.AddRange(frequenciaAula.Alunos.Select(aluno => aluno.CodigoAluno).ToList());
                    }

                    frequenciaAuditoria.TratarRetornoAuditoria(frequenciaAuditoriaAulaDto);
                } 
            }

            if (datasAulas.Any())
                await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos.Distinct(), datasAulas.Min(), turmaId, disciplinaId, datasAulas.Select(data => data.Month).Distinct().ToArray()));

            return frequenciaAuditoria;
        }
    }
}
