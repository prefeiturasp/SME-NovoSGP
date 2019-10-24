using System;

namespace SME.SGP.Dominio
{
    public class FeriadoCalendario : EntidadeBase
    {
        public FeriadoCalendario()
        {
            Ativo = true;
        }

        public AbrangenciaFeriadoCalendario Abrangencia { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataFeriado { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public TipoFeriadoCalendario Tipo { get; set; }
    }
}