using Dapper.FluentMap.Mapping;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalAbandonoMap : EntityMap<PainelEducacionalAbandono>
    {
        public PainelEducacionalAbandonoMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Dre).ToColumn("dre");
            Map(p => p.Ano).ToColumn("ano");
            Map(p => p.ModalidadeTurma).ToColumn("modalidade_turma");
            Map(p => p.Turma).ToColumn("turma");
            Map(p => p.QuantidadeAlunoDesistentes).ToColumn("quantidade_aluno_desistentes");
            Map(p => p.CriadoEm).ToColumn("criado_em");
        }
    }
}
