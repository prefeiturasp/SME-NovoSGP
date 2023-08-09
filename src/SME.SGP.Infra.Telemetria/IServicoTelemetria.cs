using Elastic.Apm.Api;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public interface IServicoTelemetria
    {

        ITransaction? Iniciar(string nome, string tipo);
        void Finalizar(ITransaction? transacao);

        //nesse caso nao entendi direito o porque do uso da palavra chave dynamic aqui, sendo que o tipo dinamico já é indicado no metodo
        //a keyword dynamic nao vai conseguir fazer checagens de tipo em tempo de compilacao
        //dynamic tambem vai usar mais ciclos de cpu do que tipo estaticos passados
        //se nao for o caso de realmente usar dynamic no metodo eu trocaria para o Tipo T do metodo
        Task<T> RegistrarComRetornoAsync<T>(Func<Task<T>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");

        //Caso de assinatura que evita alocacao dos delegates
        Task<T> RegistrarComRetornoAsync<T1,T2,T3,T>(T1 t1,T2 t2,T3 t3,Func<T1,T2,T3,Task<T>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");


        T RegistrarComRetorno<T>(Func<T> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
    }
}
