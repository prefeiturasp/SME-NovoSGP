# SME-NovoSGP - Front

---

Configuração inicial de ambiente de desenvolvimento para o Frontend

**1- Instalar Docker**

**2- Configurar variáveis de ambiente do backend**

Na raiz do projeto, criar um arquivo chamado sme-api.env, baseado no sme-api-sample.env

A chave do sentry, deverá ser obtida através dos canais de comunicação do projeto.

**3- Executar docker-compose**

Para ter o backend disponível para desenvolvimento, executar **raiz da solução**

obs.: lembrar de atualizar a solução via git para receber as últimas atualizações

```
docker-compose -f .\docker-compose.front.yml up --build
```

Com este comando, o backend com a base de dados estará disponível, com todos os scripts já aplicados.
