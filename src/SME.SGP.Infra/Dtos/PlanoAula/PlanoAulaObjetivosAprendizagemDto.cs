using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAulaObjetivosAprendizagemDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long AulaId { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int Quantidade { get; set; }
        public long TipoCalendarioId { get; set; }
        public DateTime DataAula { get; set; }

        //public AulaConsultaSimplesDto AulaConsultaSimples { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagemAula { get; set; }

        public void Adicionar(ObjetivoAprendizagemDto objetivoAprendizagem)
        {
            if (objetivoAprendizagem != null)
                ObjetivosAprendizagemAula.Add(objetivoAprendizagem);
        }
    }
}
