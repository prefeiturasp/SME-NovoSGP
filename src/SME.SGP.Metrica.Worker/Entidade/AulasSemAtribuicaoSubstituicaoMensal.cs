using SME.SGP.Dominio;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Globalization;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class AulasSemAtribuicaoSubstituicaoMensal : EntidadeElasticBase
    {
        public AulasSemAtribuicaoSubstituicaoMensal(string turmaCodigo, string componenteCurricularCodigo, int modalidade,
                                                    DateTime data, int quantidade, bool ehRegencia = false) : base("")
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            Data = data.Date.ToUniversalTime();
            Quantidade = quantidade;
            Ano = data.Year;
            Mes = data.Month;
            Semana = UtilData.ObterSemanaDoAno(data);
            EhRegencia = ehRegencia;
            Modalidade = modalidade;
            Id = $"{turmaCodigo}-{ComponenteCurricularCodigo}-{(EhRegencia ? data.ToString("yyyyMMdd") : $"{data.ToString("yyyyMM")}-{Semana}")}";
        }
        public DateTime Data { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Semana { get; set; }
        public int Quantidade { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public bool EhRegencia { get; set; }
        public int Modalidade { get; set; }
    }
}
