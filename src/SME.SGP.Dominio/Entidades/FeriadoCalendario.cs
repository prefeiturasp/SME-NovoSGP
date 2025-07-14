using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
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