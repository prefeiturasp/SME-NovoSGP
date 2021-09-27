using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EventoAulaDto
    {
        public EventoAulaDto()
        {
            AtividadesAvaliativas = new List<AtividadeAvaliativaParaEventoAulaDto>();
            PodeCadastrarAvaliacao = false;
            EhAula = false;
            MostrarBotaoFrequencia = false;
            EhAulaCJ = false;
            EstaAguardandoAprovacao = false;
            EhReposicao = false;
        }
        public long ComponenteCurricularId { get; set; }
        public string Titulo { get; set; }
        public int Quantidade { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Descricao { get; set; }
        public bool PodeCadastrarAvaliacao { get; set; }
        public IList<AtividadeAvaliativaParaEventoAulaDto> AtividadesAvaliativas { get; set; }
        public long? AulaId { get; set; }
        public bool EhAula { get; set; }
        public bool EhAulaCJ { get; set; }
        public bool MostrarBotaoFrequencia { get; set; }
        public string TipoEvento { get; set; }
        public bool EhReposicao { get; set; }
        public bool EstaAguardandoAprovacao { get; set; }
        public long[] Pendencias { get; set; }
        public string Dre { get; set; }
        public string Ue { get; set; }
    }
}
