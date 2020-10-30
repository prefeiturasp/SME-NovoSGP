using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Arquivo : EntidadeBase
    {
        public string Nome { get; set; }
        public string NomeFisico { get; set; }
        public TipoArquivo Tipo { get; set; }
    }
}
