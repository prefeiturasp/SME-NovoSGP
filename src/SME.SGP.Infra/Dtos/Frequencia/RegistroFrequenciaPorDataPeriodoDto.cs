using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaPorDataPeriodoDto
    {
        public RegistroFrequenciaPorDataPeriodoDto()
        {
            Aulas = new List<AulaFrequenciaDto>();
            Alunos = new List<AlunoRegistroFrequenciaDto>();
        }

        public AuditoriaDto Auditoria { get; set; }
        public IList<AulaFrequenciaDto> Aulas { get; set; }
        public IList<AlunoRegistroFrequenciaDto> Alunos { get; set; }

        public void CarregarAulas(IEnumerable<Aula> aulas, IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos)
        {
            foreach (var aula in aulas.OrderBy(a => a.DataAula))
            {
                var frequenciaId = registrosFrequenciaAlunos.FirstOrDefault(a => a.AulaId == aula.Id)?.RegistroFrequenciaId;

                Aulas.Add(new AulaFrequenciaDto(aula.Id, aula.DataAula, aula.Quantidade, frequenciaId));
            }
        }
    }
}
