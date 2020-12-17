# SME.Background.Core

Essa biblioteca foi criada com o objetivo de processar assincronamente métodos ***void*** utilizando *workers* responsáveis por captar a solicitação de execução e retornar um *idCorrelato* para acompanhamento (futuro).

 - Nesta versão está disponível  o worker baseado na solução hangfire, utilizando banco de dados PostgreSQL

# Como utilizar?

### Configuração

Para configurar o cliente é necessário registrar o Processor responsável por direcionar a solicitação para o Worker (que executará o comando num processo servidor). Para realizar o resgistro adicione o comando abaixo no *Startup.cs* do seu projeto

```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            // suas clausulas...
            SME.Background.Core.Orquestrador.Inicializar(services.BuildServiceProvider());
            SME.Background.Core.Orquestrador.Registrar<SME.Background.Hangfire.Processor>(new Background.Hangfire.Processor(Configuration, "SGP_Postgres"));
        }
```

É possível desativar o processamento em segundo plano, abaixo um exemplo de código de como pode ser feito
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            // suas clausulas...
            SME.Background.Core.Orquestrador.Inicializar(services.BuildServiceProvider());

            if (Configuration.GetValue<bool>("FF_BackgroundEnabled", false))
                SME.Background.Core.Orquestrador.Registrar<SME.Background.Hangfire.Processor>(new Background.Hangfire.Processor(Configuration, "SGP_Postgres"));
            else
                SME.Background.Core.Orquestrador.Desativar();
        }
```
Para solicitar um processamento simplesmente faça a chamada abaixo:

```csharp
        SME.Background.Core.Cliente.Executar(() => foo.Sum(1 + 1));
```

É possível utilizar a injeção de dependencias e direcionar o Worker para construir as classes e executar determinado método

```csharp
        SME.Background.Core.Cliente.Executar<Foo>(x => x.Sum(1 + 1));
```

### Algumas considerações:

 - Os métodos devem ser públicos;
 - Preferencialmente passe parâmetros de tipos primitivos, é possível a passagem de classe como parâmetro, porém entenda que essa classe será serializada e deserializada adicionando custo de processamento e pontos de falha
 - A injeção de dependencia deve ser do tipo Transient, para mais informações sugiro a [seguinte leitura](https://medium.com/volosoft/asp-net-core-dependency-injection-best-practices-tips-tricks-c6e9c67f9d96#9d32)
 - Separe um tempo de leitura para [as boas práticas do Hangfire](https://docs.hangfire.io/en/latest/best-practices.html)
 
## Backlog para futuras implementações

 - Criar método no cliente para obter os status por correlationID
 - Talvez seja interessante direcionar a chamada do cliente para um método interno da biblioteca para possibilitar sinalização de erros para ferramentas externas. Hoje só é possivel acompanhar os erros no dashboard
 - Disponibilizar a passagem de parâmetros como Intervalo de Pooling no BD, Quantidade de tentativas no caso de falha, nome do schema
