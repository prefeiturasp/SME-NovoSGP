# SME-NovoSGP-Api

Configuração inicial de ambiente de desenvolvimento

**1- Instalar Docker**

**2- DataBase - PostgreSQL**
- No diretório base, existe o arquivo de docker-compose o qual contém a configuração para executar um postgreSQL local. Para executá-lo, em um prompt de comando, executar: ```
docker-compose up ```
 
**3- Variáveis de ambiente**
> Informações sensíveis como string de conexão deverão ser mantidas em variáveis de ambiente. 

-  Adicionar nas variáveis de ambiente
   - *Nome da variável:* ConnectionStrings__SGP-Postgres
   -  *Valor da variável:*  User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=sgp_db;Pooling=true;
   
   - *Nome da variável:* Sentry__DSN
   -  *Valor da variável:*  xx;
