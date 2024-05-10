using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FeriadoCalendarioDto
    {
        [EnumeradoRequirido(ErrorMessage = "A abrangência deve ser informada.")]
        public AbrangenciaFeriadoCalendario Abrangencia { get; set; }

        [Required(ErrorMessage = "O status (Ativo/Inativo) deve ser informado.")]
        public bool Ativo { get; set; }

        [Range(typeof(DateTime), "1900-01-01", "2500-01-01", ErrorMessage = "Data Inválida / Não informada")]
        public DateTime DataFeriado { get; set; }

        public string DescricaoAbrangencia { get { return Abrangencia == 0 ? "" : Abrangencia.GetAttribute<DisplayAttribute>().Name; } }
        public string DescricaoTipo { get { return Tipo == 0 ? "" : Tipo.GetAttribute<DisplayAttribute>().Name; } }
        public long Id { get; set; }

        [Required(ErrorMessage = "O nome  deve ser informado.")]
        public string Nome { get; set; }

        [EnumeradoRequirido(ErrorMessage = "O tipo deve ser informado.")]
        public TipoFeriadoCalendario Tipo { get; set; }
    }
}