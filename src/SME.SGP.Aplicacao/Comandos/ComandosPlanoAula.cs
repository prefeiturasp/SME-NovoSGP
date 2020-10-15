using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAula : IComandosPlanoAula
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IConsultasPlanoAnual consultasPlanoAnual;
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IRepositorioPlanoAula repositorio;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano;
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ComandosPlanoAula(IRepositorioPlanoAula repositorioPlanoAula,
                        IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
                        IRepositorioObjetivoAprendizagemPlano repositorioObjetivoAprendizagemPlano,
                        IRepositorioAula repositorioAula,
                        IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                        IConsultasAbrangencia consultasAbrangencia,
                        IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                        IConsultasPlanoAnual consultasPlanoAnual,
                        IConsultasProfessor consultasProfessor,
                        IServicoUsuario servicoUsuario,
                        IUnitOfWork unitOfWork,
                        IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                        IMediator mediator)
        {
            this.repositorio = repositorioPlanoAula;
            this.repositorioObjetivosAula = repositorioObjetivosAula;
            this.repositorioObjetivoAprendizagemPlano = repositorioObjetivoAprendizagemPlano;
            this.repositorioAula = repositorioAula;
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ;
            this.consultasAbrangencia = consultasAbrangencia;
            this.consultasProfessor = consultasProfessor;
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem;
            this.consultasPlanoAnual = consultasPlanoAnual;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario;
        }

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = repositorioAula.ObterPorId(aulaId);

            await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            await repositorio.ExcluirPlanoDaAula(aulaId);
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime dataAula, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await servicoUsuario.ObterUsuarioLogado();

            if (!usuario.EhProfessorCj() && !await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
        }
    }
}