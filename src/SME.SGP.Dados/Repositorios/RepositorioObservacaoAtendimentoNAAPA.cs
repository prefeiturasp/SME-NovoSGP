using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObservacaoAtendimentoNAAPA : RepositorioBase<EncaminhamentoNAAPAObservacao>, IRepositorioObservacaoEncaminhamentoNAAPA
    {
        public RepositorioObservacaoAtendimentoNAAPA(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {

        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, string usuarioLogadoRf, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>();
            var sql = @$"select 
                             id as IdObservacao,
                             observacao as Observacao,
                             CASE
                                WHEN Criado_RF = @usuarioId THEN true
                                ELSE false
                             end Proprietario,
                             alterado_em as AlteradoEm,
                             Alterado_Por as AlteradoPor,
                             Alterado_RF as AlteradoRF,
                             Criado_Em as CriadoEm,
                             Criado_Por as CriadoPor,
                             Criado_RF as CriadoRF
                            from encaminhamento_naapa_observacao 
                        where not excluido  and encaminhamento_naapa_id = @encaminhamentoNAAPAId ";
            
            var observacoesConsulta = await database.Conexao.QueryAsync<AtendimentoNAAPAObservacoesConsultaDto>(sql,new { encaminhamentoNAAPAId, usuarioId = usuarioLogadoRf },commandTimeout: 300);
            var observacoes = MapearAuditoria(observacoesConsulta);

            if (paginacao.EhNulo() || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);


            var retornoPaginado = new PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>
            {
                TotalRegistros = observacoes.Any() ? observacoes.Count : 0
            };

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            retornoPaginado.Items = paginacao.QuantidadeRegistros > 0
                                    ? observacoes
                                        .Skip(paginacao.QuantidadeRegistrosIgnorados)
                                        .Take(paginacao.QuantidadeRegistros)
                                    : observacoes;

            return retornoPaginado;
        }

        private List<AtendimentoNAAPAObservacoesDto> MapearAuditoria(IEnumerable<AtendimentoNAAPAObservacoesConsultaDto> observacoes)
        {
            var lista = new List<AtendimentoNAAPAObservacoesDto>(); 
            foreach (var item in observacoes)
            {
                var obs = new AtendimentoNAAPAObservacoesDto 
                {
                    Id = item.IdObservacao,
                    Observacao = item.Observacao,
                    Proprietario = item.Proprietario,
                    Auditoria = new AuditoriaDto
                    {
                        AlteradoEm = item.AlteradoEm,
                        AlteradoPor = item.AlteradoPor,
                        AlteradoRF = item.AlteradoRF,
                        CriadoEm = item.CriadoEm,
                        CriadoPor = item.CriadoPor,
                        CriadoRF = item.CriadoRF
                    }
                };
                lista.Add(obs);
            }

            return lista;
        }
    }
}
