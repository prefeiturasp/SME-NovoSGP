﻿using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ComandoComunicado : IComandoComunicado
    {
        private readonly IRepositorioComunicado repositorio;        
        private readonly IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioComunicadoTurma repositorioComunicadoTurma;
        private readonly IRepositorioEvento repositorioEvento;
        //private readonly Mediator mediator;
        private const string Todas = "-99";

        public ComandoComunicado(IRepositorioComunicado repositorio,
            IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar,            
            IUnitOfWork unitOfWork,
            IRepositorioComunicadoAluno repositorioComunicadoAluno,
            IServicoUsuario servicoUsuario,
            IConsultasAbrangencia consultasAbrangencia,
            IRepositorioComunicadoTurma repositorioComunicadoTurma,
            IRepositorioEvento repositorioEvento)//, Mediator mediator)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));            
            this.servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new System.ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            //this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<string> Alterar(long id, ComunicadoInserirDto comunicadoDto)
        {
            Comunicado comunicado = BuscarComunicado(id);

            ComunicadoInserirAeDto comunicadoServico = new ComunicadoInserirAeDto();

            var usuarioLogado = (await servicoUsuario.ObterUsuarioLogado())
                ?? throw new NegocioException("Usuário logado não encontrado");

            await ValidarAbrangenciaUsuario(comunicadoDto, usuarioLogado);

            MapearAlteracao(comunicadoDto, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();

                await repositorio.SalvarAsync(comunicado);

                MapearParaEntidadeServico(comunicadoServico, comunicado);

                await servicoAcompanhamentoEscolar.AlterarComunicado(comunicadoServico, id);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado alterado com sucesso";
        }

        public async Task Excluir(long[] ids)
        {
            var erros = new StringBuilder();

            var comunicados = ids.Select(id =>
            {
                var comunicado = repositorio.ObterPorId(id);
                if (comunicado.EhNulo())
                {
                    erros.Append($"<li>{id} - comunicado não encontrado</li>");
                }
                else
                {
                    if (comunicado.EventoId.HasValue)
                    {
                        var evento = repositorioEvento.ObterPorId(comunicado.EventoId.Value);
                        if (evento.NaoEhNulo() && !evento.Excluido)
                        {
                            erros.Append($"<li>{id} - {comunicado.Titulo} - Comunicado com evento vinculado</li>");
                        }
                    }
                }
                return comunicado;
            });

            if (string.IsNullOrEmpty(erros.ToString())) {
                await servicoAcompanhamentoEscolar.ExcluirComunicado(ids);
                foreach (var comunicado in comunicados)
                {
                    try
                    {                        
                        await repositorioComunicadoAluno.RemoverTodosAlunosComunicado(comunicado.Id);
                        await repositorioComunicadoTurma.RemoverTodasTurmasComunicado(comunicado.Id);

                        comunicado.MarcarExcluido();

                        await repositorio.SalvarAsync(comunicado);
                    }
                    catch
                    {
                        erros.Append($"<li>{comunicado.Id} - {comunicado.Titulo}</li>");
                    }
                }
            }

            if (!string.IsNullOrEmpty(erros.ToString()))
                throw new NegocioException($"<p>Os seguintes comunicados não puderam ser excluídos:</p><br/>{erros.ToString()} por favor, tente novamente");
        }

        public async Task<string> Inserir(ComunicadoInserirDto comunicadoDto)
        {
            Comunicado comunicado = new Comunicado();
            ComunicadoInserirAeDto comunicadoServico = new ComunicadoInserirAeDto();

            await ValidarInsercao(comunicadoDto);

            MapearParaEntidade(comunicadoDto, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();

                var id = await repositorio.SalvarAsync(comunicado);                

                comunicado.AtualizarIdAlunos();

                comunicado.AtualizarIdTurmas();

                await SalvarTurmas(comunicado.Turmas);

                await SalvarAlunos(comunicado.Alunos);

                MapearParaEntidadeServico(comunicadoServico, comunicado);

                await servicoAcompanhamentoEscolar.CriarComunicado(comunicadoServico);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado criado com sucesso";
        }

        private async Task SalvarAlunos(IEnumerable<ComunicadoAluno> alunos)
        {
            foreach (var aluno in alunos)
                await repositorioComunicadoAluno.SalvarAsync(aluno);
        }

        private async Task SalvarTurmas(IEnumerable<ComunicadoTurma> turmas)
        {
            foreach (var turma in turmas)
                await repositorioComunicadoTurma.SalvarAsync(turma);
        }       

        private void MapearAlteracao(ComunicadoInserirDto comunicadoDto, Comunicado comunicado)
        {
            comunicado.Descricao = comunicadoDto.Descricao;
            comunicado.Titulo = comunicadoDto.Titulo;
            comunicado.DataExpiracao = comunicadoDto.DataExpiracao;
            comunicado.TipoCalendarioId = comunicadoDto.TipoCalendarioId;
            comunicado.EventoId = comunicadoDto.EventoId;
        }

        private async Task ValidarInsercao(ComunicadoInserirDto comunicadoDto)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            await ValidarAbrangenciaUsuario(comunicadoDto, usuarioLogado);

            if (comunicadoDto.CodigoDre.Equals(Todas) && !comunicadoDto.CodigoUe.Equals(Todas))
                throw new NegocioException("Não é possivel especificar uma escola quando o comunicado é para todas as DREs");

            if (comunicadoDto.CodigoUe.Equals(Todas) && comunicadoDto.Turmas.Any())
                throw new NegocioException("Não é possivel especificar uma turma quando o comunicado é para todas as UEs");

            if ((comunicadoDto.Turmas.EhNulo() || !comunicadoDto.Turmas.Any()) && (comunicadoDto.AlunoEspecificado || (comunicadoDto.Alunos?.Any() ?? false)))
                throw new NegocioException("Não é possivel especificar alunos quando o comunicado é para todas as Turmas");
        }

        private async Task ValidarAbrangenciaUsuario(ComunicadoInserirDto comunicadoDto, Usuario usuarioLogado)
        {
            if (comunicadoDto.CodigoDre.Equals(Todas) && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem realizar envio de Comunicados para todas as DREs");

            if (comunicadoDto.CodigoUe.Equals(Todas) && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem realizar envio de Comunicados para todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && !comunicadoDto.CodigoDre.Equals(Todas))
                await ValidarAbrangenciaDre(comunicadoDto);

            if (usuarioLogado.EhPerfilUE() && !comunicadoDto.CodigoUe.Equals(Todas))
                await ValidarAbrangenciaUE(comunicadoDto);
        }

        private async Task ValidarAbrangenciaTurma(ComunicadoInserirDto comunicadoDto)
        {
            foreach (var turma in comunicadoDto.Turmas)
            {
                //var abrangenciaTurmas = await mediator.Send(new ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(turma));
                var abrangenciaTurmas = await consultasAbrangencia.ObterAbrangenciaTurma(turma);

                if (abrangenciaTurmas.EhNulo())
                    throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a Turma com codigo {turma}");
            }
        }

        private async Task ValidarAbrangenciaUE(ComunicadoInserirDto comunicadoDto)
        {
            var abrangenciaUes = await consultasAbrangencia.ObterUes(comunicadoDto.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(comunicadoDto.CodigoUe));

            if (ue.EhNulo())
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a UE com codigo {comunicadoDto.CodigoUe}");

            if (comunicadoDto.Turmas.NaoEhNulo() && comunicadoDto.Turmas.Any())
                await ValidarAbrangenciaTurma(comunicadoDto);
        }

        private async Task ValidarAbrangenciaDre(ComunicadoInserirDto comunicadoDto)
        {
            var abrangenciaDres = await consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(comunicadoDto.CodigoDre));

            if (dre.EhNulo())
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a DRE com codigo {comunicadoDto.CodigoDre}");
        }

        private void MapearParaEntidade(ComunicadoInserirDto comunicadoDto, Comunicado comunicado)
        {
            comunicado.DataEnvio = comunicadoDto.DataEnvio;
            comunicado.DataExpiracao = comunicadoDto.DataExpiracao;
            comunicado.AlunoEspecificado = comunicadoDto.AlunoEspecificado;
            comunicado.Descricao = comunicadoDto.Descricao;
            comunicado.Titulo = comunicadoDto.Titulo;
            comunicado.AnoLetivo = comunicadoDto.AnoLetivo;
            comunicado.TipoCalendarioId = comunicadoDto.TipoCalendarioId;
            comunicado.EventoId = comunicadoDto.EventoId;

            if (!comunicadoDto.CodigoDre.Equals("-99"))
                comunicado.CodigoDre = comunicadoDto.CodigoDre;

            if (!comunicadoDto.CodigoUe.Equals("-99"))
                comunicado.CodigoUe = comunicadoDto.CodigoUe;

            if (comunicadoDto.Turmas.NaoEhNulo() && comunicadoDto.Turmas.Any())
                comunicadoDto.Turmas.ToList().ForEach(x => comunicado.AdicionarTurma(x));

            if (comunicadoDto.Modalidades.Any())
                comunicado.Modalidades = comunicadoDto.Modalidades;           

            if (comunicadoDto.AlunoEspecificado)
                comunicadoDto.Alunos.ToList().ForEach(x => comunicado.AdicionarAluno(x));

            if (comunicadoDto.Semestre > 0)
                comunicado.Semestre = comunicadoDto.Semestre;

            comunicado.SetarTipoComunicado();
        }

        private Comunicado BuscarComunicado(long id)
        {
            return repositorio.ObterPorId(id) ?? throw new NegocioException("Comunicado não encontrado");
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
            comunicadoServico.Modalidades = string.Join(",", comunicado.Modalidades.Select(x => x).ToArray());
        }
    }
}