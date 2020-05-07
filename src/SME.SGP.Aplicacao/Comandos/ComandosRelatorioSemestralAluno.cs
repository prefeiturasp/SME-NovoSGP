using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosRelatorioSemestralAluno : IComandosRelatorioSemestralAluno
    {
        public async Task<AuditoriaRelatorioSemestralAlunoDto> Salvar(string alunoCodigo, string turmaCodigo, int semestre, long relatorioSemestralId, long relatorioSemestralAlunoId, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto)
        {
            return new AuditoriaRelatorioSemestralAlunoDto()
            {
                RelatorioSemestralId = 1,
                RelatorioSemestralAlunoId = 1,
                Auditoria = new AuditoriaDto()
                {
                    Id = 1,
                    CriadoPor = "Fulano",
                    CriadoEm = DateTime.Today,
                    CriadoRF = "789456123",
                    AlteradoPor = "Fulano",
                    AlteradoEm = DateTime.Now,
                    AlteradoRF = "789456123",
                }
            };
        }
    }
}
