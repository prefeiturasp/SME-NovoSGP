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
using ClassificacaoDocumento = SME.SGP.Dominio.Enumerados.ClassificacaoDocumento;
using TipoDocumento = SME.SGP.Dominio.Enumerados.TipoDocumento;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoDocumento : IRepositorioTipoDocumento
    {
        protected readonly ISgpContext database;
        
        public RepositorioTipoDocumento(ISgpContext database)
        {
            this.database = database;
        }
        
        private string ConsultaTipoDocumentoClassificacao =>
            @"select distinct td.id as TipoDocumentoId, 
                                          td.descricao as TipoDocumentoNome,
                                          cd.id as ClassificacaoId,
                                          cd.descricao as ClassificacaoNome
                          from tipo_documento td 
                            inner join classificacao_documento cd on td.id = cd.tipo_documento_id";

        public async Task<IEnumerable<TipoDocumentoDto>> ListarDocumentosPorPerfil(string[] perfis)
        {
            var query = $"{ConsultaTipoDocumentoClassificacao} where (cd.descricao = ANY(@perfis) and td.id = @planoDeTrabalho);";

            var parametros = new
            {
                perfis,
                planoDeTrabalho = (long)TipoDocumento.PlanoTrabalho, 
                documentos = (long)TipoDocumento.Documento
            };

            var tipoDocumentoDtos = await database.Conexao.QueryAsync<TipoDocumentoCompletoDto>(query, parametros);
            
            var tipoDocumentoAgrupados = ObterTipoDocumentoAgrupados(tipoDocumentoDtos);

            return tipoDocumentoAgrupados;
        }

        public async Task<IEnumerable<TipoDocumentoDto>> ListarTipoDocumentoClassificacao()
        {
            var query = ConsultaTipoDocumentoClassificacao;

            var tipoDocumentoDtos = await database.Conexao.QueryAsync<TipoDocumentoCompletoDto>(query);
            
            var tipoDocumentoAgrupados = ObterTipoDocumentoAgrupados(tipoDocumentoDtos);

            return tipoDocumentoAgrupados;
        }

        private List<TipoDocumentoDto> ObterTipoDocumentoAgrupados(IEnumerable<TipoDocumentoCompletoDto> tiposDocumentos)
        {
            return tiposDocumentos.GroupBy(g => new
                { 
                    g.TipoDocumentoId,
                    g.TipoDocumentoNome
                }, (key, group) => 
                    new TipoDocumentoDto { 
                        Id = key.TipoDocumentoId,
                        TipoDocumento = key.TipoDocumentoNome,
                        Classificacoes = group.Select(s=>
                                new ClassificacaoDocumentoDto
                                {
                                    Id = s.ClassificacaoId,
                                    Classificacao = s.ClassificacaoNome,
                                    TipoDocumentoId = s.TipoDocumentoId
                                })
                            .OrderBy(o=> o.Classificacao)
                            .ToList()
                    })
                .OrderBy(o=> o.TipoDocumento)
                .ToList();
        }
    }
}