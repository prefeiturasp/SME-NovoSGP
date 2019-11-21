using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasAtribuicaoEsporadica : ConsultasBase, IConsultasAtribuicaoEsporadica
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IServicoEOL servicoEOL;

        public ConsultasAtribuicaoEsporadica(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica, IServicoEOL servicoEOL, IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>> Listar(FiltroAtribuicaoEsporadicaDto filtro)
        {
            var retornoConsultaPaginada = await repositorioAtribuicaoEsporadica.ListarPaginada(Paginacao, filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.CodigoRF);

            var retorno = new PaginacaoResultadoDto<AtribuicaoEsporadicaDto>
            {
                TotalPaginas = retornoConsultaPaginada.TotalPaginas,
                TotalRegistros = retornoConsultaPaginada.TotalRegistros
            };

            bool nenhumItemEncontrado = retornoConsultaPaginada.Items == null ||
               !retornoConsultaPaginada.Items.Any() ||
               retornoConsultaPaginada.Items.ElementAt(0).Id == 0;

            retorno.Items = nenhumItemEncontrado
                ? null
                : retornoConsultaPaginada.Items.Select(x => EntidadeParaDto(x)).ToList();

            return retorno;
        }

        public AtribuicaoEsporadicaDto ObterPorId(long id)
        {
            var atribuicaoEsporadica = repositorioAtribuicaoEsporadica.ObterPorId(id);

            if (atribuicaoEsporadica is null)
                return null;

            return EntidadeParaDto(atribuicaoEsporadica);
        }

        private AtribuicaoEsporadicaDto EntidadeParaDto(AtribuicaoEsporadica entidade)
        {
            var professorResumo = servicoEOL.ObterResumoProfessorPorRFAnoLetivo(entidade.ProfessorRf, entidade.DataInicio.Year).Result;

            return new AtribuicaoEsporadicaDto
            {
                AnoLetivo = entidade.DataInicio.Year,
                DataFim = entidade.DataFim,
                DataInicio = entidade.DataInicio,
                DreId = entidade.DreId,
                Excluido = entidade.Excluido,
                Id = entidade.Id,
                Migrado = entidade.Migrado,
                ProfessorNome = professorResumo != null ? professorResumo.Nome : "Professor não encontrado",
                ProfessorRf = entidade.ProfessorRf,
                UeId = entidade.UeId
            };
        }
    }
}