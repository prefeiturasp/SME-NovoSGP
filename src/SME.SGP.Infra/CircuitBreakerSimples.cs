using SME.SGP.Infra.Enumerados;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;

namespace SME.SGP.Infra
{
    public sealed class CircuitBreaker : ICircuitBreaker
    {
        private readonly int _limiteFalhas;
        private readonly TimeSpan _tempoAbertura;

        private int _falhas;
        private DateTime? _abertoAte;
        private int _halfOpenEmUso;

        public EstadoCircuito EstadoAtual
        {
            get
            {
                if (!_abertoAte.HasValue)
                    return EstadoCircuito.Closed;

                if (DateTime.UtcNow < _abertoAte.Value)
                    return EstadoCircuito.Open;

                return EstadoCircuito.HalfOpen;
            }
        }

        public CircuitBreaker(int limiteFalhas, TimeSpan tempoAbertura)
        {
            _limiteFalhas = limiteFalhas;
            _tempoAbertura = tempoAbertura;
        }

        public bool PodeExecutar()
        {
            var estado = EstadoAtual;

            if (estado == EstadoCircuito.Closed)
                return true;

            if (estado == EstadoCircuito.Open)
                return false;

            return Interlocked.CompareExchange(
                ref _halfOpenEmUso,
                1,
                0
            ) == 0;
        }

        public void RegistrarSucesso()
        {
            Interlocked.Exchange(ref _falhas, 0);
            _abertoAte = null;
            Interlocked.Exchange(ref _halfOpenEmUso, 0);
        }

        public void RegistrarFalha()
        {
            var falhas = Interlocked.Increment(ref _falhas);

            if (falhas >= _limiteFalhas)
            {
                _abertoAte = DateTime.UtcNow.Add(_tempoAbertura);
                Interlocked.Exchange(ref _halfOpenEmUso, 0);
            }
        }
    }
}
