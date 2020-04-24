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
        }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool PodeCadastrarAvaliacao { get; set; }
        public IList<AtividadeAvaliativaParaEventoAulaDto> AtividadesAvaliativas { get; set; }
        public bool EhAula { get; set; }
        public bool MostrarBotaoFrequencia { get; set; }
        public string TipoEvento { get; set; }

    }
}
