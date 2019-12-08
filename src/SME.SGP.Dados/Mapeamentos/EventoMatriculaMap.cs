using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoMatriculaMap : BaseMap<EventoMatricula>
    {
        public EventoMatriculaMap()
        {
            ToTable("evento_matricula");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.DataEvento).ToColumn("data_evento");
            Map(c => c.NomeEscola).ToColumn("nome_escola");
            Map(c => c.NomeTurma).ToColumn("nome_turma");
        }
    }
}
