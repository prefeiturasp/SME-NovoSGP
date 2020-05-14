using System;

namespace SME.SGP.Dominio
{
    public class NotaTipoValor : EntidadeBase
    {
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public DateTime FimVigencia { get; set; }
        public DateTime InicioVigencia { get; set; }
        public TipoNota TipoNota { get; set; }

        public bool EhNota()
        {
            return TipoNota == TipoNota.Nota;
        }
    }
}