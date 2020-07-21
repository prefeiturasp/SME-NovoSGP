import { Form, Formik } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import { Auditoria, CampoData, Loader, PainelCollapse } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Editor from '~/componentes/editor/editor';
import ModalMultiLinhas from '~/componentes/modalMultiLinhas';
import SelectComponent from '~/componentes/select';
import { URL_HOME } from '~/constantes/url';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

const DiarioBordo = () => {
  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada } = usuario;

  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const anoLetivo = turmaSelecionada ? turmaSelecionada.anoLetivo : 0;

  const [
    listaComponenteCurriculare,
    setListaComponenteCurriculare,
  ] = useState();
  const [
    componenteCurricularSelecionado,
    setComponenteCurricularSelecionado,
  ] = useState();
  const [ehTurmaInfantil, setEhTurmaInfantil] = useState(true);
  const [dataSelecionada, setDataSelecionada] = useState();
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [ehRegencia, setEhRegencia] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [listaDatasAulas, setListaDatasAulas] = useState([]);
  const [diasParaHabilitar, setDiasParaHabilitar] = useState();
  const [errosValidacao, setErrosValidacao] = useState([]);
  const [mostrarErros, setMostarErros] = useState(false);
  const [auditoria, setAuditoria] = useState('');

  const [valoresIniciais, setValoresIniciais] = useState({
    planejamento: '',
    reflexoesReplanejamentos: '',
    devolutivas: '',
  });

  const validacoes = Yup.object({
    planejamento: Yup.string()
      .required('Campo planejamento é obrigatório')
      .min(
        200,
        'Campo planejamento é obrigatório ter no mínimo 200 caracteres'
      ),
  });

  const resetarTela = form => {
    form.resetForm();
    setDataSelecionada('');
    setModoEdicao(false);
  };

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    // TODO Verificar se vai mudar o endpoint!
    const componentes = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaId
    ).catch(e => erros(e));

    if (componentes.data && componentes.data.length) {
      setListaComponenteCurriculare(componentes.data);

      if (componentes.data.length === 1) {
        const componente = componentes.data[0];
        setComponenteCurricularSelecionado(
          String(componente.codigoComponenteCurricular)
        );
        setEhRegencia(componente.regencia);
        // TODO - alterar o valor fixo!
        setEhTurmaInfantil(true);
      }
    }

    setCarregandoGeral(false);
  }, [turmaId]);

  useEffect(() => {
    if (turmaId) {
      obterComponentesCurriculares();
    }
  }, [turmaId, obterComponentesCurriculares]);

  const obterDatasDeAulasDisponiveis = useCallback(async () => {
    setCarregandoGeral(true);
    // TODO Verificar se vai mudar o enpoint!
    const datasDeAulas = await api
      .get(
        `v1/calendarios/frequencias/aulas/datas/${anoLetivo}/turmas/${turmaId}/disciplinas/${componenteCurricularSelecionado}`
      )
      .catch(e => {
        setCarregandoGeral(false);
        erros(e);
      });

    setCarregandoGeral(false);
    if (datasDeAulas && datasDeAulas.data && datasDeAulas.data.length) {
      setListaDatasAulas(datasDeAulas.data);
      const habilitar = datasDeAulas.data.map(item =>
        window.moment(item.data).format('YYYY-MM-DD')
      );
      setDiasParaHabilitar(habilitar);
    } else {
      setListaDatasAulas();
      setDiasParaHabilitar();
    }
  }, [anoLetivo, turmaId, componenteCurricularSelecionado]);

  useEffect(() => {
    if (turmaId && componenteCurricularSelecionado) {
      obterDatasDeAulasDisponiveis();
    }
  }, [turmaId, componenteCurricularSelecionado, obterDatasDeAulasDisponiveis]);

  const onChangeComponenteCurricular = valor => {
    if (valor) {
      const componente = listaComponenteCurriculare.find(
        item => item.codigoComponenteCurricular == valor
      );
      // TODO - alterar o valor fixo!
      componente.ehTurmaInfantil = true;
      setEhTurmaInfantil(componente.ehTurmaInfantil);
    } else {
      setDiasParaHabilitar([]);
    }
    setDataSelecionada('');
    setComponenteCurricularSelecionado(valor);
  };

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const salvarDiarioDeBordo = (valores, form) => {
    return new Promise((resolve, reject) => {
      console.log(valores);
      // TODO Chamar post para salvar os registros, caso de erro retornar reject(false);
      setCarregandoGeral(true);
      setTimeout(() => {
        sucesso('Diário de bordo salvo com sucesso.');
        resetarTela(form);
        setCarregandoGeral(false);
        resolve(true);
      }, 3000);
    });
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    return form.validateForm().then(() => {
      if (Object.keys(form.errors).length > 0) {
        setErrosValidacao([form.errors.planejamento]);
        setMostarErros(true);
      } else {
        setErrosValidacao([]);
        setMostarErros(false);
      }

      if (form.isValid || Object.keys(form.errors).length == 0) {
        return salvarDiarioDeBordo(form.values, form);
        // form.handleSubmit(e => e);
      }
      return false;
    });
  };

  const obterAulaSelecionada = useCallback(
    async data => {
      if (listaDatasAulas) {
        const aulaDataSelecionada = listaDatasAulas.filter(item => {
          return (
            window.moment(item.data).format('DD/MM/YYYY') ===
            window.moment(data).format('DD/MM/YYYY')
          );
        });

        return aulaDataSelecionada;
      }
      return null;
    },
    [listaDatasAulas]
  );

  const validaSeTemIdAula = useCallback(
    async (data, form) => {
      form.resetForm();
      setModoEdicao(false);
      const aulaDataSelecionada = await obterAulaSelecionada(data);
      console.log('Aula selecionada:' + aulaDataSelecionada);
      // TODO Chamar endpoint para obter os registros!
    },
    [obterAulaSelecionada]
  );

  const onChangeData = async (data, form) => {
    if (modoEdicao) {
      const confirmarParaSalvar = await pergutarParaSalvar();
      if (confirmarParaSalvar) {
        const salvoComSucesso = await validaAntesDoSubmit();
        if (salvoComSucesso) {
          setDataSelecionada(data);
          await validaSeTemIdAula(data, form);
        }
      } else {
        setDataSelecionada(data);
        await validaSeTemIdAula(data, form);
      }
    } else {
      setDataSelecionada(data);
      await validaSeTemIdAula(data, form);
    }
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        resetarTela(form);
      }
    }
  };

  const onClickVoltar = async form => {
    if (modoEdicao) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        validaAntesDoSubmit(form);
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const onCloseErros = () => {
    setErrosValidacao([]);
    setMostarErros(false);
  };

  return (
    <Loader loading={carregandoGeral} className="w-100 my-2">
      {usuario && turmaSelecionada.turma ? null : (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'diario-bordo-selecione-turma',
            mensagem: 'Você precisa escolher uma turma',
          }}
          className="mb-2"
        />
      )}
      {componenteCurricularSelecionado && !ehTurmaInfantil ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'eh-turma-infantil',
            mensagem:
              'Esta interface só pode ser utilizada para turmas da educação infantil',
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
      <ModalMultiLinhas
        key="erros-diario-bordo"
        visivel={mostrarErros}
        onClose={onCloseErros}
        type="error"
        conteudo={errosValidacao}
        titulo="Erros diário de bordo"
      />
      <Cabecalho pagina="Diário de bordo" />
      <Card>
        <div className="col-md-12">
          <Formik
            enableReinitialize
            onSubmit={(v, form) => {
              salvarDiarioDeBordo(v, form);
            }}
            validationSchema={validacoes}
            initialValues={valoresIniciais}
            validateOnBlur
            validateOnChange
          >
            {form => (
              <Form>
                <div className="row">
                  <div className="col-md-12 d-flex justify-content-end pb-4">
                    <Button
                      id="btn-voltar-ata-diario-bordo"
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-2"
                      onClick={() => onClickVoltar(form)}
                    />
                    <Button
                      id="btn-cancelar-ata-diario-bordo"
                      label="Cancelar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-3"
                      onClick={() => onClickCancelar(form)}
                      disabled={!modoEdicao}
                    />
                    <Button
                      id="btn-gerar-ata-diario-bordo"
                      label="Salvar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-2"
                      onClick={() => validaAntesDoSubmit(form)}
                      disabled={!modoEdicao}
                    />
                  </div>
                  <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
                    <SelectComponent
                      id="disciplina"
                      name="disciplinaId"
                      lista={listaComponenteCurriculare || []}
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      valueSelect={componenteCurricularSelecionado}
                      onChange={onChangeComponenteCurricular}
                      placeholder="Selecione um componente curricular"
                      disabled={
                        listaComponenteCurriculare &&
                        listaComponenteCurriculare.length == 1
                      }
                    />
                  </div>
                  <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
                    <CampoData
                      valor={dataSelecionada}
                      onChange={data => onChangeData(data, form)}
                      placeholder="DD/MM/AAAA"
                      formatoData="DD/MM/YYYY"
                      desabilitado={
                        !listaComponenteCurriculare ||
                        !componenteCurricularSelecionado ||
                        !diasParaHabilitar
                      }
                      diasParaHabilitar={diasParaHabilitar}
                    />
                  </div>
                </div>
                <div className="row">
                  {componenteCurricularSelecionado && dataSelecionada ? (
                    <>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                        <PainelCollapse activeKey="1">
                          <PainelCollapse.Painel
                            temBorda
                            header="Planejamento"
                            key="1"
                          >
                            <Editor
                              form={form}
                              name="planejamento"
                              id="editor-planejamento"
                              onChange={onChangeCampos}
                            />
                          </PainelCollapse.Painel>
                        </PainelCollapse>
                      </div>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                        <PainelCollapse activeKey="2">
                          <PainelCollapse.Painel
                            temBorda
                            header="Reflexões e Replanejamentos"
                            key="2"
                          >
                            <Editor
                              form={form}
                              name="reflexoesReplanejamentos"
                              id="editor-reflexoes-replanejamentos"
                              onChange={onChangeCampos}
                            />
                          </PainelCollapse.Painel>
                        </PainelCollapse>
                      </div>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                        <PainelCollapse>
                          <PainelCollapse.Painel temBorda header="Devolutivas">
                            <Editor
                              form={form}
                              name="devolutivas"
                              id="editor-devolutivas"
                              desabilitar
                            />
                          </PainelCollapse.Painel>
                        </PainelCollapse>
                      </div>
                    </>
                  ) : (
                    ''
                  )}
                  {auditoria ? (
                    <Auditoria
                      criadoEm={auditoria.criadoEm}
                      criadoPor={auditoria.criadoPor}
                      criadoRf={auditoria.criadoRf}
                      alteradoPor={auditoria.alteradoPor}
                      alteradoEm={auditoria.alteradoEm}
                      alteradoRf={auditoria.alteradoRf}
                    />
                  ) : (
                    ''
                  )}
                </div>
              </Form>
            )}
          </Formik>
        </div>
      </Card>
    </Loader>
  );
};

export default DiarioBordo;
