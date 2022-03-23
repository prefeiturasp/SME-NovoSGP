using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaRegistroIndividualAlunoMap : DommelEntityMap<PendenciaRegistroIndividualAluno>
    {
        public PendenciaRegistroIndividualAlunoMap()
        {
            ToTable("pendencia_registro_individual_aluno");

            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.PendenciaRegistroIndividualId).ToColumn("pendencia_registro_individual_id");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}