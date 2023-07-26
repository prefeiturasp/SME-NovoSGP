using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class EventoEADto
    {
        public string evento_id { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }
        public string dre_id { get; set; }
        public string ue_id { get; set; }
        public int tipo_evento { get; set; }
        public string turma_id { get; set; }
        public int ano_letivo { get; set; }
        public int modalidade { get; set; }
        public DateTime ultima_alteracao_sgp { get; set; }
        public bool excluido { get; set; }
        public string componente_curricular { get; set; }
        public long? tipo_calendario_id { get; set; }
    }
}