using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasPorUsuarioQueryHandler : ConsultasBase, IRequestHandler<ObterPendenciasPorUsuarioQuery, PaginacaoResultadoDto<PendenciaDto>>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IMediator mediator;

        public ObterPendenciasPorUsuarioQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioPendencia repositorioPendencia) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<PaginacaoResultadoDto<PendenciaDto>> Handle(ObterPendenciasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var pendencias = new PaginacaoResultadoDto<Pendencia>();

            if (!string.IsNullOrEmpty(request.TurmaCodigo) || !string.IsNullOrEmpty(request.TituloPendencia) || request.TipoPendencia > 0)
            {
                int[] tiposPendenciasAFiltrar = request.TipoPendencia > 0 ? RetornaTiposPendenciaGrupo((TipoPendenciaGrupo)request.TipoPendencia).ToArray() : new int[] { };
                pendencias = await repositorioPendencia.ListarPendenciasUsuarioComFiltro(request.UsuarioId,
                                                                                             tiposPendenciasAFiltrar.ToArray(),
                                                                                             request.TituloPendencia,
                                                                                             request.TurmaCodigo,
                                                                                             Paginacao,
                                                                                             request.TipoPendencia);

                var itensDaLista = pendencias.Items.ToList();

                if (!string.IsNullOrEmpty(request.TurmaCodigo) && request.TipoPendencia == 0)
                {
                    foreach (var pendencia in pendencias.Items)
                    {
                        var pendenciaFiltrada = await repositorioPendencia
                                                       .FiltrarListaPendenciasUsuario(request.TurmaCodigo,
                                                                                      pendencia);

                        if (pendenciaFiltrada == null)
                            itensDaLista.Remove(pendencia);
                    }
                }

                pendencias.Items = itensDaLista;
            }
            else
            {
                pendencias = await repositorioPendencia.ListarPendenciasUsuarioSemFiltro(request.UsuarioId,
                                                                                             Paginacao);
            }        

            return await MapearParaDtoPaginado(pendencias);
        }

        public IEnumerable<int> RetornaTiposPendenciaGrupo(TipoPendenciaGrupo tipoGrupo)
        {
            var tiposPendencias = Enum.GetValues(typeof(TipoPendencia))
                           .Cast<TipoPendencia>()
                           .Select(d => new { codigo = (int)d, descricao = d.ObterNomeGrupo() })
                           .Where(d => d.descricao == tipoGrupo.Name())
                           .ToList();
            return tiposPendencias.Select(tp => tp.codigo);
        }

        private async Task<PaginacaoResultadoDto<PendenciaDto>> MapearParaDtoPaginado(PaginacaoResultadoDto<Pendencia> pendenciasPaginadas)
        {
            return new PaginacaoResultadoDto<PendenciaDto>()
            {
                Items = await MapearParaDto(pendenciasPaginadas.Items),
                TotalPaginas = pendenciasPaginadas.TotalPaginas,
                TotalRegistros = pendenciasPaginadas.TotalRegistros
            };
        }

        private async Task<IEnumerable<PendenciaDto>> MapearParaDto(IEnumerable<Pendencia> pendencias)
        {
            var listaPendenciasDto = new List<PendenciaDto>();
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            foreach (var pendencia in pendencias)
                listaPendenciasDto.AddRange(await ObterPendencias(pendencia, usuarioLogado.CodigoRf));

            return listaPendenciasDto;
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendencias(Pendencia pendencia, string codigoRf)
        {
            return pendencia.EhPendenciaAula() ?
                        await ObterPendenciasAulasFormatadas(pendencia) :
                    pendencia.EhPendenciaAusenciaAvaliacaoProfessor() ?
                        await ObterPendenciaAusenciaAvaliacaoProfessor(pendencia) :
                    pendencia.EhPendenciaAusenciaAvaliacaoCP() ?
                        await ObterDescricaoPendenciaAusenciaAvaliacaoCP(pendencia): 
                    pendencia.EhPendenciaFechamento() ?
                        await ObterPendenciasFechamentoFormatadas(pendencia) :
                        pendencia.EhAusenciaFechamento() ?
                        await ObterPendenciasProfessorFormatadas(pendencia) :
                    pendencia.EhPendenciaDiarioBordo() ?
                        await ObterPendenciasDiarioBordoFormatadas(pendencia, codigoRf):
                    new List<PendenciaDto>();
        }
        
        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasProfessorFormatadas(Pendencia pendencia)
        {
            var turma = await mediator.Send(new ObterTurmaDaPendenciaProfessorQuery(pendencia.Id));

            return new List<PendenciaDto>
            {
                new PendenciaDto()
                {
                    Tipo = pendencia.Tipo.GroupName(),
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Detalhe = pendencia.Descricao,
                    Turma = ObterNomeTurma(turma),
                    Bimestre = await ObterBimestreTurma(pendencia)
                }
            };
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasFechamentoFormatadas(Pendencia pendencia)
        {
            var turma = await mediator.Send(new ObterTurmaDaPendenciaFechamentoQuery(pendencia.Id));

            var descricao = string.Empty;

            if (pendencia.EhPendenciaCadastroEvento())
                descricao = await ObterDescricaoPendenciaEvento(pendencia);

            if (pendencia.EhPendenciaAusenciaDeRegistroIndividual())
                descricao = await ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(pendencia);

            return new List<PendenciaDto>
            {
                new PendenciaDto()
                {
                    Tipo = pendencia.Tipo.GroupName(),
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Detalhe = descricao,
                    Turma = ObterNomeTurma(turma),
                    Bimestre = await ObterBimestreTurma(pendencia)
                }
            };
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciaAusenciaAvaliacaoProfessor(Pendencia pendencia)
        {
            var pendenciasProfessorBimestre = await mediator.Send(new ObterPendenciasProfessorPorPendenciaIdQuery(pendencia.Id));

            var pendenciasProfessor = new List<PendenciaDto>();

            var pendenciaFormatada = new PendenciaDto
            {
                Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                Tipo = pendencia.Tipo.GroupName(),
                Bimestre = ObterNomeBimestre(pendenciasProfessorBimestre.FirstOrDefault().Bimestre),
                Turma = $"{((Modalidade)pendenciasProfessorBimestre.FirstOrDefault().ModalidadeCodigo).ShortName()} - {pendenciasProfessorBimestre.FirstOrDefault().NomeTurma}"
            };

            var descricao = new StringBuilder(pendencia.Descricao);
            
            descricao.AppendLine("<br /><ul>");

            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            pendenciaFormatada.Detalhe = descricao.ToString();
            pendenciasProfessor.Add(pendenciaFormatada);

            return pendenciasProfessor;
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasAulasFormatadas(Pendencia pendencia)
        {
            var pendenciasAulas = await mediator.Send(new ObterPendenciasAulasPorPendenciaQuery(pendencia.Id));

            var agrupamentoPendenciasBimestres = pendenciasAulas.GroupBy(g => new { g.Bimestre,g.DisciplinaId, g.ModalidadeCodigo, g.NomeTurma }, (key, group) =>
                                                        new PendenciaAgrupamentoDto()
                                                        {
                                                            Bimestre = key.Bimestre,
                                                            ModalidadeCodigo = key.ModalidadeCodigo,
                                                            NomeTurma = key.NomeTurma,
                                                            PendenciaDetalhes = group.Select(s => new PendenciaDetalheDto()
                                                            {
                                                                DataAula = s.DataAula,
                                                                EhReposicao = s.EhReposicao
                                                            })
                                                        });

            return ObterPendenciasFormatadas(pendencia, agrupamentoPendenciasBimestres);
        }

        private async Task<IEnumerable<PendenciaDto>> ObterPendenciasDiarioBordoFormatadas(Pendencia pendencia, string codigoRf)
        {
            var pendenciasDiarios = await mediator.Send(new ObterPendenciasDiarioPorPendenciaIdEProfessorQuery(pendencia.Id, codigoRf));

            var agrupamentoPendenciasBimestres = pendenciasDiarios.GroupBy(g => new { g.Bimestre, g.ModalidadeCodigo, g.NomeTurma }, (key, group) =>
                                                        new PendenciaAgrupamentoDto()
                                                        {
                                                            Bimestre = key.Bimestre,
                                                            ModalidadeCodigo = key.ModalidadeCodigo,
                                                            NomeTurma = key.NomeTurma,
                                                            PendenciaDetalhes = group.Select(s => new PendenciaDetalheDto()
                                                            {
                                                                DataAula = s.DataAula,
                                                                EhReposicao = s.EhReposicao
                                                            })
                                                        });

            return ObterPendenciasFormatadas(pendencia, agrupamentoPendenciasBimestres);
        }

        private IEnumerable<PendenciaDto> ObterPendenciasFormatadas(Pendencia pendencia, IEnumerable<PendenciaAgrupamentoDto> agrupamentoPendenciasBimestres)
        {
            var pendenciasDiarioBordo = new List<PendenciaDto>();

            var descricao = new StringBuilder();

            foreach (var pendenciaBimestre in agrupamentoPendenciasBimestres)
            {
                descricao = new StringBuilder(pendencia.Descricao);

                descricao.AppendLine("<br /><ul>");

                var pendenciaFormatada = new PendenciaDto
                {
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Tipo = pendencia.Tipo.GroupName(),
                    Bimestre = ObterNomeBimestre(pendenciaBimestre.Bimestre),
                    Turma = $"{((Modalidade)pendenciaBimestre.ModalidadeCodigo).ShortName()} - {pendenciaBimestre.NomeTurma}"
                };

                foreach (var pendenciaDetalhe in pendenciaBimestre.PendenciaDetalhes)
                    descricao.AppendLine($"<li>{pendenciaDetalhe.DataAula:dd/MM/yyyy} {ObterComplementoReposicao(pendenciaDetalhe.EhReposicao)}</li>");

                descricao.AppendLine("</ul>");
                descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

                pendenciaFormatada.Detalhe = descricao.ToString();
                pendenciasDiarioBordo.Add(pendenciaFormatada);

            }
            return pendenciasDiarioBordo;
        }

        private async Task<string> ObterBimestreTurma(Pendencia pendencia)
        {
            Turma turma = pendencia.EhPendenciaAula() ?
                         await mediator.Send(new ObterTurmaDaPendenciaAulaQuery(pendencia.Id)) :
                    pendencia.EhPendenciaFechamento() ?
                         await mediator.Send(new ObterTurmaDaPendenciaFechamentoQuery(pendencia.Id)) :
                    pendencia.EhPendenciaProfessor() ?
                        await mediator.Send(new ObterTurmaDaPendenciaProfessorQuery(pendencia.Id)) :
                    pendencia.EhPendenciaDiarioBordo()?
                         await mediator.Send(new ObterTurmaDaPendenciaDiarioQuery(pendencia.Id)) :
                        null;

            if (turma == null)
                return "";
            else
                return await ObterDescricaoBimestrePendencia(pendencia.Id, turma.Id, pendencia.CriadoEm);
        }

        private async Task<string> ObterDescricaoBimestrePendencia(long pendenciaId, long turmaId, DateTime dataPendenciaCriada)
        {
            int bimestre = await mediator.Send(new ObterModalidadePorPendenciaQuery(pendenciaId, turmaId, dataPendenciaCriada));
            return ObterNomeBimestre(bimestre);
        }

        private string ObterNomeTurma(Turma turma)
            => turma != null ? $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}" : "";

        private string ObterNomeBimestre(int bimestre)
            => bimestre == 0 ? "Final" : $"{bimestre}º Bimestre";

        private async Task<string> ObterDescricaoPendencia(Pendencia pendencia )
        {           
            if (pendencia.EhPendenciaCadastroEvento())
                return await ObterDescricaoPendenciaEvento(pendencia);
           
            
            if (pendencia.EhPendenciaAusenciaDeRegistroIndividual())
                return await ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(pendencia);

            return ObterDescricaoPendenciaGeral(pendencia);
        }

        private async Task<IEnumerable<PendenciaDto>> ObterDescricaoPendenciaAusenciaAvaliacaoCP(Pendencia pendencia)
        {
            var pendenciasProfessor = await mediator.Send(new ObterPendenciasProfessorPorPendenciaIdQuery(pendencia.Id));

            var pendenciasCP = new List<PendenciaDto>();

            var pendenciaFormatada = new PendenciaDto
            {
                Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                Tipo = pendencia.Tipo.GroupName(),
                Bimestre = ObterNomeBimestre(pendenciasProfessor.FirstOrDefault().Bimestre),
                Turma = $"{((Modalidade)pendenciasProfessor.FirstOrDefault().ModalidadeCodigo).ShortName()} - {pendenciasProfessor.FirstOrDefault().NomeTurma}"
            };

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.Append("<br/><table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.Append("<tr>");
            descricao.Append("<td style='padding: 5px;'><b>Componente curricular</b></td>");
            descricao.Append("<td style='padding: 5px;'><b>Professor titular</b></td>");
            descricao.Append("</tr>");
            foreach (var pendenciaProfessor in pendenciasProfessor)
            {
                descricao.Append("<tr style='padding:5px'>");
                descricao.Append($"<td style='padding: 5px;'>{pendenciaProfessor.ComponenteCurricular}</td>");
                descricao.Append($"<td style='padding: 5px;'>{pendenciaProfessor.Professor}({pendenciaProfessor.ProfessorRf})</td>");
                descricao.Append("</tr>");
            }
            descricao.Append("</table><br/>");
            descricao.Append($"<b>{pendencia.Instrucao}</b>");

            pendenciaFormatada.Detalhe = descricao.ToString();
            pendenciasCP.Add(pendenciaFormatada);

            return pendenciasCP;
        }
                


        private string ObterDescricaoPendenciaGeral(Pendencia pendencia)
        {
            return ObterDescricaoPendenciaGeral(pendencia.Descricao,pendencia.Instrucao);
        }

        private string ObterDescricaoPendenciaGeral(string descricao, string instrucao)
        {
            return $"{descricao}<br /><br/><b>{instrucao}</b>";
        }

        private async Task<string> ObterDescricaoPendenciaEvento(Pendencia pendencia)
        {
            var pendenciasEventos = await mediator.Send(new ObterPendenciasParametroEventoPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul>");

            foreach (var pendenciaEvento in pendenciasEventos)
            {
                descricao.AppendLine($"<li>{pendenciaEvento.Descricao} ({pendenciaEvento.Valor})</li>");
            }
            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }

        private string ObterComplementoReposicao(bool ehReposicao)
        {
            return ehReposicao ? " - Reposição" : "";
        }

        private async Task<string> ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(Pendencia pendencia)
        {
            var alunos = await mediator.Send(new ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul style='padding-top: 20px; padding-left: 20px';>");

            foreach (var aluno in alunos)
            {
                descricao.AppendLine($"<li style='padding-top: 2px;'>{aluno.NomeValido()} ({aluno.CodigoAluno})</li>");
            }
            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }
    }
}