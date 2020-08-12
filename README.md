# README
---

Modelo de README do **SGP - Sistema de Gestão Pedagógica** no [GitHub](https://github.com/prefeiturasp/SME-NovoSGP).

## Índice de Conteúdo
[[_TOC_]]


# **Introdução**
O Sistema de Gestão Pedagógica (SGP) é o novo sistema de gestão pedagógica dos gestores e professores de toda a rede de ensino do Município de São Paulo, como parte de uma proposta de transformação digital da Secretaria de Educação. O Sistema é utilizado diariamente por milhares de professores e funcionários da Rede Pública, trazendo alto impacto no ambiente escolar.


## Visão do Produto:
---

Para os professores da rede municipal de ensino que precisam realizar registros pedagógicos o SGP é um sistema de gestão que facilita estes registros e seu acompanhamento. Ao contrário do SGP Legado, nosso produto oferece interfaces amigáveis para o usuário e diversas automações de processos com envio de feedbacks em tempo real.

## É / Não é / Faz / Não faz:
---

- **É**: Sistema para registros pedagógicos.
- **Não é**: Repositório de arquivos, sistema de gestão de usuários e gestão administrativa.
- **Faz**: Registros de aulas e frequência, planejamento das turmas e aulas, lançamento de notas e fechamento bimestral, além da geração de documentos legais (boletim, atas) e demais relatórios pedagógicos.
- **Não faz**: Atribuição de professores e alunos, cadastro de turmas e demais cadastros administrativos.

## Objetivos de Negócio:
---

- Realizar registros pedagógicos (frequência, atividades e notas).
- Fornecedor relatórios pedagógicos e de acompanhamento das atividades desenvolvidas nas escolas, além de relatórios obrigatórios previstos em legislação como atas e histórico escolar.
- Registrar a evolução dos estudantes em projetos de apoio e evolução de aprendizagem.
- Registrar e controlar os períodos escolares e seus eventos dentro do ano letivo.


## Personas:
---

- **ATE**: Auxiliar Técnico de Educação, é um funcionário da UE que ajuda na organização escolar e faz serviço de monitoria.
- **AD**: Assistente de Diretor de Escola, é equivalente a um vice-diretor, que tem atribuições de gestão mais voltadas a parte administrativa, além de ser o substituto oficial do Diretor, quando necessário. Cada UE pode ter de 1 até 3 AD’s.
- **ADM COTIC**: É o usuário com perfil de Administrador na COTIC, que é a área de TI da SME.
- **ADM DRE**: É o usuário que normalmente trabalha no setor pedagógico ou de TI da DRE e é responsável pelo suporte aos usuários de todas as UE’s da sua DRE.
- **ADM SME/COPED**: É o usuário com perfil de Administrador na COPED, que é a principal área de negócio do SGP.
- **ADM UE**: É responsável pelo suporte aos demais usuários de uma UE.
- **CEFAI**: É o Centro de Formação e Acompanhamento à Inclusão, responsável por toda a coordenação dos programas de educação especial.
- **Coordenador SME**: Qualquer coordenador de área da SME poderá ter este perfil para realizar consultas no SGP.
- **COPED**: É a Coordenadoria Pedagógica, a principal área de negócio do SGP e responsável por toda a parte pedagógica do ensino público municipal.
- **COPED Básico**: É um perfil para usuários da COPED que precisam apenas de informações básicas.
- **CP**: Coordenador Pedagógico, é um cargo de gestão da UE que é responsável pelo acompanhamento pedagógico. Cada UE pode ter de 1 até 3 CP’s.
- **DIEE**: É a Divisão de Educação Especial, uma subdivisão da COPED que é especializada nas ações pedagógicas da educação inclusiva.
- **DIEFEM**: É a Divisão de Ensino Fundamental e Médio.
- **DIEJA**: É a Divisão de Educação de Jovens e Adultos.
- **DIPED**: É a Divisão Pedagógica da DRE, equivale a COPED na SME.
- **Diretor**: É o principal gestor da UE e tem atribuições administrativas na sua grande maioria, mas também apoia e acompanha o CP no pedagógico.
- **Diretor DIPED**: É o Diretor da Divisão Pedagógica da DRE, é o segundo cargo mais importante da DRE abaixo apenas do Diretor Regional.
- **Diretor Regional**: É o cargo mais importante da DRE e equivale a um secretário de educação regional.
- **DRE Básico**: É um perfil para usuários da DRE que precisam apenas de informações básicas.
- **NAAPA**: É o Núcleo de Apoio e Acompanhamento para Aprendizagem, um setor da DRE responsável principalmente pelo acompanhamento de alunos em situação de vulnerabilidade social.
- **NTA**: É o Núcleo Técnico de Avaliação, o setor da SME que é responsável pela aplicação e resultados de avaliações gerais da rede, como a Prova e Provinha São Paulo, simulado do ENEM, entre outras.
- **NTC**: Núcleo Técnico de Currículo é um setor da SME responsável pela avaliação dos professores e dos processos aplicados.
- **NTC – NAAPA**: É uma subdivisão do NTC.
- **PAAI**: Professor de Apoio e Acompanhamento à Inclusão.
- **PAEE**: Professor do Atendimento Educação Especializado.
- **PAP**: Professor de Apoio Pedagógico, que nada mais é que um professor de reforço ou recuperação.
- **POA**: O Professor Orientador de Área é responsável por orquestrar ações de formação com os demais professores da sua UE nos componentes de regência de classe, língua portuguesa e matemática.
- **POEI**: Professor Orientador de Educação Integral.
- **Professor**: Qualquer servidor ativo na rede que tenha um cargo de professor (são mais de 30).
- **Professor CJ**: É o professor de módulo ou professor substituto. Também tem um cargo de professor, porém ficou sem atribuição nas turmas regulares e então está atribuído a um “módulo” que nada mais é um componente curricular. Todas as UE’s tem um conjunto de professores de módulo.
- **Professor Readaptado**: É um professor que tem laudo médico do tipo R (trabalho), mas não em sala de aula, ou seja, este professor faz apenas trabalhos administrativos.
- **Secretário**: É o usuário responsável pela secretaria da UE, ou seja, pela recepção de documentos, encaminhamentos e matrículas.
- **Supervisor**: É o nível mais alto na carreira “pedagógica” passível de concurso. Cada Supervisor Escolar é responsável por algumas UE’s da DRE, respondendo pela parte pedagógica, administrativa e jurídica.
- **Supervisor – Consulta**: É um perfil para os supervisores que apresenta todas as UE’s da DRE, é utilizado principalmente no caso de plantão, onde pode ocorrer a necessidade de consulta de alguma informação de unidades que não estão sob a jurisdição do supervisor plantonista.
- **Supervisor Técnico**: É o coordenador dos supervisores da DRE.

## Funcionalidades:
---

- **Diário de classe**: É a parte do sistema responsável pelos registros periódicos (de preferência "diários"), como frequência, plano de aula e notas.
- **Planejamento**: Neste módulo estão as funcionalidades relacionadas com o planejamento da unidade escolar, suas turmas e componentes curriculares.
- **Fechamento**: As atividades que estão relacionadas ao encerramento dos bimestres ou do ano estão contempladas neste módulo, como o próprio fechamento de bimestre, análise de pendências e conselho de classe.
- **Calendário escolar**: As funcionalidades relacionadas a calendário servem para indicar os limites de datas para registros pedagógicos e demais ocorrências nas unidades escolares. Também é neste módulo que se encontra o "Calendário do Professor" uma das principais funcionalidades do sistema, onde são registradas as aulas e avaliações das turmas por componente curricular.
- **Gestão**: Neste módulo se encontram algumas funcionalidades que são mais "administrativas", porém com grande impacto nos demais módulos, como as atribuições de CJ e de Supervisores.
- **Relatórios**: Módulo destinado a geração de diversos relatórios e execução de registros pedagógicos, como os relacionados ao PAP - Projeto de Apoio Pedagógico.
- **Configurações**: Parâmetros e configurações restritas à SME estão neste módulo, além da funcionalidade de reset de senha dos usuários.

## Jornadas:
---

### Login - Fluxo Resumido
![1. Fluxo Resumido_login.png](http://amcom.educacao.ws/images/1-fluxo-resumido-login.png)

### Login - Fluxo Completo
![2. Fluxo Completo_Login.png](http://amcom.educacao.ws/images/2-fluxo-completo-login.png)

### Calendário
![3. Calendário.png](http://amcom.educacao.ws/images/3-calendario.png)

### Criação de aula
![4. Criação de aula.png](http://amcom.educacao.ws/images/4-criacao-aula.png)

### Plano de Aula
![5. Plano de Aula.png](http://amcom.educacao.ws/images/5-plano-aula.png)

### Frequência
![6. Frequência.png](http://amcom.educacao.ws/images/6-frequencia.png)

### Atribuição CJ
![7. Atribuição CJ.png](http://amcom.educacao.ws/images/7-atribuicao-cj.png)

### Notificação
![8. Notificacao.png](http://amcom.educacao.ws/images/8-notificacao.png)

## Roadmap:
---

O Roadmap do Produto é vivo e dinâmico. Essa é a foto atual do plano de evolução do produto, com previsão de entrega da Release 4 em Setembro, incluindo as Unidades Escolares do Infantil no sistema.

![roadmap.jfif](http://amcom.educacao.ws/images/roadmap.jfif)
## Configuração do Projeto
---

Configuração inicial de ambiente de desenvolvimento

Esta solução contém o projet de [API](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.Api) (backend) e [Front](https://github.com/prefeiturasp/SME-NovoSGP/tree/master/src/SME.SGP.WebClient)

Cada projeto contém seu próprio readme e nestes contem instruções para conseguir um ambiente de desenvolvimento local.

Para executar toda a solução em um ambiente local deverá:

**1 - Instalar Docker**

**2 - Configurar variáveis de ambiente**

Acessar o readme do projeto de Front e seguir a configuração de variáveis de ambiente Acessar o readme do projeto de Back e seguir a configuração de variáveis de ambiente

**3 - Docker Compose**

Executar o comando na raiz da solução

`docker-compose up --build`
