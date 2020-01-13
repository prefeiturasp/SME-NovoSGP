using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class MarcadorFrequenciaDto
    {
        public TipoMarcadorFrequencia Tipo { get; set; }
        public string Nome { get => Enum.GetName(typeof(TipoMarcadorFrequencia), Tipo); }
        public string Descricao { get; set; }
    }
}
