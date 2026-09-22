using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Fechamento.Performance
{
    // Somente instrumentação de teste. Mede chamadas reais aos repositórios,
    // incluindo sua conclusão assíncrona; não simula latência de banco.
    public class ObservadorRepositorio<T> : DispatchProxy where T : class
    {
        public T Alvo { get; set; }
        public MedicaoRepositorios Medicao { get; set; }

        public static T Criar(T alvo, MedicaoRepositorios medicao)
        {
            var proxy = Create<T, ObservadorRepositorio<T>>();
            var observador = (ObservadorRepositorio<T>)(object)proxy;
            observador.Alvo = alvo;
            observador.Medicao = medicao;
            return proxy;
        }

        protected override object Invoke(MethodInfo metodo, object[] argumentos)
        {
            var relogio = Stopwatch.StartNew();
            try
            {
                var retorno = metodo.Invoke(Alvo, argumentos);
                if (retorno is Task && metodo.ReturnType.IsGenericType)
                    return typeof(ObservadorRepositorio<T>).GetMethod(nameof(ObservarAsync), BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(metodo.ReturnType.GetGenericArguments()[0])
                        .Invoke(this, new[] { retorno, metodo.Name, relogio });
                Registrar(metodo.Name, relogio);
                return retorno;
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                Registrar(metodo.Name, relogio);
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        private async Task<TResult> ObservarAsync<TResult>(Task<TResult> tarefa, string metodo, Stopwatch relogio)
        {
            try { return await tarefa; }
            finally { Registrar(metodo, relogio); }
        }

        private void Registrar(string metodo, Stopwatch relogio)
        {
            relogio.Stop();
            Medicao.Chamadas.Add(new ChamadaRepositorio
            {
                Repositorio = typeof(T).Name, Metodo = metodo, Milissegundos = relogio.Elapsed.TotalMilliseconds
            });
        }
    }

    public class MedicaoRepositorios
    {
        public List<ChamadaRepositorio> Chamadas { get; } = new List<ChamadaRepositorio>();
    }

    public class ChamadaRepositorio
    {
        public string Repositorio { get; set; }
        public string Metodo { get; set; }
        public double Milissegundos { get; set; }
    }
}
