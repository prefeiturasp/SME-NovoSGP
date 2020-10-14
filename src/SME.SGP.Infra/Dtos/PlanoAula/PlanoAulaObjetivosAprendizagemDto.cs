using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAulaObjetivosAprendizagemDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public AulaConsultaSimplesDto AulaConsultaSimples { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagemAula { get; set; }

        public void Adicionar(ObjetivoAprendizagemDto objetivoAprendizagem)
        {
            if (objetivoAprendizagem != null)
                ObjetivosAprendizagemAula.Add(objetivoAprendizagem);
        }
    }
}
