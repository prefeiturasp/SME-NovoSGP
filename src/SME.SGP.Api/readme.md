# SME-NovoSGP-Api
Este projeto visa servir de backend com recursos Rest para o novo projeto SGP.

------------



Configuração inicial de ambiente de desenvolvimento

**1- Instalar Docker**

##### BASE DE DADOS

Para ter a base de dados disponível para desenvolvimento, executar o comando docker na **raiz da solução**
Obs.: Lembrar de atualizar a solução via git, branch development, para receber as últimas atualizações
```
docker-compose -f .\docker-compose.database.yml up
```

Com este comando, a base de dados estará disponível, com todos os scripts já aplicados.

###### Variáveis de ambiente
> Todas as variáveis de configuração são mantidas como variáveis de ambiente. 

**Modificar o arquivo /configuracoes/criar-variaveis-ambiente.ps1 com a chave do Sentry obtida pelos canais de comunicação do projeto, substituindo pelo xxx.

1- Procurar por poweshell no inciar. 

2- Clicar com botão direito e executar como administrador

3- Executar o comando: Set-ExecutionPolicy Unrestricted 

4- digitar s ou y (dependendo do idioma do sistema operacional)

	** O comando acima é para habilitar scripts não criados na maquina local e sem certificado a serem executados;
	
5- Navegar até a pasta do projeto /configuracoes pelo powershell

6- Executar o comando & .\criar-variaveis-ambiente.ps1 

7- Executar o comando: Set-ExecutionPolicy Restricted 

8- digitar s ou y (dependendo do idioma do sistema operacional)

	** O comando acima é para desabilitar scripts de serem executados;


