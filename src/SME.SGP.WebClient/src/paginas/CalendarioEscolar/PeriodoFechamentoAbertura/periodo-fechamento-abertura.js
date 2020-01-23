import React, { useState, useEffect } from 'react';
import { Form, Formik, FieldArray } from 'formik';
import * as Yup from 'yup';
import { DreDropDown, UeDropDown } from 'componentes-sgp';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import {
  BoxTextoBimetre,
  CaixaBimestre,
} from './periodo-fechamento-abertura.css';
import api from '~/servicos/api';
import periodo from '~/dtos/periodo';
import { CampoData, Loader, momentSchema, Auditoria } from '~/componentes';
import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import { erros } from '~/servicos/alertas';
import moment from 'moment';
import { CampoDataFormik } from '~/componentes/campoDataFormik/campoDataFormik';

const PeriodoFechamentoAbertura = () => {
  const [listaTipoCalendarioEscolar, setListaTipoCalendarioEscolar] = useState(
    []
  );
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    ''
  );
  const [dreSelecionada, setDreSelecionada] = useState('');
  const [ueSelecionada, setUeSelecionada] = useState('');
  const [ehTipoCalendarioAnual, setEhTipoCalendarioAnual] = useState(true);
  const [desabilitaCampos, setDesabilitaCampos] = useState(false);

  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [desabilitarTipoCalendario, setDesabilitarTipoCalendario] = useState(
    false
  );
  const [anoLetivo, setAnoLetivo] = useState(new Date().getFullYear());
  const [modoEdicao, setModoEdicao] = useState(false);
  const obtemPeriodosIniciais = () => {
    return {
      dreId: null,
      ueId: null,
      tipoCalendarioId: null,
      migrado: false,
      id: 0,
      fechamentosBimestres: [],
    };
  };
  const [periodos, setPeriodos] = useState(obtemPeriodosIniciais());
  const [auditoria, setAuditoria] = useState([]);

  const [validacoes, setValidacoes] = useState(
    Yup.object().shape({
      fechamentosBimestres: Yup.array().of(
        Yup.object().shape({
          inicioDoFechamento: Yup.string().required(
            'Data de início obrigatória.'
          ),
          finalDoFechamento: Yup.string().required('Data final obrigatória.'),
        })
      ),
    })
  );

  useEffect(() => {
    async function consultaTipos() {
      setCarregandoTipos(true);
      const listaTipo = await api.get('v1/calendarios/tipos');
      if (listaTipo && listaTipo.data && listaTipo.data.length) {
        listaTipo.data.map(item => {
          item.id = String(item.id);
          item.descricaoTipoCalendario = `${item.anoLetivo} - ${item.nome} - ${item.descricaoPeriodo}`;
        });
        setListaTipoCalendarioEscolar(listaTipo.data);
        if (listaTipo.data.length === 1) {
          setTipoCalendarioSelecionado(listaTipo.data[0].id);
          setDesabilitarTipoCalendario(true);
        } else {
          setDesabilitarTipoCalendario(false);
        }
      } else {
        setListaTipoCalendarioEscolar([]);
      }
      setCarregandoTipos(false);
    }
    consultaTipos();
  }, []);

  useEffect(() => {
    if (tipoCalendarioSelecionado) {
      api
        .get(
          `/v1/periodos/fechamentos/aberturas?tipoCalendarioId=${tipoCalendarioSelecionado}&dreId=${dreSelecionada}&ueId=${ueSelecionada}`
        )
        .then(resposta => {
          if (resposta.data && resposta.data.fechamentosBimestres) {
            resposta.data.fechamentosBimestres.forEach(bimestre => {
              bimestre.inicioDoFechamento = moment(bimestre.inicioDoFechamento);
              bimestre.finalDoFechamento = moment(bimestre.finalDoFechamento);
              bimestre.inicioMinimo = moment(bimestre.inicioMinimo);
              bimestre.finalMaximo = moment(bimestre.finalMaximo);
            });
          }
          setPeriodos(resposta.data);
        })
        .catch(e => {
          setPeriodos(obtemPeriodosIniciais());
          erros(e);
        });
    }
  }, [dreSelecionada, tipoCalendarioSelecionado, ueSelecionada]);

  // useEffect(() => {
  //   if (periodos.fechamentosBimestres.length > 0)
  //     setValidacoes(
  //       Yup.object({
  //         fechamentosBimestres: [
  //           periodos.fechamentosBimestres.map(c =>
  //             Yup.object({
  //               inicioDoFechamento: momentSchema.required('Data obrigatória'),
  //               finalDoFechamento: momentSchema.required('Data obrigatória'),
  //             })
  //           ),
  //         ],
  //       })
  //     );
  // }, [periodos]);

  const onChangeCamposData = valor => {
    setModoEdicao(true);
  };

  const validacaoAnoLetivo = () => {
    return momentSchema.test({
      name: 'teste',
      exclusive: true,
      message: 'Data inváçida',
      test: value => value.year().toString() === anoLetivo.toString(),
    });
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const validaAntesDoSubmit = form => {
    form.validateForm().then(() => {
      if (
        form.isValid ||
        (Object.keys(form.errors).length == 0 &&
          Object.keys(form.values).length > 0)
      ) {
        form.handleSubmit(e => e);
      }
    });
    // const arrayCampos = Object.keys(valoresFormInicial);
    // arrayCampos.forEach(campo => {
    //   form.setFieldTouched(campo, true, true);
    // });
    // form.validateForm().then(() => {
    //   if (
    //     form.isValid ||
    //     (Object.keys(form.errors).length == 0 &&
    //       Object.keys(form.values).length > 0)
    //   ) {
    //     form.handleSubmit(e => e);
    //   }
    // });
  };

  const onClickCancelar = form => {
    form.resetForm();
    setModoEdicao(false);
  };

  const buscarPeriodosPorTipoCalendario = id => {};

  const onSubmit = form => {
    console.log(form);
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

  const obterErros = (form, campo, indice) => (
    <span className="erro">
      {form &&
        form.errors['fechamentosBimestres'] &&
        form.errors['fechamentosBimestres'][indice] &&
        form.errors['fechamentosBimestres'][indice][campo]}
    </span>
  );

  const criaBimestre = (
    form,
    descricao,
    chaveDataInicial,
    chaveDataFinal,
    diasParaHabilitar,
    indice
  ) => {
    return (
      <div className="row">
        <div className="col-md-6 mb-2">
          <CaixaBimestre>
            <BoxTextoBimetre>{descricao}</BoxTextoBimetre>
          </CaixaBimestre>
        </div>
        <div className="col-md-3 mb-2">
          <CampoData
            form={form}
            placeholder="Início do Bimestre"
            formatoData="DD/MM/YYYY"
            name={chaveDataInicial}
            onChange={valor => onChangeCamposData(valor)}
            desabilitado={desabilitaCampos}
            diasParaHabilitar={diasParaHabilitar}
          />
          {obterErros(form, 'inicioDoFechamento', indice)}
        </div>
        <div className="col-md-3 mb-2">
          <CampoData
            form={form}
            placeholder="Fim do Bimestre"
            formatoData="DD/MM/YYYY"
            name={chaveDataFinal}
            onChange={onChangeCamposData}
            desabilitado={desabilitaCampos}
          />
          {obterErros(form, 'finalDoFechamento', indice)}
        </div>
      </div>
    );
  };

  return (
    <>
      <Cabecalho pagina="Período de Fechamento (Abertura)" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={periodos}
          validationSchema={validacoes}
          onSubmit={values => onSubmit(values)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12">
              <div className="row mb-4">
                <div className="col-md-12 d-flex justify-content-end pb-4">
                  <Button
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-3"
                    onClick={onClickVoltar}
                  />
                  <Button
                    label="Cancelar"
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-3"
                    disabled={!modoEdicao}
                    onClick={() => onClickCancelar(form)}
                  />
                  <Button
                    label="Cadastrar"
                    color={Colors.Roxo}
                    border
                    bold
                    disabled={!modoEdicao}
                    onClick={() => validaAntesDoSubmit(form)}
                  />
                </div>
                <div className="col-md-12 pb-2">
                  <Loader loading={carregandoTipos} tip="">
                    <div style={{ maxWidth: '300px' }}>
                      <SelectComponent
                        name="tipoCalendarioId"
                        id="tipoCalendarioId"
                        lista={listaTipoCalendarioEscolar}
                        valueOption="id"
                        valueText="descricaoTipoCalendario"
                        onChange={id => setTipoCalendarioSelecionado(id)}
                        valueSelect={tipoCalendarioSelecionado}
                        disabled={desabilitarTipoCalendario}
                        placeholder="Selecione um tipo de calendário"
                      />
                    </div>
                  </Loader>
                </div>
                <br />
                <div className="col-md-6 pb-2">
                  {tipoCalendarioSelecionado && (
                    <DreDropDown
                      label="Diretoria Regional de Educação (DRE)"
                      form={form}
                      onChange={dreId => setDreSelecionada(dreId)}
                      desabilitado={false}
                    />
                  )}
                </div>
                <div className="col-md-6 pb-2">
                  {tipoCalendarioSelecionado && (
                    <UeDropDown
                      dreId={form.values.dreId}
                      label="Unidade Escolar (UE)"
                      form={form}
                      url="v1/dres"
                      onChange={ueId => setUeSelecionada(ueId)}
                      desabilitado={false}
                    />
                  )}
                </div>
              </div>
              <FieldArray
                name="fechamentosBimestres"
                render={arrayHelpers => (
                  <>
                    {periodos.fechamentosBimestres.map((c, indice) =>
                      criaBimestre(
                        form,
                        `${c.bimestre} ° Bimestre`,
                        `fechamentosBimestres[${indice}].inicioDoFechamento`,
                        `fechamentosBimestres[${indice}].finalDoFechamento`,
                        obterDatasParaHabilitar(c.inicioMinimo, c.finalMaximo),
                        indice
                      )
                    )}
                  </>
                )}
              />
            </Form>
          )}
        </Formik>
        <Auditoria
          criadoEm={auditoria.criadoEm}
          criadoPor={auditoria.criadoPor}
          criadoRf={auditoria.criadoRf}
          alteradoPor={auditoria.alteradoPor}
          alteradoEm={auditoria.alteradoEm}
          alteradoRf={auditoria.alteradoRf}
        />
      </Card>
    </>
  );
};

export default PeriodoFechamentoAbertura;
