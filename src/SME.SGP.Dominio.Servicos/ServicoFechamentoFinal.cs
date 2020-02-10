using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoFinal : IServicoFechamentoFinal
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoFechamentoFinal(IRepositorioFechamentoFinal repositorioFechamentoFinal, IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioEvento repositorioEvento, IServicoEOL servicoEOL, IServicoUsuario servicoUsuario)
        {
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new ArgumentNullException(nameof(repositorioFechamentoFinal));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task SalvarAsync(FechamentoFinal fechamentoFinal)
        {
            await repositorioFechamentoFinal.SalvarAsync(fechamentoFinal);
        }

        public async Task VerificaPersistenciaGeral(Turma turma)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), turma.Semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi possível localizar o tipo de calendário.");

            var diaAtual = DateTime.Today;

            var eventoFechamento = await repositorioEvento.EventosNosDiasETipo(diaAtual, diaAtual, TipoEvento.FechamentoBimestre, tipoCalendario.Id, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);
            if (eventoFechamento == null || !eventoFechamento.Any())
                throw new NegocioException("Não foi possível localizar um fechamento de período ou reabertura para esta turma.");

            var professorRf = servicoUsuario.ObterRf();
            var professorPodePersistirTurma = await servicoEOL.ProfessorPodePersistirTurma(professorRf, turma.CodigoTurma, diaAtual);
            if (!professorPodePersistirTurma)
                throw new NegocioException("Você não pode executar alterações nesta turma.");
        }
    }
}