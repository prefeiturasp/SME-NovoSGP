import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import * as Yup from 'yup';
import {
  Base,
  Button,
  CampoData,
  Card,
  Colors,
  JoditEditor,
  Loader,
  momentSchema,
} from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import { RotasDto } from '~/dtos';
import {
  confirmar,
  erro,
  erros,
  setBreadcrumbManual,
  sucesso,
} from '~/servicos';
import ServicoRegistroItineranciaAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoRegistroItineranciaAEE';
import {
  CollapseAluno,
  ModalAlunos,
  ModalObjetivos,
  ModalUE,
  TabelaLinhaRemovivel,
} from './componentes';
import { NOME_CAMPO_QUESTAO } from './componentes/ConstantesCamposDinâmicos';

const RegistroItineranciaAEECadastro = ({ match }) => {
  const [refForm, setRefForm] = useState({});
  const valoresIniciaisFixos = { dataVisita: '', dataRetornoVerificacao: '' };
  const [valoresIniciais, setValoresIniciais] = useState(valoresIniciaisFixos);
  const validacoesFixas = {
    dataVisita: momentSchema.required('Campo obrigatório'),
    dataRetornoVerificacao: momentSchema.required('Campo obrigatório'),
  };
  const [validacoes, setValidacoes] = useState(Yup.object(validacoesFixas));
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [dataVisita, setDataVisita] = useState('');
  const [dataRetornoVerificacao, setDataRetornoVerificacao] = useState('');
  const [modalVisivelUES, setModalVisivelUES] = useState(false);
  const [modalVisivelObjetivos, setModalVisivelObjetivos] = useState(false);
  const [modalVisivelAlunos, setModalVisivelAlunos] = useState(false);
  const [objetivosSelecionados, setObjetivosSelecionados] = useState();
  const [alunosSelecionados, setAlunosSelecionados] = useState();
  const [uesSelecionados, setUesSelecionados] = useState();
  const [modoEdicao, setModoEdicao] = useState(false);
  const [objetivosBase, setObjetivosBase] = useState([]);
  const [itineranciaId, setItineranciaId] = useState();
  const [questoesAlunos, setQuestoesAluno] = useState([]);
  const [itineranciaAlteracao, setItineranciaAlteracao] = useState({});

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA];
  const [questoesItinerancia, setQuestoesItinerancia] = useState([]);

  const onClickVoltar = () => {};

  const onClickSalvar = async () => {
    const itinerancia = {
      id: itineranciaId,
      dataVisita,
      dataRetornoVerificacao,
      objetivosVisita: objetivosSelecionados,
      ues: uesSelecionados,
      alunos: alunosSelecionados,
      questoes: alunosSelecionados?.length ? [] : questoesItinerancia,
    };
    if (!itinerancia.objetivosVisita?.length) {
      erro('A itinerância precisa ter ao menos um objetivo selecionado');
      return;
    }
    if (!itinerancia.ues?.length) {
      erro(
        'A itinerância precisa ter ao menos uma unidade escolar selecionada'
      );
      return;
    }
    let errosAlunos = '';
    if (itinerancia.alunos?.length) {
      itinerancia.alunos.forEach(aluno => {
        const questoesInvalidas = aluno.questoes.filter(
          questao => questao.obrigatorio && !questao.resposta
        );
        if (questoesInvalidas.length) {
          const camposInvalidos = questoesInvalidas.map(questao => {
            return ` '${questao.descricao}'`;
          });
          errosAlunos += `O(s) campo(s) ${camposInvalidos} do aluno ${aluno.alunoNome}, são obrigatórios. `;
        }
      });
    }
    if (errosAlunos) {
      erro(errosAlunos);
    } else {
      setCarregandoGeral(true);
      const salvar = await ServicoRegistroItineranciaAEE.salvarItinerancia(
        itinerancia
      )
        .catch(e => erros(e))
        .finally(setCarregandoGeral(false));
      if (salvar?.status === 200) {
        sucesso(`Registro ${itineranciaId ? 'alterado' : 'salvo'} com sucesso`);
        setModoEdicao(false);
      }
    }
  };

  const perguntarAntesDeInserirAluno = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Ao selecionar o estudante, o registro será específico por estudante. As informações preenchidas até o momento serão descartadas',
      'Deseja continuar?'
    );
    return resposta;
  };

  const selecionarAlunos = async alunos => {
    const questao = questoesItinerancia.find(q => q.resposta);
    if (alunosSelecionados?.length === 0 && questao) {
      const pergunta = await perguntarAntesDeInserirAluno();
      if (!pergunta) {
        return;
      }
    }
    setAlunosSelecionados(alunos);
    refForm.setFieldValue('alunos', alunos);
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

  const montarValidacoesValoresQuestoes = (
    questoes,
    dtVisita = null,
    dtRetorno = null
  ) => {
    const validacoesQuestoes = validacoesFixas;
    const valoresIniciaisQuestoes = valoresIniciaisFixos;
    valoresIniciaisQuestoes.dataVisita = dtVisita || dataVisita;
    valoresIniciaisQuestoes.dataRetornoVerificacao =
      dtRetorno || dataRetornoVerificacao;
    questoes.forEach(questao => {
      validacoesQuestoes[
        NOME_CAMPO_QUESTAO + questao.questaoId
      ] = Yup.string().test(
        `validaCampoQuestao-${questao.questaoId}`,
        'campo obrigatório',
        function validar() {
          const { alunos } = this.parent;
          if (questao.obrigatorio && !alunos?.length && !questao.resposta) {
            return false;
          }
          return true;
        }
      );
      valoresIniciais[NOME_CAMPO_QUESTAO + questao.questaoId] =
        questao.resposta || '';
    });
    setValidacoes(Yup.object(validacoesQuestoes));
    setValoresIniciais(valoresIniciaisQuestoes);
  };

  const obterQuestoes = async () => {
    const result = await ServicoRegistroItineranciaAEE.obterQuestoesItinerancia();
    if (result?.status === 200) {
      setQuestoesItinerancia(result?.data?.itineranciaQuestao);
      setQuestoesAluno(result?.data?.itineranciaAlunoQuestao);
      montarValidacoesValoresQuestoes(result.data.itineranciaQuestao);
    }
  };

  const resetTela = () => {
    refForm.resetForm();
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
    refForm.setFieldValue('dataVisita', window.moment(itinerancia.dataVisita));
    refForm.setFieldValue(
      'dataRetornoVerificacao',
      window.moment(itinerancia.dataRetornoVerificacao)
    );
    setDataVisita(window.moment(itinerancia.dataVisita));
    setDataRetornoVerificacao(
      window.moment(itinerancia.dataRetornoVerificacao)
    );
    valoresIniciais.dataVisita = window.moment(itinerancia.dataVisita);
    valoresIniciais.dataRetornoVerificacao = window.moment(
      itinerancia.dataRetornoVerificacao
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
      montarValidacoesValoresQuestoes(
        itinerancia.questoes,
        window.moment(itinerancia.dataVisita),
        window.moment(itinerancia.dataRetornoVerificacao)
      );
    }
    if (itinerancia.alunos?.length) {
      setAlunosSelecionados(itinerancia.alunos);
      refForm.setFieldValue('alunos', itinerancia.alunos);
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
      const result = await ServicoRegistroItineranciaAEE.obterItineranciaPorId(
        id
      ).catch(e => erros(e));
      if (result.data && result?.status === 200) {
        const itinerancia = result.data;
        setItineranciaAlteracao(itinerancia);
        construirItineranciaAlteracao(itinerancia);
      }
    }
    if (itineranciaId) {
      obterItinerancia(itineranciaId);
    } else {
      obterQuestoes();
    }
  }, [itineranciaId]);

  const removerAlunos = alunoCodigo => {
    const novosAlunos =
      alunosSelecionados.filter(item => item.alunoCodigo !== alunoCodigo) || [];
    if (!novosAlunos.length) {
      montarValidacoesValoresQuestoes(questoesItinerancia);
    }
    setAlunosSelecionados(novosAlunos);
    setModoEdicao(true);
    refForm.setFieldValue('alunos', novosAlunos);
  };

  useEffect(() => {
    setCarregandoGeral(true);
    obterObjetivos();
    setCarregandoGeral(false);
  }, []);

  const desabilitarData = dataCorrente => {
    return (
      dataCorrente > window.moment() ||
      dataCorrente < window.moment().startOf('year')
    );
  };

  const desabilitarCamposPorPermissao = () => {
    return match?.params?.id
      ? !permissoesTela?.podeAlterar
      : !permissoesTela?.podeIncluir;
  };

  const permiteApenasUmaUe = () => {
    if (objetivosSelecionados?.length) {
      const objetivosComVariasUes = objetivosSelecionados.filter(
        objetivo => objetivo.permiteVariasUes
      );
      const objetivosComApenasUmaUe = objetivosSelecionados.filter(
        objetivo => !objetivo.permiteVariasUes
      );
      return (
        (!objetivosComVariasUes?.length && objetivosComApenasUmaUe?.length) ||
        alunosSelecionados?.length
      );
    }
    return false;
  };

  const validaAntesDoSubmit = form => {
    const camposValidacao = Object.keys(valoresIniciaisFixos);
    if (!alunosSelecionados?.length) {
      questoesItinerancia.forEach(questao => {
        camposValidacao.push(NOME_CAMPO_QUESTAO + questao.questaoId);
      });
    }
    if (camposValidacao.length) {
      camposValidacao.forEach(campo => {
        form.setFieldTouched(campo, true, true);
      });
      form.validateForm().then(() => {
        if (form.isValid || Object.keys(form.errors).length === 0) {
          form.submitForm(form);
        }
      });
    }
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
          <Formik
            enableReinitialize
            initialValues={valoresIniciais}
            ref={refFormik => setRefForm(refFormik)}
            validationSchema={validacoes}
            onSubmit={() => onClickSalvar()}
            validateOnBlur
            validateOnChange
          >
            {form => (
              <Form className="col-md-12 mb-4">
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
                        disabled
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
                        onClick={() => validaAntesDoSubmit(form)}
                        disabled={!modoEdicao}
                      />
                    </div>
                  </div>
                  <div className="row mb-4">
                    <div className="col-3">
                      <CampoData
                        name="dataVisita"
                        form={form}
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
                      desabilitadoIncluir={!permissoesTela?.podeIncluir}
                      desabilitadoExcluir={!permissoesTela?.podeAlterar}
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
                      desabilitadoIncluir={!permissoesTela?.podeIncluir}
                      desabilitadoExcluir={
                        !permissoesTela?.podeAlterar ||
                        alunosSelecionados?.length
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
                          Estudantes
                        </span>
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
                        />
                      </div>
                    </div>
                  )}
                  {alunosSelecionados?.length
                    ? alunosSelecionados.map(aluno => (
                        <CollapseAluno
                          key={aluno.alunoCodigo}
                          aluno={aluno}
                          removerAlunos={() => removerAlunos(aluno.alunoCodigo)}
                          setModoEdicaoItinerancia={setModoEdicao}
                        />
                      ))
                    : questoesItinerancia?.map(questao => {
                        return (
                          <div className="row mb-4" key={questao.questaoId}>
                            <div className="col-12">
                              <JoditEditor
                                form={form}
                                label={questao.descricao}
                                value={questao.resposta}
                                name={NOME_CAMPO_QUESTAO + questao.questaoId}
                                id={`editor-questao-${questao.questaoId}`}
                                onChange={e => setQuestao(e, questao)}
                              />
                            </div>
                          </div>
                        );
                      })}
                  <div className="row mb-4">
                    <div className="col-3">
                      <CampoData
                        form={form}
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
              </Form>
            )}
          </Formik>
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
            permissoesTela?.podeAlterar || alunosSelecionados?.length
          }
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
