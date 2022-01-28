using SME.SGP.Dominio;
using System;
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

        public void CarregarAulas(IEnumerable<Aula> aulas, IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos, bool professorCj, bool ehGestor)
        {
            foreach (var aula in aulas.OrderBy(a => a.DataAula))
            {
                bool podeEditar = false;
                if (!aula.AulaCJ && !professorCj) 
                    podeEditar = true;
                else if (aula.AulaCJ && (professorCj || ehGestor)) 
                    podeEditar = true;
                else podeEditar = false;

                var frequenciaId = registrosFrequenciaAlunos.FirstOrDefault(a => a.AulaId == aula.Id)?.RegistroFrequenciaId;
                bool ehReposicao = TipoAula.Reposicao == aula.TipoAula;
                Aulas.Add(new AulaFrequenciaDto(aula.Id, aula.DataAula, aula.Quantidade, ehReposicao, frequenciaId, aula.AulaCJ, podeEditar));
            }
        }

        public void CarregarAuditoria(IEnumerable<RegistroFrequenciaAlunoPorAulaDto> registrosFrequenciaAlunos)
        {
            var ultimoRegistro = registrosFrequenciaAlunos
                .OrderByDescending(a => a.AlteradoEm > a.CriadoEm ? a.AlteradoEm : a.CriadoEm)
                .FirstOrDefault();

            if (ultimoRegistro != null)
                Auditoria = new AuditoriaDto()
                {
                    Id = ultimoRegistro.RegistroFrequenciaId,
                    CriadoEm = ultimoRegistro.CriadoEm,
                    CriadoPor = ultimoRegistro.CriadoPor,
                    CriadoRF = ultimoRegistro.CriadoRf,
                    AlteradoEm = ultimoRegistro.AlteradoEm,
                    AlteradoPor = ultimoRegistro.AlteradoPor,
                    AlteradoRF = ultimoRegistro.AlteradoRf,
                };

        }
    }
}
