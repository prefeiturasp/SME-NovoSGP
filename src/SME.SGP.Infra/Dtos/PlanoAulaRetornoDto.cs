using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAulaRetornoDto
    {
        public PlanoAulaRetornoDto()
        {
            ObjetivosAprendizagemAula = new List<ObjetivoAprendizagemDto>();
        }

        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRf { get; set; }
        public long AulaId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRf { get; set; }
        public string Descricao { get; set; }
        public string DesenvolvimentoAula { get; set; }
        public long Id { get; set; }
        public long? IdAtividadeAvaliativa { get; set; }
        public string LicaoCasa { get; set; }
        public bool Migrado { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagemAula { get; set; }
        public bool PodeLancarNota { get; set; }
        public int QtdAulas { get; set; }
        public string RecuperacaoAula { get; set; }
        public bool PossuiPlanoAnual { get; set; }
        public bool ObjetivosAprendizagemOpcionais { get; set; }
    }
}