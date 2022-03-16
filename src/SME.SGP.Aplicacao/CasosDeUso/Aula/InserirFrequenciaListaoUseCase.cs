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

        public async Task<AuditoriaDto> Executar(IEnumerable<FrequenciaSalvarAulaAlunosDto> frequencias)
        {
            var ultimaAuditoria = new AuditoriaDto();
            foreach(var frequenciaAula in frequencias)
            {
                var frequencia = new FrequenciaDto(frequenciaAula.AulaId);

                foreach(var frequenciaAluno in frequenciaAula.Alunos)
                {
                    frequencia.ListaFrequencia.Add(new RegistroFrequenciaAlunoDto()
                    {
                        CodigoAluno = frequenciaAluno.CodigoAluno,
                        Aulas = frequenciaAluno.Frequencias.ToList()
                    });
                }

                ultimaAuditoria = await mediator.Send(new InserirFrequenciasAulaCommand(frequencia));
            }

            return ultimaAuditoria;
        }
    }
}
