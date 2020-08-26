using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasAtribuicaoEsporadica : ConsultasBase, IConsultasAtribuicaoEsporadica
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IServicoEol servicoEOL;

        public ConsultasAtribuicaoEsporadica(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica, IServicoEol servicoEOL, IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>> Listar(FiltroAtribuicaoEsporadicaDto filtro)
        {
            var retornoConsultaPaginada = await repositorioAtribuicaoEsporadica.ListarPaginada(Paginacao, filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.ProfessorRF);

            var retorno = new PaginacaoResultadoDto<AtribuicaoEsporadicaDto>
            {
                TotalPaginas = retornoConsultaPaginada.TotalPaginas,
                TotalRegistros = retornoConsultaPaginada.TotalRegistros
            };

            bool nenhumItemEncontrado = retornoConsultaPaginada.Items == null ||
               !retornoConsultaPaginada.Items.Any() ||
               retornoConsultaPaginada.Items.ElementAt(0).Id == 0;

            retorno.Items = !nenhumItemEncontrado ? ListaEntidadeParaListaDto(retornoConsultaPaginada.Items) : null;

            return retorno;
        }

        public AtribuicaoEsporadicaCompletaDto ObterPorId(long id)
        {
            var atribuicaoEsporadica = repositorioAtribuicaoEsporadica.ObterPorId(id);

            if (atribuicaoEsporadica is null)
                return null;

            return EntidadeParaDtoCompleto(atribuicaoEsporadica);
        }

        private AtribuicaoEsporadicaDto EntidadeParaDto(AtribuicaoEsporadica entidade, bool buscarNome = true, string nomeProfessor = "")
        {
            if (buscarNome)
            {
                var professorResumo = servicoEOL.ObterResumoProfessorPorRFAnoLetivo(entidade.ProfessorRf, entidade.DataInicio.Year).Result;
                nomeProfessor = professorResumo != null ? professorResumo.Nome : "Professor não encontrado";
            }

            return new AtribuicaoEsporadicaDto
            {
                AnoLetivo = entidade.DataInicio.Year,
                DataFim = entidade.DataFim,
                DataInicio = entidade.DataInicio,
                DreId = entidade.DreId,
                Excluido = entidade.Excluido,
                Id = entidade.Id,
                Migrado = entidade.Migrado,
                ProfessorNome = nomeProfessor,
                ProfessorRf = entidade.ProfessorRf,
                UeId = entidade.UeId
            };
        }

        private AtribuicaoEsporadicaCompletaDto EntidadeParaDtoCompleto(AtribuicaoEsporadica entidade)
        {
            var professorResumo = servicoEOL.ObterResumoProfessorPorRFAnoLetivo(entidade.ProfessorRf, entidade.DataInicio.Year).Result;

            return new AtribuicaoEsporadicaCompletaDto
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
                UeId = entidade.UeId,
                AlteradoEm = entidade.AlteradoEm,
                AlteradoPor = entidade.AlteradoPor,
                AlteradoRF = entidade.AlteradoRF,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                CriadoRF = entidade.CriadoRF
            };
        }

        private IEnumerable<AtribuicaoEsporadicaDto> ListaEntidadeParaListaDto(IEnumerable<AtribuicaoEsporadica> entidades)
        {
            var professores = servicoEOL.ObterListaNomePorListaRF(entidades.Select(x => x.ProfessorRf)).Result;

            return entidades.Select(x => EntidadeParaDto(x, false, ObterNomeProfessor(professores, x.ProfessorRf)));
        }

        private string ObterNomeProfessor(IEnumerable<ProfessorResumoDto> professores, string codigoRF)
        {
            var professor = professores.FirstOrDefault(p => p.CodigoRF == codigoRF);

            if (professor == null)
                return "Professor não encontrado";

            return professor.Nome;
        }
    }
}