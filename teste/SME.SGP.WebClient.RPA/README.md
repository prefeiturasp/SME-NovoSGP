# SME.SGP.WebClient.RPA

Robot para automação dos testes das histórias e validação automática dos critérios de aceite.


## Preparação do ambiente

Para preparar o ambiente, pode seguir as instruções de instalação [aqui](https://github.com/robotframework/robotframework/blob/master/INSTALL.rst). 


## Organização do repositório

A estrutura de pastas segue o mesmo layout do projeto `SME.SGP.WebClient`, para facilitar a navegação e organização dos testes, contendo apenas como diferença os nomes dos scripts. Para cada página/componente, deve-se criar ao menos 1 arquivo com a extensão `.lib.robot` que é a library de comandos possiveis naquele escopo. 
Os testes são escritos com a extensão `.test.robot` para sinalizar que é um arquivo de cenários de teste. O nome dos arquivos devem conter o nome da pagina/componente, exemplo: `Login.js` possui como teste `Login.test.robot` e como biblioteca de comandos `Login.lib.robot`.
O arquivo `src/configuracao/variaveis.robot` possui as variáveis globais necessárias para a execução dos testes no ambiente local de desenvolvimento, caso precisa rodar em outro ambiente deve passar as variaveis por linha de comando como descrito neste [link](https://robotframework.org/robotframework/latest/RobotFrameworkUserGuide.html#variable-files).


## Executando os testes

Para executar um teste específico basta rodar o comando `robot` apontando para o script de teste específico, exemplo com o Login:

```
robot src/paginas/Login/Login.test.robot
```
