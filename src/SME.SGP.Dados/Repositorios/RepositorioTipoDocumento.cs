using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoDocumento : IRepositorioTipoDocumento
    {
        protected readonly ISgpContext database;

        public RepositorioTipoDocumento(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<TipoDocumentoDto>> ListarDocumentosPorPerfil(string[] perfis)
        {
            List<TipoDocumentoDto> retorno = new List<TipoDocumentoDto>();

            var query = @"select distinct tipo_documento.id, tipo_documento.descricao as tipodocumento from tipo_documento 
                          inner join classificacao_documento on tipo_documento.id = classificacao_documento.tipo_documento_id
                          where classificacao_documento.descricao = ANY(@perfis);

                        select Id, descricao as Classificacao, tipo_documento_id as TipoDocumentoId from classificacao_documento 
                            where descricao = ANY(@perfis);";

            using (var multi = await database.Conexao.QueryMultipleAsync(query, new { perfis }))
            {
                retorno = multi.Read<TipoDocumentoDto>().ToList();
                var classificacoes = multi.Read<ClassificacaoDocumentoDto>().ToList();

                retorno.ForEach(td => td.Classificacoes.AddRange(classificacoes.Where(c => c.TipoDocumentoId == td.Id)));
            }

            return retorno;
        }

        public async Task<IEnumerable<TipoDocumentoDto>> ListarTipoDocumentoClassificacao()
        {
            List<TipoDocumentoDto> retorno = new List<TipoDocumentoDto>();

            var query = @"select distinct tipo_documento.id, tipo_documento.descricao as tipodocumento from tipo_documento 
                          inner join classificacao_documento on tipo_documento.id = classificacao_documento.tipo_documento_id;

                        select Id, descricao as Classificacao, tipo_documento_id as TipoDocumentoId from classificacao_documento; ";

            using (var multi = await database.Conexao.QueryMultipleAsync(query))
            {
                retorno = multi.Read<TipoDocumentoDto>().ToList();
                var classificacoes = multi.Read<ClassificacaoDocumentoDto>().ToList();

                retorno.ForEach(td => td.Classificacoes.AddRange(classificacoes.Where(c => c.TipoDocumentoId == td.Id)));
            }

            return retorno;
        }
    }
}