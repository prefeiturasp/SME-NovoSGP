using System;

namespace SME.SGP.Infra
{
    public class ComandoRabbit
    {
        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, ulong quantidadeReprocessamentoDeadLetter = 3)
        {
            NomeProcesso = nomeProcesso;
            TipoCasoUso = tipoCasoUso;
            QuantidadeReprocessamentoDeadLetter = quantidadeReprocessamentoDeadLetter;
        }

        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, bool modeLazy) : this(nomeProcesso, tipoCasoUso)
        {
            ModeLazy = modeLazy;
        }

        public string NomeProcesso { get; private set; }
        public Type TipoCasoUso { get; private set; }
        public ulong QuantidadeReprocessamentoDeadLetter { get; private set; } = 3;
        public bool ModeLazy { get; set; } = false;
    }
}
