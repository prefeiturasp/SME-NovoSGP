using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class EventoTipoInclusaoDto
    {
        public bool Ativo { get; set; }
        public bool Concomitancia { get; set; }
        public bool Dependencia { get; set; }

        [Required(ErrorMessage = "A Descrição é obrigatória")]
        public string Descricao { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Deve ser informado se o feriado é letivo, opcional ou não letivo")]
        public EventoLetivo Letivo { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Deve ser informando o local de ocorrência")]
        public EventoLocalOcorrencia LocalOcorrencia { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Deve ser informado o tipo de data")]
        public EventoTipoData TipoData { get; set; }
    }
}