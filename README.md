# SME Novo SGP

O Sistema de Gestão Pedagógica (SGP) é o novo sistema de gestão pedagógica dos gestores e professores de toda rede Municipal de ensino do município de São Paulo como parte de uma transformação digital da Secretaria de Educação. O Sistema é utilizado diariamente por milhares professores e alunos da rede pública tendo alto impacto no ambiente escolar.

Configuração inicial de ambiente de desenvolvimento

Esta solução contém o projet de [API](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.Api "API") (backend) e [Front](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.WebClient "Front")

Cada projeto contém seu próprio readme e nestes contem instruções para conseguir um ambiente de desenvolvimento local.

Para executar toda a solução em um ambiente local deverá:

**1- Instalar docker**

**2- Configurar variáveis de ambiente**

Acessar o readme do projeto de [Front](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.WebClient "Front") e seguir a configuração de variáveis de ambiente
Acessar o readme do projeto de [Back](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.Api "Back") e seguir a configuração de variáveis de ambiente

**3- Docker Compose**

Executar o comando na **raiz da solução**


`docker-compose up --build`

