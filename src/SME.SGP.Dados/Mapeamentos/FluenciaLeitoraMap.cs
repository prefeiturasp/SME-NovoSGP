using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FluenciaLeitoraMap : BaseMap<FluenciaLeitora>
    {
        public FluenciaLeitoraMap()
        {
            ToTable("fluencia_leitora");
            Map(nameof(FluenciaLeitora.AnoLetivo), "ano_letivo");
            Map(nameof(FluenciaLeitora.CodigoEOLTurma), "codigo_eol_turma");
            Map(nameof(FluenciaLeitora.CodigoEOLAluno), "codigo_eol_aluno");
            Map(nameof(FluenciaLeitora.Fluencia), "fluencia");
            Map(nameof(FluenciaLeitora.TipoAvaliacao), "tipo_avaliacao");
        }
    }
}