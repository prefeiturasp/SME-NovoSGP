using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using SME.SGP.Dominio.Constantes;

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

        public async Task<AulasPrevistasDadasAuditoriaDto> Inserir(AulaPrevistaDto dto)
        {
            var turma = await ObterTurma(dto.TurmaId);

            var tipoCalendario = await ObterTipoCalendarioPorTurmaAnoLetivo(turma.AnoLetivo, turma.ModalidadeCodigo, turma.Semestre);

            AulaPrevista aulaPrevista = null;
            aulaPrevista = MapearParaDominio(dto, aulaPrevista, tipoCalendario.Id);

            unitOfWork.IniciarTransacao();

            var aulaPrevistaDto = await Inserir(dto, aulaPrevista);
            var nomeChave = string.Format(NomeChaveCache.AULAS_PREVISTAS_UE, turma.UeId);
            await mediator.Send(new CriarCacheAulaPrevistaCommand(nomeChave, turma.UeId));

            unitOfWork.PersistirTransacao();

            var aulasPrevistasConsulta = await ObterBimestres(aulaPrevistaDto.Id);

            return MapearDtoRetorno(aulaPrevista, aulasPrevistasConsulta);
        }

        private async Task<AulaPrevistaDto> Inserir(AulaPrevistaDto aulaPrevistaDto, AulaPrevista aulaPrevista)
        {
            aulaPrevistaDto.Id = repositorio.Salvar(aulaPrevista);

            if (aulaPrevistaDto.BimestresQuantidade.Count() > 4)
                throw new NegocioException("O número de bimestres passou do limite padrão. Favor entrar em contato com o suporte.");

            if (aulaPrevistaDto.BimestresQuantidade != null)
            {
                var aulasPrevistasBimestres = await mediator.Send(new ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQuery(aulaPrevista.Id, aulaPrevistaDto.BimestresQuantidade.Select(a => a.Bimestre).ToArray()));
                foreach (var bimestreQuantidadeDto in aulaPrevistaDto.BimestresQuantidade)
                {
                    var aulaJaPrevistaNoBimestre = aulasPrevistasBimestres.Any(a => a.Bimestre == bimestreQuantidadeDto.Bimestre);

                    await repositorioAulaPrevistaBimestre.SalvarAsync(new AulaPrevistaBimestre()
                    {
                        Id = !aulaJaPrevistaNoBimestre ? 0 : aulasPrevistasBimestres.FirstOrDefault(a => a.Bimestre == bimestreQuantidadeDto.Bimestre).Id,
                        AulaPrevistaId = aulaPrevistaDto.Id,
                        Bimestre = bimestreQuantidadeDto.Bimestre,
                        Previstas = bimestreQuantidadeDto.Quantidade
                    });
                }
            }

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearDtoRetorno(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> aulasPrevistasBimestre)
        {
            if (aulasPrevistasBimestre.Any())
                aulasPrevistasBimestre = aulasPrevistasBimestre.DistinctBy(a => a.Bimestre).ToList();

            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto = MapearParaDto(aulaPrevista, aulasPrevistasBimestre) ?? new AulasPrevistasDadasAuditoriaDto();

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearParaDto(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> bimestres = null)
        {
            var bimestre = bimestres.FirstOrDefault();

            return aulaPrevista == null ? null : new AulasPrevistasDadasAuditoriaDto
            {
                Id = aulaPrevista.Id,
                AlteradoEm = bimestre?.AlteradoEm ?? DateTime.MinValue,
                AlteradoPor = bimestre?.AlteradoPor ?? "",
                AlteradoRF = bimestre?.AlteradoRF ?? "",
                CriadoEm = bimestre?.CriadoEm ?? aulaPrevista.CriadoEm,
                CriadoPor = bimestre?.CriadoPor ?? aulaPrevista.CriadoPor,
                CriadoRF = bimestre?.CriadoRF ?? aulaPrevista.CriadoRF,
                AulasPrevistasPorBimestre = bimestres?.Select(x => new AulasPrevistasDadasDto
                {
                    Bimestre = x.Bimestre,
                    Criadas = new AulasQuantidadePorProfessorDto()
                    {
                        QuantidadeCJ = x.CriadasCJ,
                        QuantidadeTitular = x.CriadasTitular
                    },
                    Cumpridas = x.LancaFrequencia || x.Cumpridas > 0 ? x.Cumpridas : x.CumpridasSemFrequencia,
                    Inicio = x.Inicio,
                    Fim = x.Fim,
                    Previstas = new AulasPrevistasDto() { Quantidade = x.Previstas, Mensagens = MapearMensagens(x)},
                    Reposicoes = x.LancaFrequencia || x.Reposicoes != 0 ? x.Reposicoes : x.ReposicoesSemFrequencia,
                    PodeEditar = true
                }).ToList()
            };
        }

        private string[] MapearMensagens(AulaPrevistaBimestreQuantidade aula)
        {
            var mensagens = new List<string>();

            if (aula != null)
            {
                if (aula.Previstas != (aula.CriadasCJ + aula.CriadasTitular) && aula.Fim.Date >= DateTime.Today)
                    mensagens.Add("Quantidade de aulas previstas diferente da quantidade de aulas criadas.");

                int aulaCumprida = aula.LancaFrequencia || aula.Cumpridas > 0 ? aula.Cumpridas : aula.CumpridasSemFrequencia;

                if(aula.Previstas != (aulaCumprida + aula.Reposicoes) && aula.Fim.Date < DateTime.Today)
                    mensagens.Add("Quantidade de aulas previstas diferente do somatório de aulas dadas + aulas repostas, após o final do bimestre.");
            }

            return mensagens.ToArray();
        }

        private async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestres(long? aulaPrevistaId)
        {
            return await repositorioAulaPrevistaBimestreConsulta.ObterBimestresAulasPrevistasPorId(aulaPrevistaId);
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
