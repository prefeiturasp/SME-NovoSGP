using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoMatriculaMap : BaseMap<EventoMatricula>
    {
        public EventoMatriculaMap()
        {
            ToTable("evento_matricula");
            Map(nameof(EventoMatricula.CodigoAluno), "codigo_aluno");
            Map(nameof(EventoMatricula.Tipo), "tipo");
            Map(nameof(EventoMatricula.DataEvento), "data_evento");
            Map(nameof(EventoMatricula.NomeEscola), "nome_escola");
            Map(nameof(EventoMatricula.NomeTurma), "nome_turma");
        }
    }
}