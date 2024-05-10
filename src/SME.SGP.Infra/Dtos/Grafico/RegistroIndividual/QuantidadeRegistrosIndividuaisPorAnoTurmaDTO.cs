using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class QuantidadeRegistrosIndividuaisPorAnoTurmaDTO
    {
        public string Ano { get; set; }
        public string Turma { get; set; }
        public int QuantidadeRegistrosIndividuais { get; set; }
        public string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
               => possuiFiltroUe
               ? turmaAno
               : $"{modalidade.ShortName()} - {turmaAno}";
    }
}

