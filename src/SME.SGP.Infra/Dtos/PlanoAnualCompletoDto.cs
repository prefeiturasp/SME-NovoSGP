using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class PlanoAnualCompletoDto : AuditoriaDto
    {
        public PlanoAnualCompletoDto()
        {
            ObjetivosAprendizagem = new List<ObjetivoAprendizagemDto>();
        }

        public int? AnoLetivo { get; set; }

        public int Bimestre { get; set; }
        public string Descricao { get; set; }

        public string EscolaId { get; set; }
        public IEnumerable<long> IdsObjetivosAprendizagem => ObjetivosAprendizagemPlano?.Split(',').Select(c => Convert.ToInt64(c));
        public bool Migrado { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagem { get; set; }
        public bool ObjetivosAprendizagemOpcionais { get; set; }

        public bool Obrigatorio { get; set; }
        public string TurmaId { get; set; }
        private string ObjetivosAprendizagemPlano { get; }
    }
}