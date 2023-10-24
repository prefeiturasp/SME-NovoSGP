using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.Metrica.Worker.Repositorios;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Reflection;
using System.Threading;

namespace SME.SGP.Metrica.Worker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            RegistrarElasticSearch(services);
            RegistrarDependencias(services);
            RegistrarMapeamentos();
            RegistrarTelemetria(services);
            RegistrarRabbitMQ(services);
        }

        private void RegistrarElasticSearch(IServiceCollection services)
        {
            services.RegistrarElastic(Configuration);
        }

        private void RegistrarTelemetria(IServiceCollection services)
        {
            services.AddOptions<TelemetriaOptions>()
                .Bind(Configuration.GetSection(TelemetriaOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<TelemetriaOptions>();
            services.AddSingleton<IServicoTelemetria, ServicoTelemetria>();

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<TelemetriaOptions>>();
            var servicoTelemetria = new ServicoTelemetria(options);
            DapperExtensionMethods.Init(servicoTelemetria);

        }

        private void RegistrarRabbitMQ(IServiceCollection services)
        {
            services.AddPolicies();
            services.ConfigurarRabbit(Configuration);
            services.ConfigurarRabbitParaLogs(Configuration);
        }

        private void RegistrarMapeamentos()
        {
            FluentMapper.Initialize(config =>
            {

                config.ForDommel();
            });
        }

        private void RegistrarDependencias(IServiceCollection services)
        {
            services.ConfigurarTelemetria(Configuration);
            services.AddHttpContextAccessor();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            RegistrarRepositorio(services);
            RegistrarUseCases(services);
        }

        private void RegistrarRepositorio(IServiceCollection services)
        {
            services.TryAddScoped<IContextoAplicacao, ContextoHttp>();
            services.TryAddScoped<ISgpContext, SgpContext>();
            // Postgres
            services.TryAddScoped<IRepositorioSGP, RepositorioSGP>();
            services.TryAddScoped<IRepositorioSGPConsulta, RepositorioSGPConsulta>();
            // ElasticSearch
            services.TryAddScoped<IRepositorioAcessos, RepositorioAcessos>();
            services.TryAddScoped<IRepositorioConselhoClasseDuplicado, RepositorioConselhoClasseDuplicado>();
            services.TryAddScoped<IRepositorioConselhoClasseAlunoDuplicado, RepositorioConselhoClasseAlunoDuplicado>();
            services.TryAddScoped<IRepositorioConselhoClasseNotaDuplicado, RepositorioConselhoClasseNotaDuplicado>();
            services.TryAddScoped<IRepositorioFechamentoTurmaDuplicado, RepositorioFechamentoTurmaDuplicado>();
            services.TryAddScoped<IRepositorioFechamentoTurmaDisciplinaDuplicado, RepositorioFechamentoTurmaDisciplinaDuplicado>();
            services.TryAddScoped<IRepositorioFechamentoAlunoDuplicado, RepositorioFechamentoAlunoDuplicado>();
            services.TryAddScoped<IRepositorioFechamentoNotaDuplicado, RepositorioFechamentoNotaDuplicado>();
            services.TryAddScoped<IRepositorioConsolidacaoConselhoClasseNotaNulos, RepositorioConsolidacaoConselhoClasseNotaNulos>();
            services.TryAddScoped<IRepositorioConsolidacaoConselhoClasseAlunoTurmaDuplicado, RepositorioConsolidacaoConselhoClasseAlunoTurmaDuplicado>();
            services.TryAddScoped<IRepositorioConsolidacaoCCNotaDuplicado, RepositorioConsolidacaoCCNotaDuplicado>();
            services.TryAddScoped<IRepositorioConselhoClasseNaoConsolidado, RepositorioConselhoClasseNaoConsolidado>();
        }

        private void RegistrarUseCases(IServiceCollection services)
        {
            services.TryAddScoped<IAcessosDiarioSGPUseCase, AcessosDiarioSGPUseCase>();
            services.TryAddScoped<IConselhoClasseDuplicadoUseCase, ConselhoClasseDuplicadoUseCase>();
            services.TryAddScoped<ILimpezaConselhoClasseDuplicadoUseCase, LimpezaConselhoClasseDuplicadoUseCase>();
            services.TryAddScoped<IConselhoClasseAlunoDuplicadoUseCase, ConselhoClasseAlunoDuplicadoUseCase>();
            services.TryAddScoped<IConselhoClasseAlunoUeDuplicadoUseCase, ConselhoClasseAlunoUeDuplicadoUseCase>();
            services.TryAddScoped<ILimpezaConselhoClasseAlunoDuplicadoUseCase, LimpezaConselhoClasseAlunoDuplicadoUseCase>();
            services.TryAddScoped<IConselhoClasseNotaDuplicadoUseCase, ConselhoClasseNotaDuplicadoUseCase>();
            services.TryAddScoped<ILimpezaConselhoClasseNotaDuplicadoUseCase, LimpezaConselhoClasseNotaDuplicadoUseCase>();
            services.TryAddScoped<IFechamentoTurmaDuplicadoUseCase, FechamentoTurmaDuplicadoUseCase>();
            services.TryAddScoped<ILimpezaFechamentoTurmaDuplicadoUseCase, LimpezaFechamentoTurmaDuplicadoUseCase>();
            services.TryAddScoped<IFechamentoTurmaDisciplinaDuplicadoUseCase, FechamentoTurmaDisciplinaDuplicadoUseCase>();
            services.TryAddScoped<ILimpezaFechamentoTurmaDisciplinaDuplicadoUseCase, LimpezaFechamentoTurmaDisciplinaDuplicadoUseCase>();
            services.TryAddScoped<IFechamentoAlunoDuplicadoUseCase, FechamentoAlunoDuplicadoUseCase>();
            services.TryAddScoped<IFechamentoAlunoDuplicadoUEUseCase, FechamentoAlunoDuplicadoUEUseCase>();
            services.TryAddScoped<ILimpezaFechamentoAlunoDuplicadoUseCase, LimpezaFechamentoAlunoDuplicadoUseCase>();
            services.TryAddScoped<IFechamentoNotaDuplicadoUseCase, FechamentoNotaDuplicadoUseCase>();
            services.TryAddScoped<IFechamentoNotaDuplicadoTurmaUseCase, FechamentoNotaDuplicadoTurmaUseCase>();
            services.TryAddScoped<ILimpezaFechamentoNotaDuplicadoUseCase, LimpezaFechamentoNotaDuplicadoUseCase>();
            services.TryAddScoped<IConsolidacaoConselhoClasseNotaNuloUseCase, ConsolidacaoConselhoClasseNotaNuloUseCase>();
            services.TryAddScoped<IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase, ConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase>();
            services.TryAddScoped<IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase, ConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase>();
            services.TryAddScoped<ILimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase, LimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase>();
            services.TryAddScoped<IConsolidacaoCCNotaDuplicadoUseCase, ConsolidacaoCCNotaDuplicadoUseCase>();
            services.TryAddScoped<ILimpezaConsolidacaoCCNotaDuplicadoUseCase, LimpezaConsolidacaoCCNotaDuplicadoUseCase>();
            services.TryAddScoped<IConselhoClasseNaoConsolidadoUseCase, ConselhoClasseNaoConsolidadoUseCase>();
            services.TryAddScoped<IConselhoClasseNaoConsolidadoUEUseCase, ConselhoClasseNaoConsolidadoUEUseCase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            app.UseHealthChecksSgp();
            app.UseHealthCheckPrometheusSgp();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();


            var threadPoolOptions = new ThreadPoolOptions();
            Configuration.GetSection(ThreadPoolOptions.Secao).Bind(threadPoolOptions, c => c.BindNonPublicProperties = true);
            if (threadPoolOptions.WorkerThreads > 0 && threadPoolOptions.CompletionPortThreads > 0)
                ThreadPool.SetMinThreads(threadPoolOptions.WorkerThreads, threadPoolOptions.CompletionPortThreads);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitMetricas!");
            });
        }
    }
}
