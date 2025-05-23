﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAluno : RepositorioBase<AcompanhamentoAluno>, IRepositorioAcompanhamentoAluno
    {
        public RepositorioAcompanhamentoAluno(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }
    }
}
