using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FeriadoCalendarioCompletoDto
    {
        public AbrangenciaFeriadoCalendario Abrangencia { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public DateTime DataFeriado { get; set; }
        public string DescricaoAbrangencia { get { return Abrangencia == 0 ? "" : Abrangencia.GetAttribute<DisplayAttribute>().Name; } }
        public string DescricaoTipo { get { return Tipo == 0 ? "" : Tipo.GetAttribute<DisplayAttribute>().Name; } }
        public long Id { get; set; }
        public string Nome { get; set; }
        public TipoFeriadoCalendario Tipo { get; set; }
        public bool PossuiEventos { get; set; }
    }
}