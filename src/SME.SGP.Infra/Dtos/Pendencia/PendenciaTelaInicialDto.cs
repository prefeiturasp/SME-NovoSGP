using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaTelaInicialDto
    {
        public string Descricao { get; set; }
        public SituacaoPendencia Situacao { get; set; }
        public TipoPendencia Tipo { get; set; }
        public string Titulo { get; set; }
        public string Instrucao { get; set; }
        public bool Excluido { get; set; }
        public string DescricaoHtml { get; set; }
        public long? UeId { get; set; }
        public int Modalidade { get; set; }
        public int Bimestre { get; set; }
        public string NomeBimestre { get => RetornaNomeBimestreConcatenado(Bimestre); }

        public string RetornaNomeBimestreConcatenado(int bimestre)
        {
            return $"{bimestre}º Bimestre";
        }
        
    }
}
