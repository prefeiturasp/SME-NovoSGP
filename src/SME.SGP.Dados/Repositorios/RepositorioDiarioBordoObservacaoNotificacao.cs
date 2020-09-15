using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacaoNotificacao : IRepositorioDiarioBordoObservacaoNotificacao
    {
        private readonly ISgpContext database;
        
        public RepositorioDiarioBordoObservacaoNotificacao(ISgpContext database)
        {
            this.database = database;         
        }       

        public async Task Salvar(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.InsertAsync(notificacao);
        }

    }
}
