using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class PlanoAnualCompletoDto
    {
        public PlanoAnualCompletoDto()
        {
            ObjetivosAprendizagem = new List<ObjetivoAprendizagemDto>();
        }

        public DateTime AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public int? AnoLetivo { get; set; }

        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public string Descricao { get; set; }

        public string EscolaId { get; set; }
        public long Id { get; set; }
        public IEnumerable<long> IdsObjetivosAprendizagem => ObjetivosAprendizagemPlano?.Split(',').Select(c => Convert.ToInt64(c));
        public bool Migrado { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagem { get; set; }

        public long TurmaId { get; set; }
        private string ObjetivosAprendizagemPlano { get; }
    }
}