using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarAlteracaoComunicadoEscolaAquiCommandHandler : IRequestHandler<SolicitarAlteracaoComunicadoEscolaAquiCommand, string>
    {
        IMediator _mediator;
        private const string TODAS = "todas";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorioComunicado _repositorioComunicado;
        private readonly IServicoAcompanhamentoEscolar _servicoAcompanhamentoEscolar;
        private readonly IConsultasAbrangencia _consultasAbrangencia;

        public SolicitarAlteracaoComunicadoEscolaAquiCommandHandler(
              IMediator mediator
            , IRepositorioComunicado repositorioComunicado
            , IUnitOfWork unitOfWork
            , IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar
            , IConsultasAbrangencia consultasAbrangencia
            )
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
            this._consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<string> Handle(SolicitarAlteracaoComunicadoEscolaAquiCommand request, CancellationToken cancellationToken)
        {
            // TODO: Criar Query Handler para Obter comunicado
            Comunicado comunicado = BuscarComunicado(request.Id);

            ComunicadoInserirAeDto comunicadoServico = new ComunicadoInserirAeDto();

            var usuarioLogado = await _mediator.Send(new ObterUsuarioLogadoQuery());

            await ValidarAbrangenciaUsuario(request, usuarioLogado);

            MapearAlteracao(request, comunicado);

            try
            {
                _unitOfWork.IniciarTransacao();

                await _repositorioComunicado.SalvarAsync(comunicado);

                MapearParaEntidadeServico(comunicadoServico, comunicado);

                await _servicoAcompanhamentoEscolar.AlterarComunicado(comunicadoServico, request.Id);

                _unitOfWork.PersistirTransacao();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            return "Comunicado alterado com sucesso";

        }

        private void MapearAlteracao(SolicitarAlteracaoComunicadoEscolaAquiCommand request, Comunicado comunicado)
        {
            comunicado.Descricao = request.Descricao;
            comunicado.Titulo = request.Titulo;
            comunicado.DataExpiracao = request.DataExpiracao;
            comunicado.TipoCalendarioId = request.TipoCalendarioId;
            comunicado.EventoId = request.EventoId;
        }

        private void MapearParaEntidadeServico(ComunicadoInserirAeDto comunicadoServico, Comunicado comunicado)
        {
            comunicadoServico.Id = comunicado.Id;
            comunicadoServico.AlteradoEm = comunicado.AlteradoEm;
            comunicadoServico.AlteradoPor = comunicado.AlteradoPor;
            comunicadoServico.AlteradoRF = comunicado.AlteradoRF;
            comunicadoServico.DataEnvio = comunicado.DataEnvio;
            comunicadoServico.DataExpiracao = comunicado.DataExpiracao;
            comunicadoServico.Mensagem = comunicado.Descricao;
            comunicadoServico.Titulo = comunicado.Titulo;
            comunicadoServico.Grupo = string.Join(",", comunicado.Grupos.Select(x => x.Id.ToString()).ToArray());
            comunicadoServico.CriadoEm = comunicado.CriadoEm;
            comunicadoServico.CriadoPor = comunicado.CriadoPor;
            comunicadoServico.CriadoRF = comunicado.CriadoRF;
            comunicadoServico.Alunos = comunicado.Alunos.Select(x => x.AlunoCodigo);
            comunicadoServico.AnoLetivo = comunicado.AnoLetivo;
            comunicadoServico.CodigoDre = comunicado.CodigoDre;
            comunicadoServico.CodigoUe = comunicado.CodigoUe;
            comunicadoServico.Turmas = comunicado.Turmas.Select(x => x.CodigoTurma);
            comunicadoServico.TipoComunicado = comunicado.TipoComunicado;
            comunicadoServico.Semestre = comunicado.Semestre;
        }

        private async Task ValidarAbrangenciaUsuario(SolicitarAlteracaoComunicadoEscolaAquiCommand comunicado, Usuario usuarioLogado)
        {
            if (comunicado.CodigoDre.Equals(TODAS) && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem realizar envio de Comunicados para todas as DREs");

            if (comunicado.CodigoUe.Equals(TODAS) && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem realizar envio de Comunicados para todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && !comunicado.CodigoDre.Equals(TODAS))
                await ValidarAbrangenciaDre(comunicado);

            if (usuarioLogado.EhPerfilUE() && !comunicado.CodigoUe.Equals(TODAS))
                await ValidarAbrangenciaUE(comunicado);
        }

        private async Task ValidarAbrangenciaUE(SolicitarAlteracaoComunicadoEscolaAquiCommand comunicado)
        {
            var abrangenciaUes = await _consultasAbrangencia.ObterUes(comunicado.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(comunicado.CodigoUe));

            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a UE com codigo {comunicado.CodigoUe}");

            if (comunicado.Turmas != null && comunicado.Turmas.Any())
                await ValidarAbrangenciaTurma(comunicado);
        }

        private async Task ValidarAbrangenciaDre(SolicitarAlteracaoComunicadoEscolaAquiCommand comunicado)
        {
            var abrangenciaDres = await _consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(comunicado.CodigoDre));

            if (dre == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a DRE com codigo {comunicado.CodigoDre}");
        }

        private async Task ValidarAbrangenciaTurma(SolicitarAlteracaoComunicadoEscolaAquiCommand comunicado)
        {
            foreach (var turma in comunicado.Turmas)
            {
                var abrangenciaTurmas = await _consultasAbrangencia.ObterAbrangenciaTurma(turma);

                if (abrangenciaTurmas == null)
                    throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a Turma com codigo {turma}");
            }
        }

        private Comunicado BuscarComunicado(long id)
        {
            return _repositorioComunicado.ObterPorId(id) ?? throw new NegocioException("Comunicado não encontrado");
        }
    }
}
