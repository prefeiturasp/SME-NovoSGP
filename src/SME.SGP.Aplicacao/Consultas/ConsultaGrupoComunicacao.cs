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
            if (grupos is null || !grupos.Any())
                throw new NegocioException("Nenhum grupo encontrado");
            return MapearParaDto(grupos);
        }

        public async Task<GrupoComunicacaoCompletoDto> ObterPorIdAsync(long id)
        {
            var grupo = await repositorioGrupoComunicacao.ObterPorIdAsync(id);

            if (grupo is null || !grupo.Any())
                throw new NegocioException("Grupo de comunicação não encontrado");
            return MapearPorIdParaDto(grupo);
        }

        private static IEnumerable<GrupoComunicacaoCompletoDto> ConverterParaDto(IEnumerable<GrupoComunicacaoCompletoRespostaDto> grupos)
        {
            var gruposDistintos = grupos
                        .Select(s => new
                        {
                            s.Nome,
                            s.Id,
                            s.AlteradoEm,
                            s.AlteradoPor,
                            s.AlteradoRF,
                            s.CriadoEm,
                            s.CriadoPor,
                            s.CriadoRF
                        })
                        .Distinct();

            return gruposDistintos.Select(g => new GrupoComunicacaoCompletoDto
            {
                Id = g.Id,
                Nome = g.Nome,
                AlteradoEm = g.AlteradoEm,
                AlteradoPor = g.AlteradoPor,
                AlteradoRF = g.AlteradoRF,
                CriadoEm = g.CriadoEm,
                CriadoPor = g.CriadoPor,
                CriadoRF = g.CriadoRF,
                CiclosEnsino = grupos.Where(w => w.Id == g.Id && w.CodCicloEnsino.HasValue && w.IdCicloEnsino.HasValue).Select(s => new CicloEnsinoDto { Id = s.IdCicloEnsino.Value, CodCicloEnsino = s.CodCicloEnsino.Value, Descricao = s.CicloEnsino }),
                TiposEscola = grupos.Where(w => w.Id == g.Id && w.CodTipoEscola.HasValue && w.IdTipoEscola.HasValue).Select(s => new TipoEscolaDto { Id = s.IdTipoEscola.Value, CodTipoEscola = s.CodTipoEscola.Value, Descricao = s.TipoEscola })
            });
        }

        private IEnumerable<GrupoComunicacaoDto> MapearParaDto(IEnumerable<GrupoComunicacaoCompletoRespostaDto> grupos)
        {
            return ConverterParaDto(grupos);
        }

        private GrupoComunicacaoCompletoDto MapearPorIdParaDto(IEnumerable<GrupoComunicacaoCompletoRespostaDto> grupos)
        {
            return ConverterParaDto(grupos).FirstOrDefault();
        }
    }
}