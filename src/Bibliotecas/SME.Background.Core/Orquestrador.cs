using SME.Background.Core.Enumerados;
using SME.Background.Core.Interfaces;
using SME.Background.Core.Processors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SME.Background.Core
{
    public static class Orquestrador
    {
        static ConcurrentDictionary<TipoProcessamento, IProcessor> processadores;

        public static IServiceProvider Provider { get; private set; }

        static Orquestrador()
        {
            processadores = new ConcurrentDictionary<TipoProcessamento, IProcessor>();
        }

        public static void Inicializar(IServiceProvider provider)
        {
            if (Provider == null)
                Provider = provider;
        }

        public static void Desativar()
        {
            processadores.Clear();
            Registrar<DisabledProcessor>(new DisabledProcessor());
        }

        public static void Registrar<T>(T processador)
            where T : IProcessor
        {
            Registrar<T>(processador, TipoProcessamento.ExecucaoImediata);
            Registrar<T>(processador, TipoProcessamento.ExecucaoLonga);
            Registrar<T>(processador, TipoProcessamento.ExecucaoRecorrente);
        }

        public static void Registrar<T>(T processador, TipoProcessamento tipoProcessamento)
            where T : IProcessor
        {
            if (processadores.TryAdd(tipoProcessamento, processador) && !processador.Registrado)
            {
                processador.Registrar();
                Console.WriteLine($"O processador {processador.GetType().Name} foi registrado para o tipo de processamento {tipoProcessamento.ToString()}");
            }
        }

        public static IProcessor ObterProcessador(TipoProcessamento tipoProcessamento)
        {
            IProcessor processador = null;

            if (processadores.TryGetValue(tipoProcessamento, out processador))
                return processador;
            else
                throw new Exception($"Não foi possível obter um processador do tipo {tipoProcessamento.ToString()} pois não foi registrado");
        }
    }
}
