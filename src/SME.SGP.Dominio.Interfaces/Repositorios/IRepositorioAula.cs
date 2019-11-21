using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplina(string turma, string disciplina);

        IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(long calendarioId, string turmaId, string disciplinaId, long usuarioId, Guid perfil);

        bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade);
    }
}