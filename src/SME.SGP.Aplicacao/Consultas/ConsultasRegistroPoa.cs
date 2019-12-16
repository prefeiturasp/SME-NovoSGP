using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasRegistroPoa : ConsultasBase, IConsultasRegistroPoa
    {
        private readonly IRepositorioRegistroPoa repositorioRegistroPoa;

        public ConsultasRegistroPoa(IRepositorioRegistroPoa repositorioRegistroPoa, IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioRegistroPoa = repositorioRegistroPoa ?? throw new System.ArgumentNullException(nameof(repositorioRegistroPoa));
        }

        public async Task<PaginacaoResultadoDto<RegistroPoaDto>> ListarPaginado(RegistroPoaFiltroDto registroPoaFiltroDto)
        {
            PaginacaoResultadoDto<RegistroPoa> retornoquery = await repositorioRegistroPoa.ListarPaginado(registroPoaFiltroDto.CodigoRf, registroPoaFiltroDto.DreId, registroPoaFiltroDto.Mes, registroPoaFiltroDto.UeId, registroPoaFiltroDto.Titulo, Paginacao);

            var retornoPaginado = new PaginacaoResultadoDto<RegistroPoaDto>()
            {
                TotalPaginas = retornoquery.TotalPaginas,
                TotalRegistros = retornoquery.TotalRegistros
            };

            bool nenhumItemEncontrado = retornoquery.Items == null ||
                !retornoquery.Items.Any() ||
                retornoquery.Items.ElementAt(0).Id == 0;

            retornoPaginado.Items = nenhumItemEncontrado ? null : retornoquery.Items.Select(x => MapearParaDto(x));

            return retornoPaginado;
        }

        public RegistroPoaCompletoDto ObterPorId(long id)
        {
            var retorno = repositorioRegistroPoa.ObterPorId(id);

            if (retorno == null || retorno.Excluido)
                return null;

            return MapearParaDto(retorno);
        }

        private RegistroPoaDto MapearParaDto(RegistroPoa registroPoa)
        {
            return new RegistroPoaDto
            {
                CodigoRf = registroPoa.CodigoRf,
                Descricao = registroPoa.Descricao,
                DreId = registroPoa.DreId,
                Excluido = registroPoa.Excluido,
                Id = registroPoa.Id,
                Mes = registroPoa.Mes,
                Titulo = registroPoa.Titulo,
                UeId = registroPoa.UeId
            };
        }

        private RegistroPoaCompletoDto MapearParaDtoCompleto(RegistroPoa registroPoa)
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
                Descricao = registroPoa.Descricao,
                DreId = registroPoa.DreId,
                Excluido = registroPoa.Excluido,
                Id = registroPoa.Id,
                Mes = registroPoa.Mes,
                Titulo = registroPoa.Titulo,
                UeId = registroPoa.UeId
            };
        }
    }
}