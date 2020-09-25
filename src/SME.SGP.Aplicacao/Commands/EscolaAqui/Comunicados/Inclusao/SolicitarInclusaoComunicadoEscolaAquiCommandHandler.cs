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
        private readonly IRepositorioComunicadoGrupo _repositorioComunicadoGrupo;
        private readonly IRepositorioComunicadoTurma _repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno _repositorioComunicadoAluno;
        private readonly IServicoAcompanhamentoEscolar _servicoAcompanhamentoEscolar;

        public SolicitarInclusaoComunicadoEscolaAquiCommandHandler(
              IRepositorioComunicado repositorioComunicado
            , IUnitOfWork unitOfWork
            , IRepositorioComunicadoGrupo repositorioComunicadoGrupo
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar
            )
        {
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._repositorioComunicadoGrupo = repositorioComunicadoGrupo ?? throw new ArgumentNullException(nameof(repositorioComunicadoGrupo));
            this._repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this._repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this._servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
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

                // Grupos
                await SalvarComunicadosParaGrupos(id, request);

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
            //var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            //await ValidarAbrangenciaUsuario(comunicadoDto, usuarioLogado);

            if (comunicado.CodigoDre == TODAS && !comunicado.CodigoUe.Equals(TODAS))
                throw new NegocioException("Não é possivel especificar uma escola quando o comunicado é para todas as DREs");

            if (comunicado.CodigoUe == TODAS && comunicado.Turmas.Any())
                throw new NegocioException("Não é possivel especificar uma turma quando o comunicado é para todas as UEs");

            if ((comunicado.Turmas == null || !comunicado.Turmas.Any()) && (comunicado.AlunosEspecificados || (comunicado.Alunos?.Any() ?? false)))
                throw new NegocioException("Não é possivel especificar alunos quando o comunicado é para todas as Turmas");
        }


        private void MapearParaEntidade(SolicitarInclusaoComunicadoEscolaAquiCommand request, Comunicado comunicado)
        {
            comunicado.DataEnvio = request.DataEnvio;
            comunicado.DataExpiracao = request.DataExpiracao;
            comunicado.AlunoEspecificado = request.AlunosEspecificados;
            comunicado.Descricao = request.Descricao;
            comunicado.Titulo = request.Titulo;
            comunicado.AnoLetivo = request.AnoLetivo;

            if (!request.CodigoDre.Equals("todas"))
                comunicado.CodigoDre = request.CodigoDre;

            if (!request.CodigoUe.Equals("todas"))
                comunicado.CodigoUe = request.CodigoUe;

            if (request.Turmas != null && request.Turmas.Any())
                request.Turmas.ToList().ForEach(x => comunicado.AdicionarTurma(x));

            if (request.Modalidade.HasValue)
                comunicado.Modalidade = request.Modalidade;

            if (request.GruposId.Any())
                comunicado.Grupos = request.GruposId.Select(s => new ComunicadoGrupo { Id = s }).ToList();

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

        private async Task SalvarComunicadosParaGrupos(long id, SolicitarInclusaoComunicadoEscolaAquiCommand comunicado)
        {
            foreach (var grupoId in comunicado.GruposId)
                await _repositorioComunicadoGrupo.SalvarAsync(new ComunicadoGrupo { ComunicadoId = id, GrupoComunicadoId = grupoId });
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
