using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Infra.Worker;
using SME.SGP.Metrica.Worker.Rotas;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;

namespace SME.SGP.Metrica.Worker
{
    public class WorkerRabbitMetrica : WorkerRabbitSGP
    {
        public WorkerRabbitMetrica(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory)
            : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas, telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitMetrica", typeof(RotasRabbitMetrica), false)
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitMetrica.AcessosSGP, new ComandoRabbit("Quantidade de Acessos Diario no SGP", typeof(IAcessosDiarioSGPUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasse, new ComandoRabbit("Registros de conselho de classe para o mesmo fechamento", typeof(IConselhoClasseDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConselhoClasseDuplicado, new ComandoRabbit("Limpeza de registros de conselho de classe para o mesmo fechamento", typeof(ILimpezaConselhoClasseDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseAluno, new ComandoRabbit("Registros de conselho de classe aluno para o mesmo conselho de classe", typeof(IConselhoClasseAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseAlunoUe, new ComandoRabbit("Registros de conselho de classe aluno para o mesmo conselho de classe na UE", typeof(IConselhoClasseAlunoUeDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConselhoClasseAlunoDuplicado, new ComandoRabbit("Limpeza de registros de conselho de classe aluno para o mesmo conselho de classe", typeof(ILimpezaConselhoClasseAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseNota, new ComandoRabbit("Registros de notas de conselho de classe para o mesmo aluno", typeof(IConselhoClasseNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConselhoClasseNotaDuplicado, new ComandoRabbit("Limpeza de registros de notas de conselho de classe para o mesmo aluno", typeof(ILimpezaConselhoClasseNotaDuplicadoUseCase)));
        }
    }
}
