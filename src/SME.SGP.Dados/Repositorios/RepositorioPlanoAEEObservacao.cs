using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPlanoAEEObservacao : RepositorioBase<PlanoAEEObservacao>, IRepositorioPlanoAEEObservacao
    {
        public RepositorioPlanoAEEObservacao(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PlanoAEEObservacaoDto>> ObterObservacoesPlanoPorId(long planoId, string usuarioRF)
        {
            var query = @"select pao.*
                            , u.id as UsuarioId, u.nome 
                        from plano_aee_observacao pao 
                         left join notificacao_plano_aee_observacao npao on npao.plano_aee_observacao_id = pao.id
                         left join notificacao n on n.id = npao.notificacao_id
                         left join usuario u on u.id = n.usuario_id
                        where not pao.excluido 
                          and pao.plano_aee_id = @planoId";

            var lookup = new Dictionary<long, PlanoAEEObservacaoDto>();
            await database.Conexao.QueryAsync<PlanoAEEObservacao, UsuarioNomeDto, PlanoAEEObservacaoDto>(query, 
                (planoAEE, usuario) =>
            {
                PlanoAEEObservacaoDto planoDto = null;
                if (!lookup.TryGetValue(planoAEE.Id, out planoDto))
                {
                    planoDto = new PlanoAEEObservacaoDto(planoAEE.Id, planoAEE.Observacao, planoAEE.CriadoRF == usuarioRF);
                    planoDto.Auditoria = (AuditoriaDto)planoAEE;

                    lookup.Add(planoAEE.Id, planoDto);
                }

                planoDto.AdicionaUsuario(usuario);

                return planoDto;
            }, new { planoId }, splitOn: "UsuarioId");

            return lookup.Values;
        }


    }
}
