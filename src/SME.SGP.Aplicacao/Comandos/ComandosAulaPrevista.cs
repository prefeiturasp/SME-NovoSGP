using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ComandosAulaPrevista : IComandosAulaPrevista
    {
        private readonly IRepositorioAulaPrevista repositorio;
        private readonly IRepositorioAulaPrevistaBimestre repositorioAulaPrevistaBimestre;
        private readonly IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestreConsulta;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ComandosAulaPrevista(IRepositorioAulaPrevista repositorio,
                                    IRepositorioAulaPrevistaBimestre repositorioAulaPrevistaBimestre,
                                    IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestreConsulta,
                                    IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta,
                                    IUnitOfWork unitOfWork,
                                    IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioAulaPrevistaBimestre = repositorioAulaPrevistaBimestre ?? throw new ArgumentNullException(nameof(repositorioAulaPrevistaBimestre));
            this.repositorioAulaPrevistaBimestreConsulta = repositorioAulaPrevistaBimestreConsulta ?? throw new ArgumentNullException(nameof(repositorioAulaPrevistaBimestreConsulta));
            this.repositorioTipoCalendarioConsulta = repositorioTipoCalendarioConsulta ?? throw new ArgumentNullException(nameof(repositorioTipoCalendarioConsulta));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Alterar(AulaPrevistaDto dto, long id)
        {
            IEnumerable<AulaPrevistaBimestre> aulasPrevistasBimestre = await repositorioAulaPrevistaBimestreConsulta.ObterBimestresAulasPrevistasPorId(id);

            unitOfWork.IniciarTransacao();

            if (dto.BimestresQuantidade.Count() > 4)
                throw new NegocioException("O número de bimestres passou do limite padrão. Favor entrar em contato com o suporte.");

            foreach (var bimestre in dto.BimestresQuantidade)
            {
                AulaPrevistaBimestre aulaPrevistaBimestre = aulasPrevistasBimestre.FirstOrDefault(b => b.Bimestre == bimestre.Bimestre);
                aulaPrevistaBimestre = MapearParaDominio(id, bimestre, aulaPrevistaBimestre);
                repositorioAulaPrevistaBimestre.Salvar(aulaPrevistaBimestre);
            }

            unitOfWork.PersistirTransacao();

            return "Alteração realizada com sucesso";
        }

        public async Task<long> Inserir(AulaPrevistaDto dto)
        {
            var turma = await ObterTurma(dto.TurmaId);

            var tipoCalendario = await ObterTipoCalendarioPorTurmaAnoLetivo(turma.AnoLetivo, turma.ModalidadeCodigo, turma.Semestre);

            long id;

            AulaPrevista aulaPrevista = null;
            aulaPrevista = MapearParaDominio(dto, aulaPrevista, tipoCalendario.Id);

            unitOfWork.IniciarTransacao();

            id = await Inserir(dto, aulaPrevista);

            unitOfWork.PersistirTransacao();

            return id;
        }

        private async Task<long> Inserir(AulaPrevistaDto aulaPrevistaDto, AulaPrevista aulaPrevista)
        {
            aulaPrevistaDto.Id = repositorio.Salvar(aulaPrevista);

            if (aulaPrevistaDto.BimestresQuantidade.Count() > 4)
                throw new NegocioException("O número de bimestres passou do limite padrão. Favor entrar em contato com o suporte.");

            if (aulaPrevistaDto.BimestresQuantidade != null)
                foreach (var bimestreQuantidadeDto in aulaPrevistaDto.BimestresQuantidade)
                    await repositorioAulaPrevistaBimestre.SalvarAsync(new AulaPrevistaBimestre()
                    {
                        AulaPrevistaId = aulaPrevistaDto.Id,
                        Bimestre = bimestreQuantidadeDto.Bimestre,
                        Previstas = bimestreQuantidadeDto.Quantidade
                    });

            return aulaPrevistaDto.Id;
        }

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            return turma;
        }

        private async Task<TipoCalendario> ObterTipoCalendarioPorTurmaAnoLetivo(int anoLetivo, Modalidade turmaModalidade, int semestre)
        {
            var tipoCalendario = await repositorioTipoCalendarioConsulta.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeParaModalidadeTipoCalendario(turmaModalidade), semestre);

            if (tipoCalendario == null)
                throw new NegocioException("Tipo calendário não encontrado!");

            return tipoCalendario;
        }

        private AulaPrevista MapearParaDominio(AulaPrevistaDto aulaPrevistaDto, AulaPrevista aulaPrevista, long tipoCalendarioId)
        {
            if (aulaPrevista == null)
            {
                aulaPrevista = new AulaPrevista();
            }
            aulaPrevista.DisciplinaId = aulaPrevistaDto.DisciplinaId;
            aulaPrevista.TipoCalendarioId = tipoCalendarioId;
            aulaPrevista.TurmaId = aulaPrevistaDto.TurmaId;
            return aulaPrevista;
        }

        private AulaPrevistaBimestre MapearParaDominio(long aulaPrevistaId,
                                               AulaPrevistaBimestreQuantidadeDto bimestreQuantidadeDto,
                                               AulaPrevistaBimestre aulaPrevistaBimestre)
        {
            if (aulaPrevistaBimestre == null)
            {
                aulaPrevistaBimestre = new AulaPrevistaBimestre();
            }
            aulaPrevistaBimestre.AulaPrevistaId = aulaPrevistaId;
            aulaPrevistaBimestre.Bimestre = bimestreQuantidadeDto.Bimestre;
            aulaPrevistaBimestre.Previstas = bimestreQuantidadeDto.Quantidade;
            return aulaPrevistaBimestre;
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }
    }
}
