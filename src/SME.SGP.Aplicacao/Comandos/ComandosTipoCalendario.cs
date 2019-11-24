using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoCalendario : IComandosTipoCalendario
    {
        private readonly IRepositorioTipoCalendario repositorio;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoFeriadoCalendario servicoFeriadoCalendario;

        public ComandosTipoCalendario(IRepositorioTipoCalendario repositorio, IServicoFeriadoCalendario servicoFeriadoCalendario, IServicoEvento servicoEvento,
            IRepositorioEvento repositorioEvento)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoFeriadoCalendario = servicoFeriadoCalendario ?? throw new ArgumentNullException(nameof(servicoFeriadoCalendario));
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task Alterar(TipoCalendarioDto dto, long id)
        {
            var tipoCalendario = MapearParaDominio(dto, id);

            bool ehRegistroExistente = await repositorio.VerificarRegistroExistente(dto.Id, dto.Nome);

            if (ehRegistroExistente)
                throw new NegocioException($"O Tipo de Calendário Escolar '{dto.Nome}' já existe");

            repositorio.Salvar(tipoCalendario);

            SME.Background.Core.Cliente.Executar<IComandosTipoCalendario>(x => x.ExecutarMetodosAsync(dto, false, tipoCalendario));
        }

        public async Task Incluir(TipoCalendarioDto dto)
        {
            var tipoCalendario = MapearParaDominio(dto, 0);

            bool ehRegistroExistente = await repositorio.VerificarRegistroExistente(0, dto.Nome);

            if (ehRegistroExistente)
                throw new NegocioException($"O Tipo de Calendário Escolar '{dto.Nome}' já existe");

            repositorio.Salvar(tipoCalendario);

            SME.Background.Core.Cliente.Executar<IComandosTipoCalendario>(x => x.ExecutarMetodosAsync(dto, false, tipoCalendario));
        }

        public TipoCalendario MapearParaDominio(TipoCalendarioDto dto, long id)
        {
            TipoCalendario entidade = repositorio.ObterPorId(id);
            bool possuiEventos = repositorioEvento.ExisteEventoPorTipoCalendarioId(id);

            if (entidade == null)
            {
                entidade = new TipoCalendario();
            }

            entidade.Nome = dto.Nome;
            entidade.Situacao = dto.Situacao;

            if (!possuiEventos)
            {
                entidade.AnoLetivo = dto.AnoLetivo;
                entidade.Periodo = dto.Periodo;
                entidade.Modalidade = dto.Modalidade;
            }
            return entidade;
        }

        public void MarcarExcluidos(long[] ids)
        {
            var idsInvalidos = "";
            var tiposInválidos = "";
            foreach (long id in ids)
            {
                var tipoCalendario = repositorio.ObterPorId(id);
                if (tipoCalendario != null)
                {
                    var possuiEventos = repositorioEvento.ExisteEventoPorTipoCalendarioId(id);
                    if (possuiEventos)
                    {
                        tiposInválidos += string.IsNullOrEmpty(tiposInválidos) ? $"{tipoCalendario.Nome}" : $", {tipoCalendario.Nome}";
                    }
                    else
                    {
                        tipoCalendario.Excluido = true;
                        repositorio.Salvar(tipoCalendario);
                    }
                }
                else
                {
                    idsInvalidos += string.IsNullOrEmpty(idsInvalidos) ? $"{id}" : $", {id}";
                }
            }
            if (!idsInvalidos.Trim().Equals(""))
            {
                throw new NegocioException($"Houve um erro ao excluir os tipos de calendário ids '{idsInvalidos}'. Um dos tipos de calendário não existe");
            }
            if (!tiposInválidos.Trim().Equals(""))
            {
                throw new NegocioException($"Houve um erro ao excluir os tipos de calendário '{tiposInválidos}'. Os tipos de calendário possuem eventos vínculados");
            }
        }

        public void ExecutarMetodosAsync(TipoCalendarioDto dto, bool inclusao, TipoCalendario tipoCalendario)
        {
            servicoFeriadoCalendario.VerficaSeExisteFeriadosMoveisEInclui(dto.AnoLetivo);

            if (inclusao)
                servicoEvento.SalvarEventoFeriadosAoCadastrarTipoCalendario(tipoCalendario);
        }
    }
}