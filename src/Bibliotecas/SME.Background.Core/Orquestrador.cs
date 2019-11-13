using SME.Background.Core.Enumerados;
using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core
{
    public static class Orquestrador
    {
        static Dictionary<TipoProcessamento, IProcessadorCliente> processadores;

        static Orquestrador()
        {
            processadores = new Dictionary<TipoProcessamento, IProcessadorCliente>();
        }

        public static void Registrar<T>(T processador)
            where T: IProcessadorCliente
        {
            processador.Registrar();
            processadores.Add(TipoProcessamento.ExecucaoLonga, processador);
            processadores.Add(TipoProcessamento.ExecucaoImediata, processador);
            processadores.Add(TipoProcessamento.ExecucaoRecorrente, processador);
        }

        public static void Registrar<T>(T processador, TipoProcessamento tipoProcessamento)
            where T : IProcessadorCliente
        {
            processador.Registrar();
            processadores.Add(tipoProcessamento, processador);
        }

        public static IProcessadorCliente ObterProcessador(TipoProcessamento tipoProcessamento)
        {
            IProcessadorCliente processador = null;

            if (processadores.TryGetValue(tipoProcessamento, out processador))
                return processador;
            else
                throw new Exception($"Não foi possível obter um processador do tipo {tipoProcessamento.ToString()} pois não foi registrado");
        }

    }
}
