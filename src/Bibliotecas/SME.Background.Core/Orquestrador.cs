using SME.Background.Core.Enumerados;
using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core
{
    public static class Orquestrador
    {
        static Dictionary<TipoProcessamento, IProcessor> processadores;

        static Orquestrador()
        {
            processadores = new Dictionary<TipoProcessamento, IProcessor>();
        }

        public static void Registrar<T>(T processador)
            where T: IProcessor
        {
            processador.Registrar();
            processadores.Add(TipoProcessamento.ExecucaoLonga, processador);
            processadores.Add(TipoProcessamento.ExecucaoImediata, processador);
            processadores.Add(TipoProcessamento.ExecucaoRecorrente, processador);
        }

        public static void Registrar<T>(T processador, TipoProcessamento tipoProcessamento)
            where T : IProcessor
        {
            processador.Registrar();
            processadores.Add(tipoProcessamento, processador);
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
