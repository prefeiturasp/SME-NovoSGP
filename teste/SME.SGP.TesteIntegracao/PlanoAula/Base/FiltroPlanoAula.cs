using System;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.PlanoAula.Base
{
    public class FiltroPlanoAula
    {
        public string Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario TipoCalendario { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Bimestre { get; set; }
        public DateTime DataAula { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public bool CriarPeriodoEscolarBimestre { get; set; }
        public long TipoCalendarioId { get; set; }
        public bool CriarPeriodoEscolarEAberturaTodosBimestres { get; set; }
        public int QuantidadeAula { get; set; }  
    }
} 