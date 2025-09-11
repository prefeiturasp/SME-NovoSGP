using System;

namespace SME.SGP.Dominio
{
    public class RecuperacaoParalelaObjetivo : EntidadeBase
    {
        public string Descricao { get; set; }
        public DateTime DtFim { get; set; }
        public DateTime DtInicio { get; set; }
        public bool EhEspecifico { get; set; }
        public int EixoId { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public int Ordem { get; set; }
    }
}