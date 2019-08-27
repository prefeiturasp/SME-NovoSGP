# SME Novo SGP

Configuração inicial de ambiente de desenvolvimento

Esta solução contém o projet de [API](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.Api "API") (backend) e [Front](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.WebClient "Front")

Cada projeto contém seu próprio readme e nestes contem instruções para conseguir um ambiente de desenvolvimento local.

Para executar toda a solução em um ambiente local deverá:

**1- Instalar docker**

**2- Configurar variáveis de ambiente**

Acessar o readme do projeto de [Front](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.WebClient "Front") e seguir a configuração de variáveis de ambiente

No projeto de Front (src/SME.SGP.WebClient), deverá ser criado um arquivo chamado .env , baseado no arquivo sample.env

**3- Docker Compose**

Executar o comando na **raiz da solução**

`docker-compose up --build`

