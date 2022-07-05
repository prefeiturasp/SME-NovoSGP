using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroNota
    {
        public string Perfil { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario TipoCalendario { get; set; }
        public int Bimestre { get; set; }
        public string ComponenteCurricular { get; set; }
        public long TipoCalendarioId { get; set; }
        public bool CriarPeriodoEscolar { get; set; }
        public bool CriarPeriodoAbertura { get; set; }
    }
}