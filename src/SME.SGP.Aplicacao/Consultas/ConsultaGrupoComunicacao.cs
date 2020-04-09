using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaGrupoComunicacao : ConsultasBase, IConsultaGrupoComunicacao
    {
        private readonly IRepositorioGrupoComunicacao repositorioGrupoComunicacao;

        public ConsultaGrupoComunicacao(
            IContextoAplicacao contextoAplicacao,
            IRepositorioGrupoComunicacao repositorioGrupoComunicacao) : base(contextoAplicacao)
        {
            this.repositorioGrupoComunicacao = repositorioGrupoComunicacao ?? throw new ArgumentNullException(nameof(repositorioGrupoComunicacao));
        }

        public async Task<IEnumerable<GrupoComunicacaoDto>> Listar(FiltroGrupoComunicacaoDto filtro)
        {
            var grupos = await repositorioGrupoComunicacao.Listar(filtro);
            if (grupos is null)
                throw new NegocioException("Nenhum grupo encontrado");
            return MapearParaDto(grupos);
        }

        public async Task<GrupoComunicacaoCompletoDto> ObterPorIdAsync(long id)
        {
            var grupo = await repositorioGrupoComunicacao.ObterPorIdAsync(id);

            if (grupo is null)
                throw new NegocioException("Grupo de comunicação não encontrado");
            return MapearPorIdParaDto(grupo);
        }

        private IEnumerable<GrupoComunicacaoDto> MapearParaDto(IEnumerable<GrupoComunicacaoCompletoRespostaDto> grupos)
        {
            return grupos.Select(g => new GrupoComunicacaoCompletoDto
            {
                Id = g.Id,
                Nome = g.Nome,
                AlteradoEm = g.AlteradoEm,
                AlteradoPor = g.AlteradoPor,
                AlteradoRF = g.AlteradoRF,
                CriadoEm = g.CriadoEm,
                CriadoPor = g.CriadoPor,
                CriadoRF = g.CriadoRF,
                CiclosEnsino = grupos.Where(w => w.CicloEnsinoId == g.CicloEnsinoId).Select(s => new CicloEnsinoDto { CodCicloEnsino = s.CicloEnsinoId, Descricao = s.CicloEnsino }),
                TiposEscola = grupos.Where(w => w.TipoEscolaId == g.TipoEscolaId).Select(s => new TipoEscolaDto { CodTipoEscola = s.TipoEscolaId, Descricao = s.TipoEscola })
            });
        }

        private GrupoComunicacaoCompletoDto MapearPorIdParaDto(IEnumerable<GrupoComunicacaoCompletoRespostaDto> grupo)
        {
            return grupo.Select(g => new GrupoComunicacaoCompletoDto
            {
                Id = g.Id,
                Nome = g.Nome,
                AlteradoEm = g.AlteradoEm,
                AlteradoPor = g.AlteradoPor,
                AlteradoRF = g.AlteradoRF,
                CriadoEm = g.CriadoEm,
                CriadoPor = g.CriadoPor,
                CriadoRF = g.CriadoRF,
                CiclosEnsino = grupo.Where(w => w.CicloEnsinoId == g.CicloEnsinoId).Select(s => new CicloEnsinoDto { CodCicloEnsino = s.CicloEnsinoId, Descricao = s.CicloEnsino }),
                TiposEscola = grupo.Where(w => w.TipoEscolaId == g.TipoEscolaId).Select(s => new TipoEscolaDto { CodTipoEscola = s.TipoEscolaId, Descricao = s.TipoEscola })
            }).FirstOrDefault();
        }
    }
}