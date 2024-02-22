using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPASecaoItineranciaDto
    {
        public string Nome { get; set; }
        public long QuestionarioId { get; set; }
        public int Etapa { get; set; }
        public int Ordem { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string TipoAtendimento { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<ArquivoResumidoDto> Arquivos { get; set; } = Enumerable.Empty<ArquivoResumidoDto>();
    }
}
