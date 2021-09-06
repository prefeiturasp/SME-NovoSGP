using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaFechamentoCompletoDto: AuditoriaDto
    {
        public long PendenciaId { get; set; }
        public long FechamentoId { get; set; }
        public int Bimestre { get; set; }
        public long DisciplinaId { get; set; }
        public string ComponenteCurricular { get; set; }
        public string Descricao { get; set; }
        public string Detalhamento { get; set; }
        public int Situacao { get; set; }
        public string SituacaoNome { get; set; }
        public string DescricaoHtml { get; set; }
        public string DetalhamentoFormatado
        {
            get => string.IsNullOrEmpty(DescricaoHtml) ? Detalhamento : DescricaoHtml;
        }

    }
}
