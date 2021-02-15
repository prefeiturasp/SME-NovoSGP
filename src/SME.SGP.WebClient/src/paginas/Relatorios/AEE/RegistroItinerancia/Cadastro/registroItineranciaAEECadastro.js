import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
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
import { setQuestoesItineranciaAluno } from '~/redux/modulos/itinerancia/action';
import { erros, setBreadcrumbManual, sucesso } from '~/servicos';
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
  const dispatch = useDispatch();
  const [refForm, setRefForm] = useState({});
  const valoresIniciaisFixos = { dataVisita: '', dataRetornoVerificacao: '' };
  const [valoresIniciais, setValoresIniciais] = useState(valoresIniciaisFixos);
  const validacoesFixas = {
    dataVisita: momentSchema.required('Campo obrigatório'),
    dataRetornoVerificacao: momentSchema.required('Campo obrigatório'),
  };
  const [validacoes, setValidacoes] = useState(Yup.object(validacoesFixas));
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [dataVisita, setDataVisita] = useState();
  const [dataRetornoVerificacao, setDataRetornoVerificacao] = useState();
  const [modalVisivelUES, setModalVisivelUES] = useState(false);
  const [modalVisivelObjetivos, setModalVisivelObjetivos] = useState(false);
  const [modalVisivelAlunos, setModalVisivelAlunos] = useState(false);
  const [objetivosSelecionados, setObjetivosSelecionados] = useState();
  const [alunosSelecionados, setAlunosSelecionados] = useState();
  const [uesSelecionados, setUesSelecionados] = useState();
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [objetivosBase, setObjetivosBase] = useState([]);
  const [itineranciaId, setItineranciaId] = useState();
  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA];
  const [questoesItinerancia, setQuestoesItinerancia] = useState([]);

  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
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
    setCarregandoGeral(true);
    const salvar = await ServicoRegistroItineranciaAEE.salvarItinerancia(
      itinerancia
    )
      .catch(e => erros(e))
      .finally(setCarregandoGeral(false));
    if (salvar?.status === 200) {
      sucesso(`Registro ${itineranciaId ? 'alterado' : 'salvo'} com sucesso`);
    }
  };

  const mudarDataVisita = data => {
    setDataVisita(data);
  };

  const mudarDataRetorno = data => {
    setDataRetornoVerificacao(data);
  };

  const removerItemSelecionado = (text, funcao) => {
    funcao(estadoAntigo => estadoAntigo.filter(item => item.key !== text.key));
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

  const montarValidacoesValoresQuestoes = questoes => {
    const validacoesQuestoes = validacoesFixas;
    const valoresIniciaisQuestoes = valoresIniciaisFixos;
    valoresIniciaisQuestoes.dataVisita = dataVisita;
    valoresIniciaisQuestoes.dataRetornoVerificacao = dataRetornoVerificacao;
    questoes.forEach(questao => {
      validacoesQuestoes[
        NOME_CAMPO_QUESTAO + questao.questaoId
      ] = Yup.string().required('Campo obrigatório');
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
      dispatch(
        setQuestoesItineranciaAluno(result?.data?.itineranciaAlunoQuestao)
      );
      montarValidacoesValoresQuestoes(result.data.itineranciaQuestao);
    }
  };

  useEffect(() => {
    setCarregandoGeral(true);
    obterObjetivos();
    obterQuestoes();
    setCarregandoGeral(false);
  }, []);

  useEffect(() => {
    if (alunosSelecionados?.length) {
      montarValidacoesValoresQuestoes([]);
    } else {
      montarValidacoesValoresQuestoes(
        questoesItinerancia?.length ? questoesItinerancia : []
      );
    }
  }, [alunosSelecionados]);

  useEffect(() => {
    async function obterItinerancia(id) {
      const result = await ServicoRegistroItineranciaAEE.obterItineranciaPorId(
        id
      ).catch(e => erros(e));
      if (result.data && result?.status === 200) {
        const itinerancia = result.data;
        setDataVisita(itinerancia.dataVisita);
        setDataRetornoVerificacao(itinerancia.dataRetornoVerificacao);
        valoresIniciais.dataVisita = window.moment(itinerancia.dataVisita);
        valoresIniciais.dataRetornoVerificacao = window.moment(
          itinerancia.dataRetornoVerificacao
        );
        if (itinerancia.objetivosVisita?.length) {
          setObjetivosSelecionados(itinerancia.objetivosVisita);
          itinerancia.objetivosVisita.forEach(objetivo => {
            let objetivoBase = objetivosBase.find(
              o =>
                o.itineranciaObjetivosBaseId ===
                objetivo.itineranciaObjetivosBaseId
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
      }
    }
    if (itineranciaId) {
      obterItinerancia(itineranciaId);
    }
  }, [itineranciaId]);

  const removerAlunos = alunoCodigo => {
    setAlunosSelecionados(estadoAntigo =>
      estadoAntigo.filter(item => item.alunoCodigo !== alunoCodigo)
    );
  };

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

  useEffect(() => {
    if (dataVisita && objetivosSelecionados?.length) {
      setDesabilitarCampos(true);
    }
  }, [dataVisita, objetivosSelecionados]);

  const permiteApenasUmaUe = () => {
    if (objetivosSelecionados?.length) {
      const objetivosComVariasUes = objetivosSelecionados.find(
        objetivo => objetivo.permiteVariasUes
      );
      const objetivosComApenasUmaUe = objetivosSelecionados.find(
        objetivo => !objetivo.permiteVariasUes
      );
      return !objetivosComVariasUes?.length && objetivosComApenasUmaUe?.length;
    }
    return false;
  };

  const validaAntesDoSubmit = form => {
    if (Object.keys(valoresIniciais).length) {
      const arrayCampos = Object.keys(valoresIniciais);
      arrayCampos.forEach(campo => {
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
            validationSchema={validacoes}
            ref={refFormik => setRefForm(refFormik)}
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
                        disabled={!desabilitarCampos}
                      />
                      <Button
                        id="btn-gerar-ata-diario-bordo"
                        label="Salvar"
                        color={Colors.Roxo}
                        border
                        bold
                        onClick={() => validaAntesDoSubmit(form)}
                        disabled={!desabilitarCampos}
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
                        desabilitarData={
                          desabilitarData || desabilitarCamposPorPermissao()
                        }
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
                      desabilitadoIncluir={permissoesTela?.podeIncluir}
                      desabilitadoExcluir={permissoesTela?.podeExcluir}
                      pagination={false}
                      dadosTabela={objetivosSelecionados}
                      removerUsuario={text =>
                        removerItemSelecionado(text, setObjetivosSelecionados)
                      }
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
                      desabilitadoIncluir={permissoesTela?.podeIncluir}
                      desabilitadoExcluir={permissoesTela?.podeExcluir}
                      dadosTabela={uesSelecionados}
                      removerUsuario={text =>
                        removerItemSelecionado(text, setUesSelecionados)
                      }
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
                        />
                      ))
                    : questoesItinerancia?.length &&
                      questoesItinerancia.map(questao => {
                        return (
                          <div className="row mb-4" key={questao.questaoId}>
                            <div className="col-12">
                              <JoditEditor
                                form={form}
                                label={questao.descricao}
                                value={questao.resposta}
                                name={NOME_CAMPO_QUESTAO + questao.questaoId}
                                id={questao.questaoId}
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
                        disabled={desabilitarCamposPorPermissao()}
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
        />
      )}
      {modalVisivelAlunos && (
        <ModalAlunos
          modalVisivel={modalVisivelAlunos}
          setModalVisivel={setModalVisivelAlunos}
          alunosSelecionados={alunosSelecionados}
          setAlunosSelecionados={setAlunosSelecionados}
          codigoUe={uesSelecionados.length && uesSelecionados[0].codigoUe}
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
