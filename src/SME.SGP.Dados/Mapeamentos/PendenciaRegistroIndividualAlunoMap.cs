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
            Map(x => x.CodigoAluno).ToColumn("codigo_aluno");
            Map(x => x.PendenciaRegistroIndividualId).ToColumn("pendencia_registro_individual_id");
        }
    }
}