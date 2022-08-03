using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Cache
{
    public class ValorCache<T> where T : class
    {
        public string Chave { get; set; }
        public T Valor {  get; set; }
    }
}
