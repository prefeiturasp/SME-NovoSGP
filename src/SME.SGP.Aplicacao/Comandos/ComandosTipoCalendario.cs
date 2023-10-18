using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using static SME.SGP.Aplicacao.ExecutarTipoCalendarioUseCase;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoCalendario : IComandosTipoCalendario
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoFeriadoCalendario servicoFeriadoCalendario;

        public ComandosTipoCalendario(IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta, 
                                      IServicoFeriadoCalendario servicoFeriadoCalendario, 
                                      IServicoEvento servicoEvento,
                                      IRepositorioEvento repositorioEvento, 
                                      IMediator mediator,
                                      IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioTipoCalendarioConsulta = repositorioTipoCalendarioConsulta ?? throw new ArgumentNullException(nameof(repositorioTipoCalendarioConsulta));
            this.servicoFeriadoCalendario = servicoFeriadoCalendario ?? throw new ArgumentNullException(nameof(servicoFeriadoCalendario));
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task Alterar(TipoCalendarioDto tipoCalendarioDto, long id)
        {
            var tipoCalendario = MapearParaDominio(tipoCalendarioDto, id);

            ValidarSemestreParaEjaECelp(tipoCalendarioDto);

            bool ehRegistroExistente = await mediator.Send(new VerificarRegistroExistenteTipoCalendarioQuery(tipoCalendario.Id, tipoCalendario.Nome));

            if (ehRegistroExistente)
                throw new NegocioException(MensagemNegocioComuns.TIPO_CALENDARIO_ESCOLAR_X_JA_EXISTE);

            repositorioTipoCalendarioConsulta.Salvar(tipoCalendario);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarTipoCalendario, new ExecutarTipoCalendarioParametro { Dto = tipoCalendarioDto, TipoCalendario = tipoCalendario }, Guid.NewGuid(), null));
        }

        private static void ValidarSemestreParaEjaECelp(TipoCalendarioDto tipoCalendarioDto)
        {
            if (tipoCalendarioDto.Modalidade.EhEjaOuCelp() && !tipoCalendarioDto.Semestre.HasValue)
                throw new NegocioException(MensagemNegocioComuns.TIPO_CALENDARIO_EJA_OU_CELP_DEVE_TER_SEMESTRE);
            
            if (tipoCalendarioDto.Modalidade.NaoEhEjaOuCelp() && tipoCalendarioDto.Semestre.HasValue)
                throw new NegocioException(MensagemNegocioComuns.TIPO_CALENDARIO_DIFERENTE_EJA_OU_CELP_NAO_DEVE_TER_SEMESTRE);
        }

        public async Task ExecutarReplicacao(TipoCalendarioDto dto, bool inclusao, TipoCalendario tipoCalendario)
        {
            servicoFeriadoCalendario.VerficaSeExisteFeriadosMoveisEInclui(dto.AnoLetivo);

            if (inclusao)
            {
                servicoEvento.SalvarEventoFeriadosAoCadastrarTipoCalendario(tipoCalendario);

                var existeParametro = await mediator.Send(new VerificaSeExisteParametroSistemaPorAnoQuery(dto.AnoLetivo));

                if (!existeParametro)
                    await mediator.Send(new ReplicarParametrosAnoAnteriorCommand(dto.AnoLetivo, dto.Modalidade));
            }
        }

        public async Task Incluir(TipoCalendarioDto tipoCalendarioDto)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                ValidarSemestreParaEjaECelp(tipoCalendarioDto);
                
                var tipoCalendario = MapearParaDominio(tipoCalendarioDto);

                bool ehRegistroExistente = await mediator.Send(new VerificarRegistroExistenteTipoCalendarioQuery(0, tipoCalendarioDto.Nome));

                if (ehRegistroExistente)
                    throw new NegocioException(MensagemNegocioComuns.TIPO_CALENDARIO_ESCOLAR_X_JA_EXISTE);

                repositorioTipoCalendarioConsulta.Salvar(tipoCalendario);

                await ExecutarReplicacao(tipoCalendarioDto, true, tipoCalendario);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }            
        }

        public TipoCalendario MapearParaDominio(TipoCalendarioDto tipoCalendarioDto, long? id = null)
        {
            var tipoCalendario = id.HasValue ? repositorioTipoCalendarioConsulta.ObterPorId(id.Value) : new TipoCalendario();
            bool possuiEventos = id.HasValue && repositorioEvento.ExisteEventoPorTipoCalendarioId(id.Value);
                
            tipoCalendario.Nome = tipoCalendarioDto.Nome;
            tipoCalendario.Situacao = tipoCalendarioDto.Situacao;
            tipoCalendario.Semestre = tipoCalendarioDto.Semestre;

            if (!possuiEventos)
            {
                tipoCalendario.AnoLetivo = tipoCalendarioDto.AnoLetivo;
                tipoCalendario.Periodo = tipoCalendarioDto.Periodo;
                tipoCalendario.Modalidade = tipoCalendarioDto.Modalidade;
            }
            return tipoCalendario;
        }

        public void MarcarExcluidos(long[] ids)
        {
            StringBuilder idsInvalidos = new StringBuilder();
            StringBuilder tiposInvalidos = new StringBuilder();
            foreach (long id in ids)
            {
                var tipoCalendario = repositorioTipoCalendarioConsulta.ObterPorId(id);
                if (tipoCalendario.NaoEhNulo())
                {
                    var possuiEventos = repositorioEvento.ExisteEventoPorTipoCalendarioId(id);
                    if (possuiEventos)
                    {
                        tiposInvalidos.Append($"{tipoCalendario.Nome}, ");
                    }
                    else
                    {
                        tipoCalendario.Excluido = true;
                        repositorioTipoCalendarioConsulta.Salvar(tipoCalendario);
                    }
                }
                else
                {
                    idsInvalidos.Append($"{id}, ");
                }
            }

            if (idsInvalidos.ToString().EstaPreenchido())
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