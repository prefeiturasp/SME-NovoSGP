import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import {
  Base,
  Button,
  CampoData,
  Card,
  Colors,
  JoditEditor,
  Loader,
  PainelCollapse,
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
} from '~/servicos';
import ServicoRegistroItineranciaAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoRegistroItineranciaAEE';
import { ordenarPor } from '~/utils/funcoes/gerais';
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

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA];

  const onClickVoltar = () => {
    history.push(RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA);
  };

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
      dataRetornoVerificacao < dataVisita
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
            history.push(RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA);
          }
        })
        .catch(e => erros(e))
        .finally(setCarregandoGeral(false));
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
    const result = await ServicoRegistroItineranciaAEE.obterQuestoesItinerancia();
    if (result?.status === 200) {
      setQuestoesItinerancia(result?.data?.itineranciaQuestao);
      setQuestoesAluno(result?.data?.itineranciaAlunoQuestao);
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
      setUesSelecionados(itinerancia.ues);
    }
    if (itinerancia.questoes?.length) {
      setQuestoesItinerancia(itinerancia.questoes);
    }
    if (itinerancia.alunos?.length) {
      setAlunosSelecionados(itinerancia.alunos);
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
      )
        .catch(e => erros(e))
        .finally(setCarregandoGeral(false));
      if (result?.data && result?.status === 200) {
        const itinerancia = result.data;
        setItineranciaAlteracao(itinerancia);
        setSomenteConsulta(itinerancia.criadoRF !== usuario.rf);
        setSomenteConsultaManual(itinerancia.criadoRF !== usuario.rf);
        construirItineranciaAlteracao(itinerancia);
      }
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
    setCarregandoGeral(true);
    obterObjetivos();
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
    setCarregandoGeral(false);
  }, []);

  const desabilitarData = dataCorrente => {
    return (
      dataCorrente > window.moment() ||
      dataCorrente < window.moment().startOf('year')
    );
  };

  const desabilitarCamposPorPermissao = () => {
    return (
      (match?.params?.id
        ? !permissoesTela?.podeAlterar
        : !permissoesTela?.podeIncluir) || somenteConsulta
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
            <div className="row mb-4">
              <div className="col-3">
                <CampoData
                  name="dataVisita"
                  formatoData="DD/MM/YYYY"
                  valor={dataVisita}
                  label="Data da visita"
                  placeholder="Selecione a data"
                  onChange={mudarDataVisita}
                  desabilitarData={desabilitarData}
                  desabilitado={desabilitarCamposPorPermissao()}
                />
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
                  !permissoesTela?.podeIncluir || somenteConsulta
                }
                desabilitadoExcluir={
                  !permissoesTela?.podeAlterar || somenteConsulta
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
                  !permissoesTela?.podeIncluir || somenteConsulta
                }
                desabilitadoExcluir={
                  !permissoesTela?.podeAlterar ||
                  alunosSelecionados?.length ||
                  somenteConsulta
                }
                dadosTabela={uesSelecionados}
                removerUsuario={text => removerUeSelecionada(text)}
                botaoAdicionar={() => setModalVisivelUES(true)}
              />
            </div>
            {uesSelecionados?.length === 1 && (
              <div className="row mb-4">
                <div className="col-12 font-weight-bold mb-2">
                  <span style={{ color: Base.CinzaMako }}>Estudantes</span>
                </div>
                <div className="col-12">
                  <Button
                    id={shortid.generate()}
                    label="Adicionar novo estudante"
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
                  desabilitado={desabilitarCamposPorPermissao()}
                />
              </div>
            </div>
          </div>
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
