using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CompensacaoAusenciaAlunoMap : BaseMap<CompensacaoAusenciaAluno>
    {
        public CompensacaoAusenciaAlunoMap()
        {
            ToTable("compensacao_ausencia_aluno");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.CompensacaoAusenciaId).ToColumn("compensacao_ausencia_id");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.QuantidadeFaltasCompensadas).ToColumn("qtd_faltas_compensadas");
            Map(c => c.Notificado).ToColumn("notificado");
        }
    }
}
