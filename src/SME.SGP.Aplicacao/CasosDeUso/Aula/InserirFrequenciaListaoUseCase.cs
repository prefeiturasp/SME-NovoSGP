using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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
            
            foreach(var frequenciaAula in frequencias)
            {
                var frequencia = new FrequenciaDto(frequenciaAula.AulaId);
                
                var alunosComAulasInvalidas = frequenciaAula.Alunos.Where(w => w.Desabilitado).Select(s => s.CodigoAluno).ToList();
                if (alunosComAulasInvalidas.Any())
                    await mediator.Send(new ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand(frequenciaAula.AulaId, alunosComAulasInvalidas.ToArray()));

                foreach(var frequenciaAluno in frequenciaAula.Alunos.Where(w=> !w.Desabilitado))
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
                    frequenciaAuditoria.TratarRetornoAuditoria(frequenciaAuditoriaAulaDto);    
                }
            }
            return frequenciaAuditoria;
        }
    }
}
