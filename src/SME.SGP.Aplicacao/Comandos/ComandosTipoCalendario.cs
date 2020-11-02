using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoCalendario : IComandosTipoCalendario
    {
        private readonly IMediator mediator;
        private readonly IRepositorioTipoCalendario repositorio;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoFeriadoCalendario servicoFeriadoCalendario;

        public ComandosTipoCalendario(IRepositorioTipoCalendario repositorio, IServicoFeriadoCalendario servicoFeriadoCalendario, IServicoEvento servicoEvento,
            IRepositorioEvento repositorioEvento, IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

        public async Task ExecutarMetodosAsync(TipoCalendarioDto dto, bool inclusao, TipoCalendario tipoCalendario)
        {
            servicoFeriadoCalendario.VerficaSeExisteFeriadosMoveisEInclui(dto.AnoLetivo);

            if (inclusao)
            {
                servicoEvento.SalvarEventoFeriadosAoCadastrarTipoCalendario(tipoCalendario);

                var existeParametro = await mediator.Send(new VerificaSeExisteParametroSistemaPorAnoQuery(dto.AnoLetivo));

                if (!existeParametro)
                    await mediator.Send(new ReplicarParametrosAnoAnteriorCommand(dto.AnoLetivo));
            }
        }

        public async Task Incluir(TipoCalendarioDto dto)
        {
            var tipoCalendario = MapearParaDominio(dto, 0);

            bool ehRegistroExistente = await repositorio.VerificarRegistroExistente(0, dto.Nome);

            if (ehRegistroExistente)
                throw new NegocioException($"O Tipo de Calendário Escolar '{dto.Nome}' já existe");

            repositorio.Salvar(tipoCalendario);

            SME.Background.Core.Cliente.Executar<IComandosTipoCalendario>(x => x.ExecutarMetodosAsync(dto, true, tipoCalendario));
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
            StringBuilder idsInvalidos = new StringBuilder();
            StringBuilder tiposInvalidos = new StringBuilder();
            foreach (long id in ids)
            {
                var tipoCalendario = repositorio.ObterPorId(id);
                if (tipoCalendario != null)
                {
                    var possuiEventos = repositorioEvento.ExisteEventoPorTipoCalendarioId(id);
                    if (possuiEventos)
                    {
                        tiposInvalidos.Append($"{tipoCalendario.Nome}, ");
                    }
                    else
                    {
                        tipoCalendario.Excluido = true;
                        repositorio.Salvar(tipoCalendario);
                    }
                }
                else
                {
                    idsInvalidos.Append($"{id}, ");
                }
            }

            if (!string.IsNullOrEmpty(idsInvalidos.ToString()))
            {
                string erroIds = idsInvalidos.ToString().TrimEnd(',');

                if (erroIds.IndexOf(',') > -1)
                    throw new NegocioException($"Houve um erro ao excluir os tipos de calendário ids '{erroIds}'. Um dos tipos de calendário não existe");
                else
                    throw new NegocioException($"Houve um erro ao excluir o tipo de calendário ids '{erroIds}'. O tipo de calendário não existe");
            }

            if (!string.IsNullOrEmpty(tiposInvalidos.ToString()))
            {
                string erroTipos = tiposInvalidos.ToString().TrimEnd(',');

                if (tiposInvalidos.ToString().IndexOf(',') > -1)
                    throw new NegocioException($"Houve um erro ao excluir os tipos de calendário '{erroTipos}'. Os tipos de calendário possuem eventos vinculados");
                else
                    throw new NegocioException($"Houve um erro ao excluir o tipo de calendário '{erroTipos}'. O tipo de calendário possui eventos vinculados");
            }
        }
    }
}