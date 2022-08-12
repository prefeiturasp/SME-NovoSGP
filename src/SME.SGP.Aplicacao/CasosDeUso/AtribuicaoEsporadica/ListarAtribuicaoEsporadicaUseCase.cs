using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAtribuicaoEsporadicaUseCase : ConsultasBase, IListarAtribuicaoEsporadicaUseCase
    {
        private readonly IMediator mediator;

        public ListarAtribuicaoEsporadicaUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator;
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>> Executar(FiltroAtribuicaoEsporadicaDto filtro)
        {
            var lstAtribuicoes = await mediator.Send(
                new ListarAtribuicaoEsporadicaPorFiltrosQuery(
                    Paginacao,
                    filtro.AnoLetivo,
                    filtro.DreId,
                    filtro.UeId,
                    filtro.ProfessorRF));

            var retorno = new PaginacaoResultadoDto<AtribuicaoEsporadicaDto>
            {
                TotalPaginas = lstAtribuicoes.TotalPaginas,
                TotalRegistros = lstAtribuicoes.TotalRegistros
            };

            bool nenhumItemEncontrado = lstAtribuicoes.Items == null ||
              !lstAtribuicoes.Items.Any() ||
              lstAtribuicoes.Items.ElementAt(0).Id == 0;

            retorno.Items = !nenhumItemEncontrado ? await ListaEntidadeParaListaDto(lstAtribuicoes.Items) : null;

            return retorno;
        }

        private async Task<IEnumerable<AtribuicaoEsporadicaDto>> ListaEntidadeParaListaDto(IEnumerable<Dominio.AtribuicaoEsporadica> entidades)
        {
            var professores = await mediator.Send(new ObterListaNomePorListaRFQuery(entidades.Select(x => x.ProfessorRf)));

            var lstDto = new List<AtribuicaoEsporadicaDto>();

            foreach (var entidade in entidades)
            {
                var nomeProfessor = ObterNomeProfessor(professores, entidade.ProfessorRf);

                var itemDto = await EntidadeParaDto(entidade, false, nomeProfessor);

                lstDto.Add(itemDto);
            }

            return lstDto;
        }

        private async Task<AtribuicaoEsporadicaDto> EntidadeParaDto(AtribuicaoEsporadica entidade, bool buscarNome = true, string nomeProfessor = "")
        {
            if (buscarNome)
            {
                var professorResumo = await mediator.Send(new ObterResumoProfessorPorRFAnoLetivoQuery(entidade.ProfessorRf, entidade.DataInicio.Year));
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

        private string ObterNomeProfessor(IEnumerable<ProfessorResumoDto> professores, string codigoRF)
        {
            var professor = professores.FirstOrDefault(p => p.CodigoRF == codigoRF);

            if (professor == null)
                return "Professor não encontrado";

            return professor.Nome;
        }
    }
}
