using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FluenciaLeitoraMap : BaseMap<FluenciaLeitora>
    {
        public FluenciaLeitoraMap()
        {
            ToTable("fluencia_leitora");
            Map(c => c.CodigoEOLTurma).ToColumn("codigo_eol_turma");
            Map(c => c.CodigoEOLAluno).ToColumn("codigo_eol_aluno");
            Map(c => c.Fluencia).ToColumn("fluencia");
            Map(c => c.Periodo).ToColumn("periodo");
        }
    }
}
