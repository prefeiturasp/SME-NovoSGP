using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroHistoricoEscolarDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string TurmaCodigo { get; set; }
        public IEnumerable<FiltroHistoricoEscolarAlunosDto> Alunos { get; set; }
        public bool ImprimirDadosResponsaveis { get; set; }
        public bool PreencherDataImpressao { get; set; }
        public Usuario Usuario { get; set; }
        public bool ConsideraHistorico { get; set; }
        public short Semestre { get; set; }
    }

    public class FiltroHistoricoEscolarAlunosDto
    {
        public string AlunoCodigo { get; set; }
        public string ObservacaoComplementar { get; set; }
    }
}
