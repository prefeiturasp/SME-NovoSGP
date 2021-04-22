import moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import {
  Auditoria,
  Base,
  Button,
  CampoData,
  Card,
  Colors,
  JoditEditor,
  Loader,
  MarcadorSituacao,
  PainelCollapse,
  SelectComponent,
} from '~/componentes';
import { Cabecalho, Paginacao } from '~/componentes-sgp';
import { RotasDto } from '~/dtos';
import {
  confirmar,
  erros,
  setBreadcrumbManual,
  setSomenteConsultaManual,
  sucesso,
  verificaSomenteConsulta,
  history,
  ServicoCalendarios,
} from '~/servicos';
import ServicoRegistroItineranciaAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoRegistroItineranciaAEE';
import { ordenarPor } from '~/utils/funcoes/gerais';
import { BotaoCustomizado } from '../registroItinerancia.css';
import {
  CollapseAluno,
  ModalAlunos,
  ModalErrosItinerancia,
  ModalObjetivos,
  ModalUE,
  TabelaLinhaRemovivel,
} from './componentes';
import { NOME_CAMPO_QUESTAO } from './componentes/ConstantesCamposDinâmicos';

const RegistroItineranciaAEECadastro = ({ match }) => {
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [carregandoQuestoes, setCarregandoQuestoes] = useState(false);
  const [dataVisita, setDataVisita] = useState('');
  const [dataRetornoVerificacao, setDataRetornoVerificacao] = useState('');
  const [modalVisivelUES, setModalVisivelUES] = useState(false);
  const [modalVisivelObjetivos, setModalVisivelObjetivos] = useState(false);
  const [modalVisivelAlunos, setModalVisivelAlunos] = useState(false);
  const [objetivosSelecionados, setObjetivosSelecionados] = useState([]);
  const [alunosSelecionados, setAlunosSelecionados] = useState([]);
  const [uesSelecionados, setUesSelecionados] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [objetivosBase, setObjetivosBase] = useState([]);
  const [itineranciaId, setItineranciaId] = useState();
  const [questoesAlunos, setQuestoesAluno] = useState([]);
  const [itineranciaAlteracao, setItineranciaAlteracao] = useState({});
  const [errosValidacao, setErrosValidacao] = useState([]);
  const [modalErrosVisivel, setModalErrosVisivel] = useState(false);
  const [questoesItinerancia, setQuestoesItinerancia] = useState([]);
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [paginaAtual, setPaginaAtual] = useState(1);
  const [auditoria, setAuditoria] = useState();
  const [imprimindo, setImprimindo] = useState(false);
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [carregandoEventos, setCarregandoEventos] = useState(false);
  const [listaCalendario, setListaCalendario] = useState([]);
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState();
  const [listaEvento, setListaEvento] = useState([]);
  const [eventoId, setEventoId] = useState();

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA];

  const permissaoStatus = itineranciaId && !itineranciaAlteracao?.podeEditar;

  const onClickSalvar = () => {
    const itinerancia = {
      id: itineranciaId,
      dataVisita,
      dataRetornoVerificacao: dataRetornoVerificacao || '',
      objetivosVisita: objetivosSelecionados,
      ues: uesSelecionados,
      alunos: alunosSelecionados,
      questoes: alunosSelecionados?.length ? [] : questoesItinerancia,
      anoLetivo: new Date().getFullYear(),
      eventoId,
    };
    const camposComErro = [];
    if (!dataVisita) {
      camposComErro.push('O campo data da visita é obrigatório');
    }
    if (!itinerancia.objetivosVisita?.length) {
      camposComErro.push(
        'A itinerância precisa ter ao menos um objetivo selecionado'
      );
    }
    if (!itinerancia.ues?.length) {
      camposComErro.push(
        'A itinerância precisa ter ao menos uma unidade escolar selecionada'
      );
    }
    if (itinerancia.alunos?.length) {
      itinerancia.alunos.forEach(aluno => {
        const questoesAlunoInvalidas = aluno.questoes.filter(
          questao => questao.obrigatorio && !questao.resposta
        );
        if (questoesAlunoInvalidas.length) {
          const camposInvalidos = questoesAlunoInvalidas.map(questao => {
            return ` '${questao.descricao}'`;
          });
          camposComErro.push(
            `O(s) campo(s) ${camposInvalidos} do aluno ${aluno.alunoNome}, são obrigatórios. `
          );
        }
      });
    } else {
      const questoesInvalidas = questoesItinerancia.filter(
        questao => questao.obrigatorio && !questao.resposta
      );
      questoesInvalidas.forEach(questao => {
        camposComErro.push(`O campo ${questao.descricao} é obrigatório. `);
      });
    }
    if (
      dataVisita &&
      dataRetornoVerificacao &&
      dataRetornoVerificacao <= dataVisita
    ) {
      camposComErro.push(
        'A data de retorno/verificação não pode ser menor que a data de visita'
      );
    }
    if (camposComErro.length) {
      setErrosValidacao(camposComErro);
      setModalErrosVisivel(true);
    } else {
      setCarregandoGeral(true);
      ServicoRegistroItineranciaAEE.salvarItinerancia(itinerancia)
        .then(resp => {
          if (resp?.status === 200) {
            sucesso(
              `Registro ${itineranciaId ? 'alterado' : 'salvo'} com sucesso`
            );
            setModoEdicao(false);
            setCarregandoGeral(false);
            history.push(RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA);
          }
        })
        .catch(e => {
          erros(e);
          setCarregandoGeral(false);
        });
    }
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) onClickSalvar();
      else history.push(RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA);
    } else {
      history.push(RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA);
    }
  };

  const selecionarAlunos = async alunos => {
    const questoes = questoesItinerancia.filter(q => q.resposta);
    if (!alunosSelecionados?.length && questoes?.length) {
      const resposta = await confirmar(
        'Atenção',
        'Ao selecionar o estudante, o registro será específico por estudante. As informações preenchidas até o momento serão descartadas',
        'Deseja continuar?'
      );
      if (resposta) {
        setAlunosSelecionados(ordenarPor(alunos, 'alunoNome'));
        questoesItinerancia.forEach(questao => {
          questao.resposta = '';
        });
      }
    } else {
      setAlunosSelecionados(ordenarPor(alunos, 'alunoNome'));
    }
  };

  const mudarDataVisita = data => {
    setDataVisita(data);
    setModoEdicao(true);
  };

  const mudarDataRetorno = data => {
    setDataRetornoVerificacao(data);
    setModoEdicao(true);
  };

  const removerObjetivoSelecionado = valor => {
    const itemLista = objetivosBase.find(
      objetivo =>
        objetivo.itineranciaObjetivoBaseId === valor.itineranciaObjetivoBaseId
    );
    if (itemLista) itemLista.descricao = '';
    setObjetivosSelecionados(estadoAntigo =>
      estadoAntigo
        ? estadoAntigo.filter(
            item =>
              item.itineranciaObjetivoBaseId !== valor.itineranciaObjetivoBaseId
          )
        : []
    );
    setModoEdicao(true);
  };

  const removerUeSelecionada = text => {
    setUesSelecionados(estadoAntigo =>
      estadoAntigo.filter(item => item.key !== text.key)
    );
    setModoEdicao(true);
  };

  useEffect(() => {
    if (match?.params?.id) {
      setBreadcrumbManual(
        match?.url,
        'Alterar',
        RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA
      );
      setItineranciaId(match.params.id);
    }
  }, [match]);

  const obterObjetivos = async () => {
    const retorno = await ServicoRegistroItineranciaAEE.obterObjetivos().catch(
      e => erros(e)
    );
    if (retorno?.data) {
      const dadosAlterados = retorno.data.map(item => ({
        ...item,
        key: item.id,
      }));
      setObjetivosBase(dadosAlterados);
    }
  };

  const obterQuestoes = async () => {
    setCarregandoQuestoes(true);
    const result = await ServicoRegistroItineranciaAEE.obterQuestoesItinerancia();
    if (result?.status === 200) {
      setQuestoesItinerancia(result?.data?.itineranciaQuestao);
      setQuestoesAluno(result?.data?.itineranciaAlunoQuestao);
      setCarregandoQuestoes(false);
    }
  };

  const resetTela = () => {
    setDataVisita('');
    setDataRetornoVerificacao('');
    setObjetivosSelecionados([]);
    setUesSelecionados([]);
    questoesItinerancia.forEach(questao => {
      questao.resposta = '';
    });
    setAlunosSelecionados([]);
  };

  const construirItineranciaAlteracao = itinerancia => {
    setDataVisita(window.moment(itinerancia.dataVisita));
    setDataRetornoVerificacao(
      itinerancia.dataRetornoVerificacao
        ? window.moment(itinerancia.dataRetornoVerificacao)
        : ''
    );
    if (itinerancia.objetivosVisita?.length) {
      setObjetivosSelecionados(itinerancia.objetivosVisita);
      itinerancia.objetivosVisita.forEach(objetivo => {
        let objetivoBase = objetivosBase.find(
          o =>
            o.itineranciaObjetivosBaseId === objetivo.itineranciaObjetivosBaseId
        );
        objetivoBase = objetivo;
        objetivoBase.checked = true;
      });
    }
    if (itinerancia.ues?.length) {
      itinerancia.ues.forEach(ue => {
        ue.key = ue.codigoUe;
      });
      setUesSelecionados(itinerancia.ues);
    }
    if (itinerancia.questoes?.length) {
      setQuestoesItinerancia(itinerancia.questoes);
    }
    if (itinerancia.alunos?.length) {
      setAlunosSelecionados(itinerancia.alunos);
    }

    if (itinerancia.tipoCalendarioId) {
      setTipoCalendarioSelecionado(String(itinerancia.tipoCalendarioId));
    }

    if (itinerancia.eventoId) {
      setEventoId(String(itinerancia.eventoId));
    }
  };

  const perguntarAntesDeCancelar = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );
    return resposta;
  };
  const onClickCancelar = async () => {
    if (modoEdicao) {
      const ehParaCancelar = await perguntarAntesDeCancelar();
      if (ehParaCancelar) {
        setModoEdicao(false);
        if (itineranciaId) {
          construirItineranciaAlteracao(itineranciaAlteracao);
        } else {
          resetTela();
        }
      }
    }
  };

  useEffect(() => {
    async function obterItinerancia(id) {
      setCarregandoGeral(true);
      const result = await ServicoRegistroItineranciaAEE.obterItineranciaPorId(
        id
      ).catch(e => erros(e));
      if (result?.data && result?.status === 200) {
        const itinerancia = result.data;
        setItineranciaAlteracao(itinerancia);
        setSomenteConsulta(itinerancia.criadoRF !== usuario.rf);
        setSomenteConsultaManual(itinerancia.criadoRF !== usuario.rf);
        construirItineranciaAlteracao(itinerancia);
        setAuditoria(itinerancia.auditoria);
      }
      setCarregandoGeral(false);
    }
    if (itineranciaId) {
      obterItinerancia(itineranciaId);
    } else {
      obterQuestoes();
    }
  }, [itineranciaId]);

  const perguntarAntesDeRemoverAluno = async () => {
    const resposta = await confirmar(
      'Atenção',
      'As informações preenchidas para o aluno serão descartadas',
      'Deseja realmente remover o aluno?'
    );
    return resposta;
  };

  const confirmarRemoverAluno = alunoCodigo => {
    const novosAlunos =
      alunosSelecionados.filter(item => item.alunoCodigo !== alunoCodigo) || [];
    setPaginaAtual(Math.ceil(novosAlunos.length / 10 || 1));
    setAlunosSelecionados(novosAlunos);
    setModoEdicao(true);
  };

  const removerAlunos = async alunoCodigo => {
    const alunoRemover = alunosSelecionados.find(
      aluno => aluno.alunoCodigo === alunoCodigo
    );
    const temQuestoesComResposta = alunoRemover.questoes.filter(
      q => q.resposta
    );
    if (temQuestoesComResposta?.length) {
      const pergunta = await perguntarAntesDeRemoverAluno();
      if (pergunta) {
        confirmarRemoverAluno(alunoCodigo);
      }
    } else {
      confirmarRemoverAluno(alunoCodigo);
    }
  };

  useEffect(() => {
    obterObjetivos();
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  const desabilitarDataVisita = dataCorrente => {
    return (
      dataCorrente > window.moment() ||
      dataCorrente < window.moment().startOf('year')
    );
  };

  const desabilitarDataRetorno = dataCorrente => {
    return (
      dataCorrente > window.moment().endOf('year') ||
      (dataVisita
        ? dataCorrente <= window.moment(dataVisita).add(1, 'd')
        : dataCorrente < window.moment().startOf('year'))
    );
  };

  const desabilitarCamposPorPermissao = () => {
    return (
      (match?.params?.id
        ? !permissoesTela?.podeAlterar
        : !permissoesTela?.podeIncluir) ||
      somenteConsulta ||
      permissaoStatus
    );
  };

  const permiteApenasUmaUe = () => {
    if (objetivosSelecionados?.length) {
      const objetivosComApenasUmaUe = objetivosSelecionados.filter(
        objetivo => !objetivo.permiteVariasUes
      );
      return objetivosComApenasUmaUe?.length > 0;
    }
    return false;
  };

  const setQuestao = (valor, questao) => {
    setModoEdicao(true);
    questao.resposta = valor;
  };

  const possuiApenasUesInfantil = () => {
    return uesSelecionados.length && uesSelecionados[0].ehInfantil;
  };

  const gerarRelatorio = () => {
    setImprimindo(true);

    ServicoRegistroItineranciaAEE.gerarRelatorio([match?.params?.id])
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .finally(setImprimindo(false))
      .catch(e => erros(e));
  };

  const selecionaTipoCalendario = tipo => {
    setEventoId();
    setListaEvento([]);

    if (tipo) {
      setTipoCalendarioSelecionado(tipo);
    } else {
      setTipoCalendarioSelecionado();
    }
    setModoEdicao(true);
  };

  const hasAnoLetivoClause = t =>
    moment(dataVisita).format('YYYY') ?? false
      ? String(t.anoLetivo) === moment(dataVisita).format('YYYY')
      : true;

  const hasActiveSituation = t => t.situacao;

  const filterAllowedCalendarTypes = data => {
    return data.filter(hasAnoLetivoClause).filter(hasActiveSituation);
  };

  const loadTiposCalendarioEffect = () => {
    let isSubscribed = true;

    (async () => {
      setCarregandoTipos(true);

      const { data } = await ServicoCalendarios
        .obterTiposCalendarioAutoComplete
        // pesquisaTipoCalendario
        ();

      if (isSubscribed) {
        const allowedList = filterAllowedCalendarTypes(data);
        setListaCalendario(allowedList);
        if (allowedList?.length === 1) {
          selecionaTipoCalendario(allowedList[0].id);
        }
        setCarregandoTipos(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  };

  useEffect(() => {
    if (dataVisita) {
      loadTiposCalendarioEffect();
    }
  }, [dataVisita]);

  const obterListaEventos = async (tipoCalendarioId, id) => {
    setCarregandoEventos(true);
    const retorno = await ServicoRegistroItineranciaAEE.obterEventos(
      tipoCalendarioId,
      id
    )
      .catch(e => erros(e))
      .finally(() => setCarregandoEventos(false));

    if (retorno?.data?.length) {
      setListaEvento(retorno.data);
    } else {
      setEventoId();
      setListaEvento([]);
    }
  };

  useEffect(() => {
    if (tipoCalendarioSelecionado) {
      obterListaEventos(tipoCalendarioSelecionado, itineranciaId);
    } else {
      setEventoId();
      setListaEvento([]);
    }
  }, [tipoCalendarioSelecionado, itineranciaId]);

  const selecionaEvento = evento => {
    if (evento) {
      setEventoId(evento);
    } else {
      setEventoId(evento);
    }
    setModoEdicao(true);
  };

  return (
    <>
      <Cabecalho pagina="Registro de itinerância" />
      <Loader loading={carregandoGeral}>
        <Card>
          <div className="col-12 p-0">
            <div className="row mb-5">
              <div className="col-md-12 d-flex justify-content-end">
                <Button
                  id="btn-voltar-ata-diario-bordo"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-3"
                  onClick={onClickVoltar}
                />
                <Button
                  id="btn-cancelar-ata-diario-bordo"
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-3"
                  onClick={onClickCancelar}
                  disabled={!modoEdicao}
                />
                <Button
                  id="btn-gerar-ata-diario-bordo"
                  label="Salvar"
                  color={Colors.Roxo}
                  border
                  bold
                  onClick={() => onClickSalvar()}
                  disabled={!modoEdicao || somenteConsulta}
                />
              </div>
            </div>
            {itineranciaId && (
              <div className="row mb-4">
                <div className="col-sm-12 d-flex justify-content-between align-items-center">
                  <div className="pr-4">
                    <Loader loading={imprimindo} ignorarTip>
                      <BotaoCustomizado
                        border
                        id="btn-imprimir-relatorio-itinerancia"
                        className="btn-imprimir"
                        icon="print"
                        color={Colors.Azul}
                        width="38px"
                        onClick={() => gerarRelatorio()}
                      />
                    </Loader>
                  </div>
                  <div>
                    {itineranciaAlteracao?.statusWorkflow && (
                      <MarcadorSituacao corFundo={Colors.Azul}>
                        {itineranciaAlteracao?.statusWorkflow}
                      </MarcadorSituacao>
                    )}
                  </div>
                </div>
              </div>
            )}
            <div className="row mb-4 mt-2">
              <div className="col-3">
                <CampoData
                  name="dataVisita"
                  formatoData="DD/MM/YYYY"
                  valor={dataVisita}
                  label="Data da visita"
                  placeholder="Selecione a data"
                  onChange={mudarDataVisita}
                  desabilitarData={desabilitarDataVisita}
                  desabilitado={desabilitarCamposPorPermissao()}
                />
              </div>
            </div>
            <div className="row mb-4">
              <div className="col-6">
                <Loader loading={carregandoTipos} tip="">
                  <SelectComponent
                    id="tipo-calendario"
                    label="Tipo de Calendário"
                    lista={listaCalendario}
                    valueOption="id"
                    valueText="descricao"
                    onChange={selecionaTipoCalendario}
                    valueSelect={tipoCalendarioSelecionado}
                    placeholder="Selecione um calendário"
                    showSearch
                  />
                </Loader>
              </div>
              <div className="col-6">
                <Loader loading={carregandoEventos} tip="">
                  <SelectComponent
                    id="evento"
                    label="Evento"
                    lista={listaEvento}
                    valueOption="id"
                    valueText="nome"
                    onChange={selecionaEvento}
                    valueSelect={eventoId}
                    placeholder="Selecione um evento"
                    showSearch
                  />
                </Loader>
              </div>
            </div>
            <div className="row mb-4">
              <TabelaLinhaRemovivel
                bordered
                ordenacao
                dataIndex="nome"
                labelTabela="Objetivos da itinerância"
                tituloTabela="Objetivos selecionados"
                labelBotao="Novo objetivo"
                desabilitadoIncluir={
                  !permissoesTela?.podeIncluir ||
                  somenteConsulta ||
                  permissaoStatus
                }
                desabilitadoExcluir={
                  !permissoesTela?.podeAlterar ||
                  somenteConsulta ||
                  permissaoStatus
                }
                pagination={false}
                dadosTabela={objetivosSelecionados}
                removerUsuario={text => removerObjetivoSelecionado(text)}
                botaoAdicionar={() => setModalVisivelObjetivos(true)}
              />
            </div>
            <div className="row mb-4">
              <TabelaLinhaRemovivel
                bordered
                dataIndex="descricao"
                labelTabela="Selecione as Unidades Escolares"
                tituloTabela="Unidades Escolares selecionadas"
                labelBotao="Adicionar nova unidade escolar"
                pagination={false}
                desabilitadoIncluir={
                  !permissoesTela?.podeIncluir ||
                  somenteConsulta ||
                  permissaoStatus
                }
                desabilitadoExcluir={
                  !permissoesTela?.podeAlterar ||
                  alunosSelecionados?.length ||
                  somenteConsulta ||
                  permissaoStatus
                }
                dadosTabela={uesSelecionados}
                removerUsuario={text => removerUeSelecionada(text)}
                botaoAdicionar={() => setModalVisivelUES(true)}
              />
            </div>
            {uesSelecionados?.length === 1 && (
              <div className="row mb-4">
                <div className="col-12 font-weight-bold mb-2">
                  <span style={{ color: Base.CinzaMako }}>
                    {possuiApenasUesInfantil() ? 'Crianças' : 'Estudantes'}
                  </span>
                </div>
                <div className="col-12">
                  <Button
                    id={shortid.generate()}
                    label={`Adicionar ${
                      possuiApenasUesInfantil()
                        ? 'nova criança'
                        : 'novo estudante'
                    }`}
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={() => setModalVisivelAlunos(true)}
                    icon="user-plus"
                    disabled={desabilitarCamposPorPermissao()}
                  />
                </div>
              </div>
            )}
            {alunosSelecionados?.length ? (
              <>
                <div className="row mb-4">
                  <div className="col-12">
                    <PainelCollapse accordion onChange={() => {}}>
                      {alunosSelecionados
                        .slice(paginaAtual * 10 - 10, paginaAtual * 10)
                        .map(aluno => (
                          <PainelCollapse.Painel
                            key={`painel-${aluno.alunoCodigo}`}
                            accordion
                            espacoPadrao
                            corBorda={Base.AzulBordaCollapse}
                            temBorda
                            header={aluno.nomeAlunoComTurmaModalidade}
                          >
                            <CollapseAluno
                              key={aluno.alunoCodigo}
                              aluno={aluno}
                              removerAlunos={() =>
                                removerAlunos(aluno.alunoCodigo)
                              }
                              setModoEdicaoItinerancia={setModoEdicao}
                              desabilitar={desabilitarCamposPorPermissao()}
                            />
                          </PainelCollapse.Painel>
                        ))}
                    </PainelCollapse>
                  </div>
                </div>

                <div className="row">
                  <div className="col-12 d-flex justify-content-center mt-4">
                    <Paginacao
                      numeroRegistros={alunosSelecionados.length}
                      pageSize={10}
                      onChangePaginacao={e => setPaginaAtual(e)}
                    />
                  </div>
                </div>
              </>
            ) : (
              questoesItinerancia?.map(questao => {
                return (
                  <Loader loading={carregandoQuestoes}>
                    <div className="row mb-4" key={questao.questaoId}>
                      <div className="col-12">
                        <JoditEditor
                          label={questao.descricao}
                          value={questao.resposta}
                          name={NOME_CAMPO_QUESTAO + questao.questaoId}
                          id={`editor-questao-${questao.questaoId}`}
                          onChange={e => setQuestao(e, questao)}
                          desabilitar={desabilitarCamposPorPermissao()}
                        />
                      </div>
                    </div>
                  </Loader>
                );
              })
            )}
            <div className="row mb-4">
              <div className="col-3">
                <CampoData
                  name="dataRetornoVerificacao"
                  formatoData="DD/MM/YYYY"
                  valor={dataRetornoVerificacao}
                  label="Data para retorno/verificação"
                  placeholder="Selecione a data"
                  onChange={mudarDataRetorno}
                  desabilitarData={desabilitarDataRetorno}
                  desabilitado={desabilitarCamposPorPermissao()}
                />
              </div>
            </div>
          </div>
          {auditoria && (
            <Auditoria
              criadoEm={auditoria.criadoEm}
              criadoPor={auditoria.criadoPor}
              criadoRf={auditoria.criadoRf}
              alteradoPor={auditoria.alteradoPor}
              alteradoEm={auditoria.alteradoEm}
              alteradoRf={auditoria.alteradoRf}
            />
          )}
        </Card>
      </Loader>
      {modalVisivelUES && (
        <ModalUE
          modalVisivel={modalVisivelUES}
          setModalVisivel={setModalVisivelUES}
          unEscolaresSelecionados={uesSelecionados}
          setUnEscolaresSelecionados={setUesSelecionados}
          permiteApenasUmaUe={permiteApenasUmaUe()}
          setModoEdicaoItinerancia={setModoEdicao}
          desabilitarBotaoExcluir={
            !permissoesTela?.podeAlterar || alunosSelecionados?.length
          }
          temAlunosSelecionados={alunosSelecionados?.length}
        />
      )}
      {modalVisivelObjetivos && (
        <ModalObjetivos
          modalVisivel={modalVisivelObjetivos}
          setModalVisivel={setModalVisivelObjetivos}
          objetivosSelecionados={objetivosSelecionados}
          setObjetivosSelecionados={setObjetivosSelecionados}
          listaObjetivos={objetivosBase}
          variasUesSelecionadas={uesSelecionados?.length > 1}
          setModoEdicaoItinerancia={setModoEdicao}
        />
      )}
      {modalVisivelAlunos && (
        <ModalAlunos
          modalVisivel={modalVisivelAlunos}
          setModalVisivel={setModalVisivelAlunos}
          alunosSelecionados={alunosSelecionados}
          setAlunosSelecionados={selecionarAlunos}
          codigoUe={uesSelecionados.length && uesSelecionados[0].codigoUe}
          questoes={questoesAlunos}
          setModoEdicaoItinerancia={setModoEdicao}
          dataVisita={dataVisita}
        />
      )}
      {modalErrosVisivel && (
        <ModalErrosItinerancia
          modalVisivel={modalErrosVisivel}
          setModalVisivel={setModalErrosVisivel}
          erros={errosValidacao}
        />
      )}
    </>
  );
};

RegistroItineranciaAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

RegistroItineranciaAEECadastro.defaultProps = {
  match: {},
};

export default RegistroItineranciaAEECadastro;
