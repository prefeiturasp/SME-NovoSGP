using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarInclusaoComunicadoEscolaAquiCommandHandler : IRequestHandler<SolicitarInclusaoComunicadoEscolaAquiCommand, string>
    {
        private const string TODAS = "todas";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorioComunicado _repositorioComunicado;
        private readonly IRepositorioComunicadoTurma _repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno _repositorioComunicadoAluno;
        private readonly IServicoAcompanhamentoEscolar _servicoAcompanhamentoEscolar;
        private readonly IServicoUsuario _servicoUsuario;
        private readonly IConsultasAbrangencia _consultasAbrangencia;

        public SolicitarInclusaoComunicadoEscolaAquiCommandHandler(
              IRepositorioComunicado repositorioComunicado
            , IUnitOfWork unitOfWork
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar
            , IServicoUsuario servicoUsuario
            , IConsultasAbrangencia consultasAbrangencia
            )
        {
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this._repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this._servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
            this._servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this._consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<string> Handle(SolicitarInclusaoComunicadoEscolaAquiCommand request, CancellationToken cancellationToken)
        {
            var comunicado = new Comunicado();
            var comunicadoServico = new ComunicadoInserirAeDto();

            await ValidarInsercao(request);

            MapearParaEntidade(request, comunicado);

            try
            {
                _unitOfWork.IniciarTransacao();

                var id = await _repositorioComunicado.SalvarAsync(comunicado);

                comunicado.AtualizarIdAlunos();
                comunicado.AtualizarIdTurmas();

                // Turmas
                await SalvarComunicadosParaTurmas(comunicado.Turmas);

                // Alunos
                await SalvarComunicadosParaAlunos(comunicado.Alunos);

                MapearParaEntidadeServico(comunicadoServico, comunicado);

                // Criar comunicado no Escola Aqui
                await _servicoAcompanhamentoEscolar.CriarComunicado(comunicadoServico);

                _unitOfWork.PersistirTransacao();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            return "Comunicado criado com sucesso!";
        }

        private async Task ValidarInsercao(SolicitarInclusaoComunicadoEscolaAquiCommand comunicado)
        {

            var usuarioLogado = await _servicoUsuario.ObterUsuarioLogado();

            await ValidarAbrangenciaUsuario(comunicado, usuarioLogado);

            if (comunicado.CodigoDre == TODAS && !comunicado.CodigoUe.Equals(TODAS))
                throw new NegocioException("Não é possivel especificar uma escola quando o comunicado é para todas as DREs");

            if (comunicado.CodigoUe == TODAS && comunicado.Turmas.Any())
                throw new NegocioException("Não é possivel especificar uma turma quando o comunicado é para todas as UEs");

            if ((comunicado.Turmas == null || !comunicado.Turmas.Any()) && (comunicado.AlunosEspecificados || (comunicado.Alunos?.Any() ?? false)))
                throw new NegocioException("Não é possivel especificar alunos quando o comunicado é para todas as Turmas");
        }

        private async Task ValidarAbrangenciaUsuario(SolicitarInclusaoComunicadoEscolaAquiCommand comunicado, Usuario usuarioLogado)
        {
            if (comunicado.CodigoDre == TODAS && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem realizar envio de Comunicados para todas as DREs");

            if (comunicado.CodigoUe == TODAS && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem realizar envio de Comunicados para todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && !comunicado.CodigoDre.Equals(TODAS))
                await ValidarAbrangenciaDre(comunicado);

            if (usuarioLogado.EhPerfilUE() && !comunicado.CodigoUe.Equals(TODAS))
                await ValidarAbrangenciaUE(comunicado);
        }

        private async Task ValidarAbrangenciaUE(SolicitarInclusaoComunicadoEscolaAquiCommand comunicado)
        {
            var abrangenciaUes = await _consultasAbrangencia.ObterUes(comunicado.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(comunicado.CodigoUe));

            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a UE com codigo {comunicado.CodigoUe}");

            if (comunicado.Turmas != null && comunicado.Turmas.Any())
                await ValidarAbrangenciaTurma(comunicado);
        }

        private async Task ValidarAbrangenciaDre(SolicitarInclusaoComunicadoEscolaAquiCommand comunicado)
        {
            var abrangenciaDres = await _consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(comunicado.CodigoDre));

            if (dre == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a DRE com codigo {comunicado.CodigoDre}");
        }

        private async Task ValidarAbrangenciaTurma(SolicitarInclusaoComunicadoEscolaAquiCommand comunicado)
        {
            foreach (var turma in comunicado.Turmas)
            {
                var abrangenciaTurmas = await _consultasAbrangencia.ObterAbrangenciaTurma(turma);

                if (abrangenciaTurmas == null)
                    throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a Turma com codigo {turma}");
            }
        }

        private void MapearParaEntidade(SolicitarInclusaoComunicadoEscolaAquiCommand request, Comunicado comunicado)
        {
            comunicado.DataEnvio = request.DataEnvio;
            comunicado.DataExpiracao = request.DataExpiracao;
            comunicado.AlunoEspecificado = request.AlunosEspecificados;
            comunicado.Descricao = request.Descricao;
            comunicado.Titulo = request.Titulo;
            comunicado.AnoLetivo = request.AnoLetivo;
            comunicado.SeriesResumidas = request.SeriesResumidas;
            comunicado.TipoCalendarioId = request.TipoCalendarioId;
            comunicado.EventoId = request.EventoId;

            if (!request.CodigoDre.Equals("todas"))
                comunicado.CodigoDre = request.CodigoDre;

            if (!request.CodigoUe.Equals("todas"))
                comunicado.CodigoUe = request.CodigoUe;

            if (request.Turmas != null && request.Turmas.Any())
                request.Turmas.ToList().ForEach(x => comunicado.AdicionarTurma(x));

            if (request.Modalidades.Any())
                comunicado.Modalidades = request.Modalidades;            

            if (request.AlunosEspecificados)
                request.Alunos.ToList().ForEach(x => comunicado.AdicionarAluno(x));

            if (request.Semestre > 0)
                comunicado.Semestre = request.Semestre;

            comunicado.SetarTipoComunicado();
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
            comunicadoServico.SeriesResumidas = comunicado.SeriesResumidas;
            comunicadoServico.Modalidades = string.Join(",", comunicado.Modalidades.Select(x => x).ToArray());
        }

        private async Task SalvarComunicadosParaTurmas(IEnumerable<ComunicadoTurma> turmas)
        {
            foreach (var turma in turmas)
                await _repositorioComunicadoTurma.SalvarAsync(turma);
        }

        private async Task SalvarComunicadosParaAlunos(IEnumerable<ComunicadoAluno> alunos)
        {
            foreach (var aluno in alunos)
                await _repositorioComunicadoAluno.SalvarAsync(aluno);
        }
    }
}
