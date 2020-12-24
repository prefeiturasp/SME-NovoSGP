using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasRegistroPoa : ConsultasBase, IConsultasRegistroPoa
    {
        private readonly IRepositorioRegistroPoa repositorioRegistroPoa;
        private readonly IServicoEol servicoEOL;

        public ConsultasRegistroPoa(IRepositorioRegistroPoa repositorioRegistroPoa, IContextoAplicacao contextoAplicacao, IServicoEol servicoEOL) : base(contextoAplicacao)
        {
            this.repositorioRegistroPoa = repositorioRegistroPoa ?? throw new System.ArgumentNullException(nameof(repositorioRegistroPoa));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<PaginacaoResultadoDto<RegistroPoaDto>> ListarPaginado(RegistroPoaFiltroDto registroPoaFiltroDto)
        {
            if (string.IsNullOrEmpty(registroPoaFiltroDto.CodigoRf))
            {
                return new PaginacaoResultadoDto<RegistroPoaDto>();
            }

            PaginacaoResultadoDto<RegistroPoa> retornoquery =
                await repositorioRegistroPoa.ListarPaginado(registroPoaFiltroDto.CodigoRf,
                    registroPoaFiltroDto.DreId,
                    registroPoaFiltroDto.Bimestre,
                    registroPoaFiltroDto.UeId,
                    registroPoaFiltroDto.Titulo,
                    registroPoaFiltroDto.AnoLetivo,
                    Paginacao);

            var retornoPaginado = new PaginacaoResultadoDto<RegistroPoaDto>()
            {
                TotalPaginas = retornoquery.TotalPaginas,
                TotalRegistros = retornoquery.TotalRegistros
            };

            bool nenhumItemEncontrado = retornoquery.Items == null ||
                !retornoquery.Items.Any() ||
                retornoquery.Items.ElementAt(0).Id == 0;

            var listaRf = retornoquery.Items.Select(x => x.CodigoRf);

            var nomes = await servicoEOL.ObterListaNomePorListaRF(listaRf);

            return MapearListagem(retornoquery, retornoPaginado, nenhumItemEncontrado, nomes);
        }

        public RegistroPoaCompletoDto ObterPorId(long id)
        {
            var registro = repositorioRegistroPoa.ObterPorId(id);

            if (registro == null)
                return null;

            var professor = servicoEOL.ObterResumoProfessorPorRFAnoLetivo(registro.CodigoRf, registro.AnoLetivo).Result;

            return MapearParaDtoCompleto(registro, professor == null ? "Professor não encontrado" : professor.Nome);
        }

        private PaginacaoResultadoDto<RegistroPoaDto> MapearListagem(PaginacaoResultadoDto<RegistroPoa> retornoquery, PaginacaoResultadoDto<RegistroPoaDto> retornoPaginado, bool nenhumItemEncontrado, IEnumerable<ProfessorResumoDto> nomes)
        {
            retornoPaginado.Items = nenhumItemEncontrado ? null : retornoquery.Items.Select(registro =>
            {
                var professor = nomes.FirstOrDefault(resumo => resumo.CodigoRF.Equals(registro.CodigoRf));

                string nome = professor == null ? "Professor não encontrado" : professor.Nome;

                return MapearParaDto(registro, nome);
            });

            return retornoPaginado;
        }

        private RegistroPoaDto MapearParaDto(RegistroPoa registroPoa, string nome)
        {
            return new RegistroPoaDto
            {
                CodigoRf = registroPoa.CodigoRf,
                Descricao = registroPoa.Descricao,
                DreId = registroPoa.DreId,
                Nome = nome,
                Excluido = registroPoa.Excluido,
                Id = registroPoa.Id,
                AnoLetivo = registroPoa.AnoLetivo,
                Bimestre = registroPoa.Bimestre,
                Titulo = registroPoa.Titulo,
                UeId = registroPoa.UeId
            };
        }

        private RegistroPoaCompletoDto MapearParaDtoCompleto(RegistroPoa registroPoa, string nome)
        {
            return new RegistroPoaCompletoDto
            {
                AlteradoEm = registroPoa.AlteradoEm,
                AlteradoPor = registroPoa.AlteradoPor,
                AlteradoRF = registroPoa.AlteradoRF,
                CodigoRf = registroPoa.CodigoRf,
                CriadoEm = registroPoa.CriadoEm,
                CriadoPor = registroPoa.CriadoPor,
                CriadoRF = registroPoa.CriadoRF,
                Nome = nome,
                Descricao = registroPoa.Descricao,
                DreId = registroPoa.DreId,
                Excluido = registroPoa.Excluido,
                Id = registroPoa.Id,
                Bimestre = registroPoa.Bimestre,
                Titulo = registroPoa.Titulo,
                UeId = registroPoa.UeId
            };
        }
    }
}