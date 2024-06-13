using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaProdutividadeDto
    {
        public string TurmaCodigo { get; set; }
        public string TurmaNome { get; set; }
        public Modalidade Modalidade { get; set; }
        public string UeCodigo { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string DreCodigo { get; set; }
        public string DreAbreviacao { get; set; }
        public DateTime DataFrequencia { get; set; }
        public DateTime DataAula { get; set; }
        public int DifDias { get; set; }
        public int Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public string ComponenteCodigo { get; set; }
        public string ComponenteNome { get; set; }
        public string ProfRf { get; set; }
        public string ProfNome { get; set; }

        public string DescricaoTurma()
            => $"{Modalidade.ObterNomeCurto()}-{TurmaNome}";

        public string DescricaoUe()
            =>  (TipoEscola)TipoEscola == TipoEscola.Nenhum
                ? UeNome
                : $"{((TipoEscola)TipoEscola).ShortName()} {UeNome}";
    }

    public static class RegistroFrequenciaProdutividadeExtension
    {
        public static ConsolidacaoProdutividadeFrequencia ToEntity(this RegistroFrequenciaProdutividadeDto source)
            => new ConsolidacaoProdutividadeFrequencia()
            {
                AnoLetivo = source.AnoLetivo,
                Bimestre = source.Bimestre,
                CodigoComponenteCurricular = source.ComponenteCodigo,
                CodigoDre = source.DreCodigo,
                CodigoTurma = source.TurmaCodigo,
                CodigoUe = source.UeCodigo,
                DataAula = source.DataAula,
                DataRegistroFrequencia = source.DataFrequencia,
                DescricaoDre = source.DreAbreviacao,
                DiferenciaDiasDataAulaRegistroFrequencia = source.DifDias,
                Modalidade = source.Modalidade,
                NomeComponenteCurricular = source.ComponenteNome,
                NomeProfessor = source.ProfNome,
                RfProfessor = source.ProfRf,
                DescricaoTurma = source.DescricaoTurma(),
                DescricaoUe = source.DescricaoUe()
            };
    }
}
