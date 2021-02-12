import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import * as Yup from 'yup';
import { useDispatch, useSelector } from 'react-redux';
import { Formik, Form } from 'formik';
import {
  Base,
  Button,
  CampoData,
  Card,
  Colors,
  Loader,
  momentSchema,
} from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import { RotasDto } from '~/dtos';
import { erros, setBreadcrumbManual, sucesso } from '~/servicos';
import {
  CollapseAluno,
  EditoresTexto,
  ModalAlunos,
  ModalObjetivos,
  ModalUE,
  TabelaLinhaRemovivel,
} from './componentes';
import ServicoRegistroItineranciaAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoRegistroItineranciaAEE';
import {
  setQuestoesItinerancia,
  setQuestoesItineranciaAluno,
  setObjetivosItinerancia,
} from '~/redux/modulos/itinerancia/action';

const RegistroItineranciaAEECadastro = ({ match }) => {
  const dispatch = useDispatch();
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    dataVisita: null,
    dataRetornoVerificacao: null,
  });
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
  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA];
  const questoesItinerancia = useSelector(
    store => store.itinerancia.questoesItinerancia
  );

  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
  const onClickSalvar = async () => {
    const itinerancia = {
      dataVisita,
      dataRetornoVerificacao,
      objetivosVisita: objetivosSelecionados,
      ues: uesSelecionados,
      alunos: alunosSelecionados,
      questoes: alunosSelecionados?.length ? [] : questoesItinerancia,
    };
    const salvar = await ServicoRegistroItineranciaAEE.salvarItinerancia(
      itinerancia
    ).catch(e => erros(e));
    if (salvar.status === 200) {
      sucesso('Registro salvo com sucesso');
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
    if (match?.params?.id)
      setBreadcrumbManual(
        match?.url,
        'Alterar',
        RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA
      );
  }, [match]);

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

  useEffect(() => {
    const buscarQuestoes = async () => {
      const result = await ServicoRegistroItineranciaAEE.obterQuestoesItinerancia();
      if (result?.status === 200) {
        dispatch(setQuestoesItinerancia(result?.data?.itineranciaQuestao));
        dispatch(
          setQuestoesItineranciaAluno(result?.data?.itineranciaAlunoQuestao)
        );
      }
    };
    buscarQuestoes();
    obterObjetivos();
  }, []);

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

  return (
    <>
      <Loader loading={carregandoGeral} className="w-100">
        <Cabecalho pagina="Registro de itinerância" />
        <Card>
          <Formik
            enableReinitialize
            initialValues={valoresIniciais}
            validationSchema={validacoes}
            ref={refFormik => setRefForm(refFormik)}
            onSubmit={valores => onClickSalvar(valores)}
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
                        onClick={onClickSalvar}
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
                      dataIndex="unidadeEscolar"
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
                  {alunosSelecionados?.length ? (
                    alunosSelecionados.map(aluno => (
                      <CollapseAluno
                        key={aluno.alunoCodigo}
                        aluno={aluno}
                        removerAlunos={() => removerAlunos(aluno.alunoCodigo)}
                      />
                    ))
                  ) : (
                    <EditoresTexto form={form} />
                  )}
                  <div className="row mb-4">
                    <div className="col-3">
                      <CampoData
                        form={form}
                        name="dataRetorno"
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
