using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class EntidadeElasticBase
    {
        public EntidadeElasticBase(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
