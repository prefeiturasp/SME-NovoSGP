using System;

namespace SME.SGP.Infra
{
    public class ComandoRabbit
    {
        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, ulong quantidadeReprocessamentoDeadLetter = 3, int ttl = 10 * 60 * 1000)
        {
            NomeProcesso = nomeProcesso;
            TipoCasoUso = tipoCasoUso;
            QuantidadeReprocessamentoDeadLetter = quantidadeReprocessamentoDeadLetter;
            TTL = ttl;
        }

        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, bool modeLazy) : this(nomeProcesso, tipoCasoUso)
        {
            ModeLazy = modeLazy;
        }

        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, bool modeLazy, ulong quantidadeReprocessamentoDeadLetter, int ttl) 
            : this(nomeProcesso, tipoCasoUso, quantidadeReprocessamentoDeadLetter, ttl)
        {
            ModeLazy = modeLazy;
        }

        public string NomeProcesso { get; private set; }
        public Type TipoCasoUso { get; private set; }
        public ulong QuantidadeReprocessamentoDeadLetter { get; set; } = 3;
        public bool ModeLazy { get; set; } = false;
        public int TTL { get; set; } = 10 * 60 * 1000;
    }
}
