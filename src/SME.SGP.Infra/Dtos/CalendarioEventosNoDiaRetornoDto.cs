using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class CalendarioEventosNoDiaRetornoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string TipoEvento { get; set; }
        public string DreNome { get; set; }
        public TipoEscola UeTipo { get; set; }
        private string _ueNome;
        public string UeNome { get => UeTipo != TipoEscola.Nenhum ? $"{UeTipo.ShortName()} {_ueNome}" : _ueNome; set => _ueNome = value; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        
    }
}