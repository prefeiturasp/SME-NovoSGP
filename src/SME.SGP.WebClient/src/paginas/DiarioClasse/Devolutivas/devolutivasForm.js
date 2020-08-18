import { Form, Formik } from 'formik';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import * as Yup from 'yup';
import { CampoData, Loader, momentSchema, Auditoria } from '~/componentes';
import AlertaPermiteSomenteTurmaInfantil from '~/componentes-sgp/AlertaPermiteSomenteTurmaInfantil/alertaPermiteSomenteTurmaInfantil';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Editor from '~/componentes/editor/editor';
import SelectComponent from '~/componentes/select';
import { erros, sucesso, confirmar } from '~/servicos/alertas';
import ServicoDevolutivas from '~/servicos/Paginas/DiarioClasse/ServicoDevolutivas';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import DadosPlanejamentoDiarioBordo from './DadosPlanejamentoDiarioBordo/dadosPlanejamentoDiarioBordo';
import {
  setDadosPlanejamentos,
  limparDadosPlanejamento,
} from '~/redux/modulos/devolutivas/actions';
import ServicoDiarioBordo from '~/servicos/Paginas/DiarioClasse/ServicoDiarioBordo';

const DevolutivasForm = ({ match }) => {
  const dispatch = useDispatch();

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const turmaCodigo = turmaSelecionada ? turmaSelecionada.turma : 0;

  const [
    listaComponenteCurriculare,
    setListaComponenteCurriculare,
  ] = useState();

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [turmaInfantil, setTurmaInfantil] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [datasParaHabilitar, setDatasParaHabilitar] = useState();
  const [idDevolutiva, setIdDevolutiva] = useState(0);
  const [refForm, setRefForm] = useState({});

  const inicial = {
    codigoComponenteCurricular: 0,
    dataInicio: '',
    dataFim: '',
    devolutiva: '',
    auditoria: null,
  };
  const [valoresIniciais, setValoresIniciais] = useState(inicial);

  const validacoesRegistroNovo = Yup.object({
    dataInicio: momentSchema.required('Data início obrigatória'),
    dataFim: momentSchema.required('Data fim obrigatória'),
    devolutiva: Yup.string()
      .required('Campo devolutiva obrigatório')
      .min(200, 'Você precisa preencher com no mínimo 200 caracteres'),
  });

  const validacoesRegistroEdicao = Yup.object({
    devolutiva: Yup.string()
      .required('Campo devolutiva obrigatório')
      .min(200, 'Você precisa preencher com no mínimo 200 caracteres'),
  });

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setBreadcrumbManual(
        match.url,
        'Alterar Devolutiva',
        RotasDto.DEVOLUTIVAS
      );
      setIdDevolutiva(match.params.id);
    } else {
      setIdDevolutiva(0);
    }
  }, [match]);

  const resetarTela = useCallback(() => {
    dispatch(limparDadosPlanejamento());
    if (refForm && refForm.resetForm) {
      refForm.resetForm();
    }
  }, [dispatch, refForm]);

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setTurmaInfantil(infantil);

    if (!infantil) {
      resetarTela();
      history.push(RotasDto.DEVOLUTIVAS);
    }
  }, [turmaSelecionada, modalidadesFiltroPrincipal, resetarTela]);

  useEffect(() => {
    if (!turmaSelecionada.turma) {
      history.push(RotasDto.DEVOLUTIVAS);
    }
    resetarTela();
  }, [turmaSelecionada.turma, resetarTela]);

  const obterSugestaoDataInicio = useCallback(
    async codigoComponenteCurricular => {
      const retorno = await ServicoDevolutivas.obterSugestaoDataInicio(
        turmaCodigo,
        codigoComponenteCurricular
      ).catch(e => erros(e));
      if (retorno && retorno.data) {
        return moment(retorno.data);
      }
      return '';
    },
    [turmaCodigo]
  );

  const obterDatasFimParaHabilitar = dataInicio => {
    const dataInicial = moment({ ...dataInicio });
    const datas = [dataInicial.format('YYYY-MM-DD')];
    const qtdMaxDias = 30;
    for (let index = 0; index < qtdMaxDias; index += 1) {
      const novaData = dataInicial.add(1, 'days');
      datas.push(novaData.format('YYYY-MM-DD'));
    }
    return datas;
  };

  const setarValoresIniciaisRegistroNovo = useCallback(
    async codigoComponenteCurricular => {
      const valores = {
        codigoComponenteCurricular: 0,
        dataInicio: '',
        dataFim: '',
        devolutiva: '',
        auditoria: null,
      };
      valores.codigoComponenteCurricular = codigoComponenteCurricular;
      valores.dataInicio = await obterSugestaoDataInicio(
        codigoComponenteCurricular
      );
      if (valores.dataInicio) {
        const paraHabilitar = await obterDatasFimParaHabilitar(
          moment(valores.dataInicio)
        );
        setDatasParaHabilitar(paraHabilitar);
      }
      setValoresIniciais(valores);
    },
    [obterSugestaoDataInicio]
  );

  const setarValoresIniciaisRegistroEdicao = (
    dados,
    codigoComponenteCurricular
  ) => {
    const valores = {
      codigoComponenteCurricular,
      dataInicio: '',
      dataFim: '',
      devolutiva: dados.devolutiva,
      auditoria: dados.auditoria,
      diariosIds: dados.diariosIds,
    };
    setValoresIniciais({ ...valores });
  };

  const obterDevolutiva = useCallback(
    async (id, codigoComponenteCurricular) => {
      const retorno = await ServicoDevolutivas.obterDevolutiva(id).catch(e =>
        erros(e)
      );
      if (retorno && retorno.data) {
        setarValoresIniciaisRegistroEdicao(
          retorno.data,
          codigoComponenteCurricular
        );
      }
    },
    []
  );

  const obterPlanejamentosPorDevolutiva = useCallback(
    async pagina => {
      setCarregandoGeral(true);
      const retorno = await ServicoDiarioBordo.obterPlanejamentosPorDevolutiva(
        idDevolutiva,
        pagina || 1
      ).catch(e => erros(e));
      setCarregandoGeral(false);
      if (retorno && retorno.data) {
        dispatch(setDadosPlanejamentos(retorno.data));
      } else {
        dispatch(limparDadosPlanejamento());
      }
    },
    [idDevolutiva, dispatch]
  );

  useEffect(() => {
    if (idDevolutiva) {
      obterPlanejamentosPorDevolutiva();
    }
  }, [idDevolutiva, obterPlanejamentosPorDevolutiva]);

  useEffect(() => {
    if (listaComponenteCurriculare && listaComponenteCurriculare.length) {
      if (listaComponenteCurriculare.length === 1) {
        const componente = listaComponenteCurriculare[0];
        const codigoComponenteCurricular = String(
          componente.codigoComponenteCurricular
        );
        if (match && match.params && match.params.id) {
          obterDevolutiva(match.params.id, codigoComponenteCurricular);
        } else {
          setarValoresIniciaisRegistroNovo(codigoComponenteCurricular);
        }
      }
    }
  }, [
    listaComponenteCurriculare,
    setarValoresIniciaisRegistroNovo,
    match,
    obterDevolutiva,
  ]);

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    dispatch(limparDadosPlanejamento());
    const componentes = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaCodigo
    ).catch(e => erros(e));

    if (componentes.data && componentes.data.length) {
      setListaComponenteCurriculare(componentes.data);
    }

    setCarregandoGeral(false);
  }, [turmaCodigo, dispatch]);

  useEffect(() => {
    if (turmaCodigo && turmaInfantil) {
      obterComponentesCurriculares();
    } else {
      setListaComponenteCurriculare([]);
    }
  }, [turmaCodigo, obterComponentesCurriculares, turmaInfantil]);

  const onChangeDataInicio = async (dataInicio, form) => {
    if (dataInicio) {
      const paraHabilitar = await obterDatasFimParaHabilitar(dataInicio);
      setDatasParaHabilitar(paraHabilitar);
    }
    form.setFieldValue('dataFim', '');
    dispatch(limparDadosPlanejamento());
    setModoEdicao(true);
  };

  const obterDadosPlanejamento = async (dataFim, form, pagina) => {
    const { dataInicio, codigoComponenteCurricular } = form.values;
    setCarregandoGeral(true);
    const retorno = await ServicoDiarioBordo.obterPlanejamentosPorIntervalo(
      turmaCodigo,
      codigoComponenteCurricular,
      dataInicio.format('YYYY-MM-DD'),
      dataFim.format('YYYY-MM-DD'),
      pagina || 1
    ).catch(e => erros(e));
    setCarregandoGeral(false);
    if (retorno && retorno.data) {
      dispatch(setDadosPlanejamentos(retorno.data));
    } else {
      dispatch(setDadosPlanejamentos({}));
    }
  };

  const onChangeDataFim = (data, form) => {
    if (!data) {
      form.setFieldValue('devolutiva', '');
    }
    if (data) {
      obterDadosPlanejamento(data, form);
    }
    dispatch(limparDadosPlanejamento());
    setModoEdicao(true);
  };

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        form.resetForm();
        setModoEdicao(false);
      }
    }
  };
  const salvarDevolutivas = async (valores, clicouBtnSalvar) => {
    setCarregandoGeral(true);
    const params = {
      devolutiva: valores.devolutiva,
    };

    if (!idDevolutiva) {
      params.diariosIds = valores.diariosIds;
    }
    const retorno = await ServicoDevolutivas.salvarAlterarDevolutiva(
      params,
      idDevolutiva
    ).catch(e => erros(e));

    setCarregandoGeral(false);
    let salvouComSucesso = false;
    if (retorno && retorno.status === 200) {
      sucesso(
        `Devolutiva ${idDevolutiva ? 'alterada' : 'inserida'} com sucesso`
      );
      if (clicouBtnSalvar) {
        setModoEdicao(false);
      }
      salvouComSucesso = true;
    }
    return salvouComSucesso;
  };

  const validaAntesDoSubmit = (form, clicouBtnSalvar) => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    return form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        return salvarDevolutivas(form.values, form, clicouBtnSalvar);
      }
      return false;
    });
  };

  const onClickVoltar = async form => {
    if (modoEdicao && turmaInfantil) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        const salvou = await validaAntesDoSubmit(form);
        if (salvou) {
          history.push(RotasDto.DEVOLUTIVAS);
        }
      } else {
        history.push(RotasDto.DEVOLUTIVAS);
      }
    } else {
      history.push(RotasDto.DEVOLUTIVAS);
    }
  };

  const onClickExcluir = async () => {
    if (idDevolutiva) {
      const confirmado = await confirmar(
        'Excluir',
        '',
        'Você tem certeza que deseja excluir este registro?'
      );
      if (confirmado) {
        const deletou = await ServicoDevolutivas.deletarDevolutiva(
          idDevolutiva
        ).catch(e => erros(e));

        if (deletou && deletou.status === 200) {
          sucesso('Registro excluído com sucesso.');
          history.push(RotasDto.DEVOLUTIVAS);
        }
      }
    }
  };

  const onChangePage = (pagina, form) => {
    if (idDevolutiva) {
      obterPlanejamentosPorDevolutiva(pagina);
    } else {
      obterDadosPlanejamento(form.values.dataFim, form, pagina);
    }
  };

  return (
    <Loader loading={carregandoGeral} className="w-100 my-2">
      {!turmaSelecionada.turma ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'devolutivas-selecione-turma',
            mensagem: 'Você precisa escolher uma turma',
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
      {turmaSelecionada.turma ? <AlertaPermiteSomenteTurmaInfantil /> : ''}
      <Cabecalho pagina="Devolutivas" />
      <Card>
        <div className="col-md-12">
          <Formik
            enableReinitialize
            validationSchema={
              idDevolutiva ? validacoesRegistroEdicao : validacoesRegistroNovo
            }
            initialValues={valoresIniciais}
            validateOnBlur
            validateOnChange
            ref={refFormik => setRefForm(refFormik)}
          >
            {form => (
              <Form>
                <div className="row">
                  <div className="col-md-12 d-flex justify-content-end pb-4">
                    <Button
                      id="btn-voltar-devolutivas"
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-3"
                      onClick={() => onClickVoltar(form)}
                    />
                    <Button
                      id="btn-cancelar-devolutivas"
                      label="Cancelar"
                      color={Colors.Roxo}
                      onClick={() => onClickCancelar(form)}
                      border
                      bold
                      className="mr-3"
                      disabled={!turmaInfantil || !modoEdicao}
                    />
                    <Button
                      label="Excluir"
                      color={Colors.Vermelho}
                      border
                      className="mr-3"
                      disabled={!idDevolutiva}
                      onClick={onClickExcluir}
                    />
                    <Button
                      id="btn-salvar-devolutivas"
                      label={idDevolutiva ? 'Alterar' : 'Salvar'}
                      color={Colors.Roxo}
                      border
                      bold
                      onClick={async () => {
                        const salvou = await validaAntesDoSubmit(form, true);
                        if (salvou) {
                          history.push(RotasDto.DEVOLUTIVAS);
                        }
                      }}
                      disabled={!turmaInfantil || !modoEdicao}
                    />
                  </div>
                  <div className="col-sm-12 col-md-12 col-lg-6 col-xl-4 mb-2">
                    <SelectComponent
                      label="Componente curricular"
                      id="disciplina"
                      lista={listaComponenteCurriculare || []}
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      placeholder="Selecione um componente curricular"
                      disabled={
                        !turmaInfantil ||
                        !turmaSelecionada.turma ||
                        (listaComponenteCurriculare &&
                          listaComponenteCurriculare.length === 1)
                      }
                      form={form}
                      name="codigoComponenteCurricular"
                    />
                  </div>
                  <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
                    <CampoData
                      label="Data início"
                      form={form}
                      name="dataInicio"
                      onChange={data => {
                        onChangeDataInicio(data, form);
                      }}
                      placeholder="DD/MM/AAAA"
                      formatoData="DD/MM/YYYY"
                      desabilitado={
                        idDevolutiva ||
                        !turmaInfantil ||
                        !listaComponenteCurriculare ||
                        !form.values.codigoComponenteCurricular
                      }
                    />
                  </div>
                  <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-5">
                    <CampoData
                      label="Data fim"
                      form={form}
                      name="dataFim"
                      onChange={data => onChangeDataFim(data, form)}
                      placeholder="DD/MM/AAAA"
                      formatoData="DD/MM/YYYY"
                      desabilitado={
                        idDevolutiva ||
                        !turmaInfantil ||
                        !listaComponenteCurriculare ||
                        !form.values.dataInicio
                      }
                      diasParaHabilitar={datasParaHabilitar}
                    />
                  </div>

                  {(form.values.dataInicio && form.values.dataFim) ||
                  idDevolutiva ? (
                    <>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <DadosPlanejamentoDiarioBordo
                          onChangePage={pagina => onChangePage(pagina, form)}
                        />
                      </div>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mt-3">
                        <Editor
                          label="Devolutiva"
                          form={form}
                          name="devolutiva"
                          id="editor-devolutiva"
                          onChange={v => {
                            if (valoresIniciais.devolutiva !== v) {
                              setModoEdicao(true);
                            }
                          }}
                        />
                      </div>
                      {form.values.auditoria ? (
                        <Auditoria
                          criadoEm={form.values.auditoria.criadoEm}
                          criadoPor={form.values.auditoria.criadoPor}
                          criadoRf={form.values.auditoria.criadoRF}
                          alteradoPor={form.values.auditoria.alteradoPor}
                          alteradoEm={form.values.auditoria.alteradoEm}
                          alteradoRf={form.values.auditoria.alteradoRF}
                        />
                      ) : (
                        ''
                      )}
                    </>
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

DevolutivasForm.propTypes = {
  match: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
};

DevolutivasForm.defaultProps = {
  match: {},
};

export default DevolutivasForm;
