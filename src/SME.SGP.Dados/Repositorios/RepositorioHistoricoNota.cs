﻿using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoNota : RepositorioBase<HistoricoNota>, IRepositorioHistoricoNota
    {
        public RepositorioHistoricoNota(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public override async Task<long> SalvarAsync(HistoricoNota entidade)
        {
            if (string.IsNullOrEmpty(entidade.CriadoRF))
                entidade.CriadoRF = database.UsuarioLogadoRF;
            if (string.IsNullOrEmpty(entidade.CriadoPor))
                entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;

            entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
            await AuditarAsync(entidade.Id, "I");

            return entidade.Id;
        }
    }
}
