using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCadastroAcessoABAEConsulta : RepositorioBase<CadastroAcessoABAE>, IRepositorioCadastroAcessoABAEConsulta
    {
        public RepositorioCadastroAcessoABAEConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {}
    }
}