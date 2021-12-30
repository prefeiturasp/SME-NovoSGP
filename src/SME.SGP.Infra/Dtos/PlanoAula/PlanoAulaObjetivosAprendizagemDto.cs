using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAulaObjetivosAprendizagemDto
    {
        public PlanoAulaObjetivosAprendizagemDto()
        {
            ObjetivosAprendizagemComponente = new List<ObjetivosAprendizagemPorComponenteDto>();
        }

        public long Id { get; set; }
        public string Descricao { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }
        public long AulaId { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int Quantidade { get; set; }
        public long TipoCalendarioId { get; set; }
        public DateTime DataAula { get; set; }

        public bool Migrado { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRf { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRf { get; set; }
        public int TipoAula { get; set; }

        public List<ObjetivosAprendizagemPorComponenteDto> ObjetivosAprendizagemComponente { get; set; }

        public void Adicionar(ObjetivosAprendizagemPorComponenteDto objetivoAprendizagemComponente)
        {
            if (objetivoAprendizagemComponente != null)
                ObjetivosAprendizagemComponente.Add(objetivoAprendizagemComponente);
        } 
    }
}
