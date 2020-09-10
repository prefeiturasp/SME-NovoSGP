using System;

namespace SME.SGP.Dominio
{
    public class RecuperacaoParalelaEixo : EntidadeBase
    {
        public string Descricao { get; set; }
        public DateTime DtFim { get; set; }
        public DateTime DtInicio { get; set; }
        public bool Excluido { get; set; }
        public int RecuperacaoParalelaPeriodoId { get; set; }
    }
}