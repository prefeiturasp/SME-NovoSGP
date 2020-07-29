using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoMap : BaseMap<Comunicado>
    {
        public ComunicadoMap()
        {
            ToTable("comunicado");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Titulo).ToColumn("Titulo");
            Map(c => c.DataEnvio).ToColumn("data_envio");
            Map(c => c.DataExpiracao).ToColumn("data_expiracao");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.CodigoDre).ToColumn("codigo_dre");
            Map(c => c.CodigoUe).ToColumn("codigo_ue");
            Map(c => c.Turmas).ToColumn("turma");
            Map(c => c.AlunoEspecificado).ToColumn("alunos_especificados");
            Map(c => c.TipoComunicado).ToColumn("tipo_comunicado");
        }
    }
}