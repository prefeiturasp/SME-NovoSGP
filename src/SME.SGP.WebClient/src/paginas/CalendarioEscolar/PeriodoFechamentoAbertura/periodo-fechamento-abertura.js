import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
import { Form, Formik, FieldArray } from 'formik';
import * as Yup from 'yup';
import moment from 'moment';
import shortid from 'shortid';

import {
  Auditoria,
  Button,
  CampoData,
  Card,
  Colors,
  Loader,
  momentSchema,
  SelectAutocomplete,
} from '~/componentes';
import {
  Cabecalho,
  DreDropDown,
  RegistroMigrado,
  UeDropDown,
} from '~/componentes-sgp';

import { URL_HOME } from '~/constantes';

import { periodo, RotasDto } from '~/dtos';

import {
  confirmar,
  erros,
  history,
  ServicoCalendarios,
  ServicoPeriodoFechamento,
  sucesso,
  verificaSomenteConsulta,
} from '~/servicos';

import {
  BoxTextoBimestre,
  CaixaBimestre,
} from './periodo-fechamento-abertura.css';

const PeriodoFechamentoAbertura = () => {
  const usuarioLogado = useSelector(store => store.usuario);
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela =
    usuarioLogado.permissoes[RotasDto.PERIODO_FECHAMENTO_ABERTURA];
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    ''
  );
  const [dreSelecionada, setDreSelecionada] = useState('');
  const [ueSelecionada, setUeSelecionada] = useState('');

  const [emProcessamento, setEmprocessamento] = useState(false);
  const [registroMigrado, setRegistroMigrado] = useState(false);
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [idFechamentoAbertura, setIdFechamentoAbertura] = useState(0);
  const [ehRegistroExistente, setEhRegistroExistente] = useState(false);
  const [modalidadeTurma, setModalidadeTurma] = useState('');

  const [listaDres, setListaDres] = useState([]);

  const obtemPeriodosIniciais = () => {
    return {
      dreId: null,
      ueId: null,
      tipoCalendarioId: null,
      periodoEscolarId: null,
      migrado: false,
      id: 0,
      fechamentosBimestres: [],
      bimestre1InicioDoFechamento: '',
      bimestre1FinalDoFechamento: '',
      bimestre2InicioDoFechamento: '',
      bimestre2FinalDoFechamento: '',
      bimestre3InicioDoFechamento: '',
      bimestre3FinalDoFechamento: '',
      bimestre4InicioDoFechamento: '',
      bimestre4FinalDoFechamento: '',
    };
  };
  const [fechamento, setFechamento] = useState(obtemPeriodosIniciais());
  const [auditoria, setAuditoria] = useState({});
  const [isTipoCalendarioAnual, setIsTipoCalendarioAnual] = useState(true);
  const [validacoes, setValidacoes] = useState();
  const [listaTipoCalendario, setListaTipoCalendario] = useState([]);
  const [valorTipoCalendario, setValorTipoCalendario] = useState('');
  const [pesquisaTipoCalendario, setPesquisaTipoCalendario] = useState('');

  const validacaoPrimeiroBim = {
    bimestre1InicioDoFechamento: momentSchema.required(
      'Data inicial obrigatória'
    ),
    bimestre1FinalDoFechamento: momentSchema
      .required('Data final obrigatória')
      .dataMenorQue(
        'bimestre1InicioDoFechamento',
        'bimestre1FinalDoFechamento',
        'Data inválida'
      ),
  };

  const validacaoSegundoBim = {
    bimestre2InicioDoFechamento: momentSchema
      .required('Data inicial obrigatória')
      .dataMenorIgualQue(
        'bimestre1FinalDoFechamento',
        'bimestre2InicioDoFechamento',
        'Data inválida'
      ),
    bimestre2FinalDoFechamento: momentSchema
      .required('Data final obrigatória')
      .dataMenorQue(
        'bimestre2InicioDoFechamento',
        'bimestre2FinalDoFechamento',
        'Data inválida'
      ),
  };

  const validacaoTerceiroBim = {
    bimestre3InicioDoFechamento: momentSchema
      .required('Data inicial obrigatória')
      .dataMenorIgualQue(
        'bimestre2FinalDoFechamento',
        'bimestre3InicioDoFechamento',
        'Data inválida'
      ),
    bimestre3FinalDoFechamento: momentSchema
      .required('Data final obrigatória')
      .dataMenorQue(
        'bimestre3InicioDoFechamento',
        'bimestre3FinalDoFechamento',
        'Data inválida'
      ),
  };

  const validacaoQuartoBim = {
    bimestre4InicioDoFechamento: momentSchema
      .required('Data inicial obrigatória')
      .dataMenorIgualQue(
        'bimestre3FinalDoFechamento',
        'bimestre4InicioDoFechamento',
        'Data inválida'
      ),
    bimestre4FinalDoFechamento: momentSchema
      .required('Data final obrigatória')
      .dataMenorQue(
        'bimestre4InicioDoFechamento',
        'bimestre4FinalDoFechamento',
        'Data inválida'
      ),
  };

  useEffect(() => {
    let periodos = {};
    if (isTipoCalendarioAnual) {
      periodos = Object.assign(
        {},
        validacaoPrimeiroBim,
        validacaoSegundoBim,
        validacaoTerceiroBim,
        validacaoQuartoBim
      );
    } else {
      periodos = Object.assign({}, validacaoPrimeiroBim, validacaoSegundoBim);
    }
    setValidacoes(Yup.object().shape(periodos));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isTipoCalendarioAnual]);

  useEffect(() => {
    const somenteConsultarFrequencia = verificaSomenteConsulta(permissoesTela);
    setSomenteConsulta(somenteConsultarFrequencia);
  }, [permissoesTela]);

  useEffect(() => {
    const desabilitar =
      idFechamentoAbertura > 0
        ? somenteConsulta || !permissoesTela.podeAlterar
        : somenteConsulta || !permissoesTela.podeIncluir;
    setDesabilitarCampos(desabilitar);
  }, [
    idFechamentoAbertura,
    permissoesTela.podeAlterar,
    permissoesTela.podeIncluir,
    somenteConsulta,
  ]);

  const obterDataMoment = data => {
    return data ? moment(data) : null;
  };

  useEffect(() => {
    let isSubscribed = true;
    (async () => {
      setCarregandoTipos(true);

      const {
        data,
      } = await ServicoCalendarios.obterTiposCalendarioAutoComplete(
        pesquisaTipoCalendario
      );

      if (isSubscribed) {
        setListaTipoCalendario(data);
        setCarregandoTipos(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [pesquisaTipoCalendario]);

  const carregaDados = useCallback(() => {
    setModoEdicao(false);
    if (tipoCalendarioSelecionado) {
      setEmprocessamento(true);
      const ue = ueSelecionada === undefined ? '' : ueSelecionada;
      ServicoPeriodoFechamento.obterPorTipoCalendarioDreEUe(
        tipoCalendarioSelecionado,
        dreSelecionada,
        ue
      )
        .then(resposta => {
          if (resposta.data && resposta.data.fechamentosBimestres) {
            const montarDataInicio = item => {
              return item.inicioDoFechamento
                ? obterDataMoment(item.inicioDoFechamento)
                : '';
            };

            const montarDataFim = item => {
              return item.finalDoFechamento
                ? obterDataMoment(item.finalDoFechamento)
                : '';
            };

            resposta.data.fechamentosBimestres.forEach(item => {
              switch (item.bimestre) {
                case 1:
                  resposta.data.bimestre1InicioDoFechamento = montarDataInicio(
                    item
                  );
                  resposta.data.bimestre1FinalDoFechamento = montarDataFim(
                    item
                  );
                  break;
                case 2:
                  resposta.data.bimestre2InicioDoFechamento = montarDataInicio(
                    item
                  );
                  resposta.data.bimestre2FinalDoFechamento = montarDataFim(
                    item
                  );
                  break;
                case 3:
                  resposta.data.bimestre3InicioDoFechamento = montarDataInicio(
                    item
                  );
                  resposta.data.bimestre3FinalDoFechamento = montarDataFim(
                    item
                  );
                  break;
                case 4:
                  resposta.data.bimestre4InicioDoFechamento = montarDataInicio(
                    item
                  );
                  resposta.data.bimestre4FinalDoFechamento = montarDataFim(
                    item
                  );
                  break;
                default:
                  break;
              }
              item.inicioMinimo = obterDataMoment(item.inicioMinimo);
              item.finalMaximo = obterDataMoment(item.finalMaximo);
            });
            setEhRegistroExistente(resposta.data.id);
            setFechamento(resposta.data);
            setRegistroMigrado(resposta.data.migrado);
            setAuditoria({
              criadoEm: resposta.data.criadoEm,
              criadoPor: resposta.data.criadoPor,
              criadoRf: resposta.data.criadoRf,
              alteradoPor: resposta.data.alteradoPor,
              alteradoEm: resposta.data.alteradoEm,
              alteradoRf: resposta.data.alteradoRf,
            });
            setIdFechamentoAbertura(resposta.data.id);
          } else {
            setFechamento(obtemPeriodosIniciais());
          }
        })
        .catch(e => {
          setFechamento(obtemPeriodosIniciais());
          erros(e);
        })
        .finally(() => setEmprocessamento(false));
    } else {
      setFechamento(obtemPeriodosIniciais());
    }
  }, [dreSelecionada, tipoCalendarioSelecionado, ueSelecionada]);

  useEffect(() => {
    carregaDados();
  }, [dreSelecionada, carregaDados, tipoCalendarioSelecionado, ueSelecionada]);

  const onChangeCamposData = form => {
    if (!modoEdicao) {
      touchedFields(form);
      setModoEdicao(true);
    }
  };

  const onClickVoltar = async form => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?',
        'Sim',
        'Não'
      );

      if (confirmado) {
        validaAntesDoSubmit(form);
        history.push(URL_HOME);
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const touchedFields = form => {
    const arrayCampos = Object.keys(fechamento);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
  };

  const validaAntesDoSubmit = form => {
    touchedFields(form);
    form.validateForm().then(() => {
      if (
        form.isValid ||
        (Object.keys(form.errors).length == 0 &&
          Object.keys(form.values).length > 0)
      ) {
        form.handleSubmit(e => e);
      }
    });
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmado) {
        resetarTela(form);
      }
    }
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
    setFechamento(obtemPeriodosIniciais());
    carregaDados();
  };

  const onSubmit = async (form, confirmou = false) => {
    form.fechamentosBimestres.forEach(item => {
      switch (item.bimestre) {
        case 1:
          item.inicioDoFechamento = form.bimestre1InicioDoFechamento.toDate();
          item.finalDoFechamento = form.bimestre1FinalDoFechamento.toDate();
          break;
        case 2:
          item.inicioDoFechamento = form.bimestre2InicioDoFechamento.toDate();
          item.finalDoFechamento = form.bimestre2FinalDoFechamento.toDate();
          break;
        case 3:
          item.inicioDoFechamento = form.bimestre3InicioDoFechamento.toDate();
          item.finalDoFechamento = form.bimestre3FinalDoFechamento.toDate();
          break;
        case 4:
          item.inicioDoFechamento = form.bimestre4InicioDoFechamento.toDate();
          item.finalDoFechamento = form.bimestre4FinalDoFechamento.toDate();
          break;
        default:
          break;
      }
    });

    setEmprocessamento(true);
    ServicoPeriodoFechamento.salvar({
      ...form,
      confirmouAlteracaoHierarquica: confirmou,
    })
      .then(() => {
        sucesso('Períodos salvos com sucesso.');
        carregaDados();
        setModoEdicao(false);
      })
      .catch(async e => {
        if (e && e.response && e.response.status === 602) {
          if (e && e.response && e.response.data && e.response.data.mensagens) {
            const confirmacao = await confirmar(
              'Atenção',
              e.response.data.mensagens[0]
            );
            if (confirmacao) {
              onSubmit(form, true);
            }
          }
        } else erros(e);
      })
      .finally(() => setEmprocessamento(false));
  };

  const obterDatasParaHabilitar = (inicio, fim) => {
    const dias = [];
    let diaInicial = inicio;

    while (diaInicial <= fim) {
      dias.push(diaInicial.format('YYYY-MM-DD'));
      diaInicial = diaInicial.clone().add(1, 'd');
    }
    return dias;
  };

  const setarModalidadeTurma = tipoSelecionado => {
    if (tipoSelecionado && tipoSelecionado.modalidade) {
      const modalidadeT = ServicoCalendarios.converterModalidade(
        tipoSelecionado.modalidade
      );
      setModalidadeTurma(modalidadeT);
    } else {
      setModalidadeTurma('');
    }
  };

  const onChangeDre = (dreId, form, dres) => {
    setListaDres(dres);
    if (dreId !== dreSelecionada) {
      setDreSelecionada(dreId);
      const ue = undefined;
      setUeSelecionada(ue);
      const tipoSelecionado = listaTipoCalendario.find(
        item => item.id == form.values.tipoCalendarioId
      );
      setarModalidadeTurma(tipoSelecionado);
    }
  };

  const criaBimestre = (
    form,
    descricao,
    chaveDataInicial,
    chaveDataFinal,
    diasParaHabilitar,
    indice
  ) => {
    return (
      <div className="row" key={`key-${indice}`}>
        <div className="col-md-6 mb-2">
          <CaixaBimestre>
            <BoxTextoBimestre>{descricao}</BoxTextoBimestre>
          </CaixaBimestre>
        </div>
        <div className="col-md-3 mb-2">
          <CampoData
            form={form}
            placeholder="Início do Bimestre"
            formatoData="DD/MM/YYYY"
            name={chaveDataInicial}
            onChange={() => onChangeCamposData(form)}
            diasParaHabilitar={diasParaHabilitar}
            desabilitado={desabilitarCampos}
          />
        </div>
        <div className="col-md-3 mb-2">
          <CampoData
            form={form}
            placeholder="Fim do Bimestre"
            formatoData="DD/MM/YYYY"
            name={chaveDataFinal}
            onChange={() => onChangeCamposData(form)}
            diasParaHabilitar={diasParaHabilitar}
            desabilitado={desabilitarCampos}
          />
        </div>
      </div>
    );
  };

  const selecionaTipoCalendario = (descricao, form) => {
    const tipo = listaTipoCalendario?.find(t => t.descricao === descricao);
    if (Number(tipo?.id) || !tipo?.id) {
      const isPeriodoAnual = tipo?.periodo === periodo?.Anual;
      setIsTipoCalendarioAnual(isPeriodoAnual);
      setValorTipoCalendario(descricao);
    }
    setTipoCalendarioSelecionado(tipo?.id);
    setUeSelecionada('');
    form.setFieldValue('ueId', '');
    setarModalidadeTurma(tipo);
    if (listaDres && listaDres.length > 1) {
      setDreSelecionada('');
      form.setFieldValue('dreId', '');
    }
  };

  const handleSearch = descricao => {
    if (descricao.length > 3 || descricao.length === 0) {
      setPesquisaTipoCalendario(descricao);
    }
  };

  return (
    <>
      <Loader loading={emProcessamento}>
        <Cabecalho pagina="Período de Fechamento (Abertura)">
          {registroMigrado && (
            <div className="col-md-2 float-right">
              <RegistroMigrado>Registro Migrado</RegistroMigrado>
            </div>
          )}
        </Cabecalho>
        <Card>
          <Formik
            enableReinitialize
            initialValues={fechamento}
            validationSchema={validacoes}
            onSubmit={values => onSubmit(values)}
            validateOnChange
            validateOnBlur
            id={shortid.generate()}
          >
            {form => (
              <Form className="col-md-12">
                <div className="row mb-4">
                  <div className="col-md-12 d-flex justify-content-end pb-4">
                    <Button
                      id={shortid.generate()}
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-3"
                      onClick={() => onClickVoltar(form)}
                    />
                    <Button
                      id={shortid.generate()}
                      label="Cancelar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-3"
                      disabled={desabilitarCampos || !modoEdicao}
                      onClick={() => onClickCancelar(form)}
                    />
                    <Button
                      id={shortid.generate()}
                      label={ehRegistroExistente ? 'Alterar' : 'Cadastrar'}
                      color={Colors.Roxo}
                      border
                      bold
                      disabled={desabilitarCampos}
                      onClick={() => validaAntesDoSubmit(form)}
                    />
                  </div>
                  <div className="col-md-8 pb-2">
                    <Loader loading={carregandoTipos} tip="">
                      <div style={{ maxWidth: '300px' }}>
                        <SelectAutocomplete
                          hideLabel
                          showList
                          isHandleSearch
                          placeholder="Selecione um tipo de calendário"
                          className="col-md-12"
                          name="tipoCalendarioId"
                          id="tipoCalendarioId"
                          lista={listaTipoCalendario}
                          valueField="id"
                          textField="descricao"
                          onSelect={valor =>
                            selecionaTipoCalendario(valor, form)
                          }
                          onChange={valor =>
                            selecionaTipoCalendario(valor, form)
                          }
                          handleSearch={handleSearch}
                          value={valorTipoCalendario}
                        />
                      </div>
                    </Loader>
                  </div>
                  <br />
                  <div className="col-md-6 pb-2">
                    {tipoCalendarioSelecionado &&
                      fechamento &&
                      fechamento.fechamentosBimestres &&
                      fechamento.fechamentosBimestres.length > 0 && (
                        <DreDropDown
                          label="Diretoria Regional de Educação (DRE)"
                          form={form}
                          onChange={(dreId, dres) =>
                            onChangeDre(dreId, form, dres)
                          }
                          desabilitado={desabilitarCampos}
                        />
                      )}
                  </div>
                  <div className="col-md-6 pb-2">
                    {tipoCalendarioSelecionado &&
                      fechamento &&
                      fechamento.fechamentosBimestres &&
                      fechamento.fechamentosBimestres.length > 0 && (
                        <UeDropDown
                          dreId={form.values.dreId}
                          label="Unidade Escolar (UE)"
                          form={form}
                          url=""
                          onChange={ueId => setUeSelecionada(ueId)}
                          desabilitado={desabilitarCampos}
                          modalidade={modalidadeTurma}
                        />
                      )}
                  </div>
                </div>
                <FieldArray
                  name="fechamentosBimestres"
                  render={() => (
                    <>
                      {fechamento.fechamentosBimestres.map((c, indice) =>
                        criaBimestre(
                          form,
                          `${c.bimestre}° Bimestre`,
                          `bimestre${c.bimestre}InicioDoFechamento`,
                          `bimestre${c.bimestre}FinalDoFechamento`,
                          obterDatasParaHabilitar(
                            c.inicioMinimo,
                            c.finalMaximo
                          ),
                          indice
                        )
                      )}
                    </>
                  )}
                />
              </Form>
            )}
          </Formik>
          <div className="col-md-6 d-flex justify-content-start">
            {tipoCalendarioSelecionado &&
            tipoCalendarioSelecionado !== '' &&
            ehRegistroExistente &&
            auditoria &&
            auditoria.criadoEm ? (
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
        </Card>
      </Loader>
    </>
  );
};

export default PeriodoFechamentoAbertura;
