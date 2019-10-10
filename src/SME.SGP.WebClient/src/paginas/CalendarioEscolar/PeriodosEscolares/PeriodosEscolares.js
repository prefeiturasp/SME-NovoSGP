import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData.js';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import SelectComponent from '~/componentes/select';

import { BoxTextoBimetre, CaixaBimestre } from './PeriodosEscoladres.css';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import { sucesso, confirmar } from '~/servicos/alertas';

const PeriodosEscolares = () => {
  const [listaCalendarioEscolar, setListaCalendarioEscolar] = useState([]);
  const [
    calendarioEscolarSelecionado,
    setCalendarioEscolarSelecionado,
  ] = useState('1');
  const [isTipoCalendarioAnual, setIsTipoCalendarioAnual] = useState(true);
  const [validacoes, setValidacoes] = useState();
  const [refForm, setRefForm] = useState();
  const [modoEdicao, setModoEdicao] = useState(false);

  const valoresIniciais = {
    primeiroBimestreDataInicial: '',
    primeiroBimestreDataFinal: '',
    segundoBimestreDataInicial: '',
    segundoBimestreDataFinal: '',
    terceiroBimestreDataInicial: '',
    terceiroBimestreDataFinal: '',
    quartoBimestreDataInicial: '',
    quartoBimestreDataFinal: '',
  };

  const validacaoPrimeiroBim = {
    primeiroBimestreDataInicial: momentSchema.required(
      'Data inicial obrigatória'
    ),
    primeiroBimestreDataFinal: momentSchema
      .required('Data final obrigatória')
      .dataMenorIgualQue(
        'primeiroBimestreDataInicial',
        'primeiroBimestreDataFinal',
        'Data inválida'
      ),
  };

  const validacaoSegundoBim = {
    segundoBimestreDataInicial: momentSchema
      .required('Data inicial obrigatória')
      .dataMenorIgualQue(
        'primeiroBimestreDataFinal',
        'segundoBimestreDataInicial',
        'Data inválida'
      ),
    segundoBimestreDataFinal: momentSchema
      .required('Data final obrigatória')
      .dataMenorIgualQue(
        'segundoBimestreDataInicial',
        'segundoBimestreDataFinal',
        'Data inválida'
      ),
  };

  const validacaoTerceiroBim = {
    terceiroBimestreDataInicial: momentSchema
      .required('Data inicial obrigatória')
      .dataMenorIgualQue(
        'segundoBimestreDataFinal',
        'terceiroBimestreDataInicial',
        'Data inválida'
      ),
    terceiroBimestreDataFinal: momentSchema
      .required('Data final obrigatória')
      .dataMenorIgualQue(
        'terceiroBimestreDataInicial',
        'terceiroBimestreDataFinal',
        'Data inválida'
      ),
  };

  const validacaoQuartoBim = {
    quartoBimestreDataInicial: momentSchema
      .required('Data inicial obrigatória')
      .dataMenorIgualQue(
        'terceiroBimestreDataFinal',
        'quartoBimestreDataInicial',
        'Data inválida'
      ),
    quartoBimestreDataFinal: momentSchema
      .required('Data final obrigatória')
      .dataMenorIgualQue(
        'quartoBimestreDataInicial',
        'quartoBimestreDataFinal',
        'Data inválida'
      ),
  };

  useEffect(() => {
    const lista = [
      { id: '1', nome: '2019 - Calendário escolar - Anual', anual: true },
      { id: '2', nome: '2019 - Calendário escolar - Semestral', anual: false },
    ];
    setListaCalendarioEscolar(lista);
  }, []);

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
  }, [isTipoCalendarioAnual]);

  const onSubmit = dados => {
    console.log(dados);
    sucesso('Suas informações foram salvas com sucesso.');
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        resetarTela();
      }
    }
  };

  const onchangeCalendarioEscolar = valor => {
    const anual = listaCalendarioEscolar.find(
      item => item.id == valor && !!item.anual
    );
    if (anual) {
      setIsTipoCalendarioAnual(true);
    } else {
      setIsTipoCalendarioAnual(false);
    }
    setCalendarioEscolarSelecionado(valor);
    resetarTela();
  };

  const resetarTela = () => {
    refForm.resetForm();
    setModoEdicao(false);
  };

  const onChangeCamposData = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  return (
    <>
      <Cabecalho pagina="Cadastro do período escolar" />
      <Card>
        <Formik
          ref={refFormik => setRefForm(refFormik)}
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={values => onSubmit(values)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12">
              <div className="row">
                <div className="col-sm-12 col-md-5 col-lg-4 col-xl-4 mb-4">
                  <SelectComponent
                    name="calEscolar"
                    id="calEscolar"
                    lista={listaCalendarioEscolar}
                    valueOption="id"
                    valueText="nome"
                    onChange={onchangeCalendarioEscolar}
                    valueSelect={calendarioEscolarSelecionado}
                  />
                </div>
                <div className="col-sm-12 col-md-7 col-lg-8 col-xl-8 d-flex justify-content-end mb-4">
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
                    onClick={onClickCancelar}
                    disabled={!modoEdicao}
                  />
                  <Button
                    label="Cadastrar"
                    color={Colors.Roxo}
                    border
                    bold
                    type="submit"
                  />
                </div>
              </div>

              <div className="row">
                <div className="col-sm-4 col-md-4 col-lg-2 col-xl-2 mb-2">
                  <Label text="Bimestre" control="bimestre" />
                  <CaixaBimestre>
                    <BoxTextoBimetre>1 ° Bimestre</BoxTextoBimetre>
                  </CaixaBimestre>
                </div>
                <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                  <CampoData
                    form={form}
                    placeholder="Início do Bimestre"
                    formatoData="DD/MM/YYYY"
                    label="Início do Bimestre"
                    name="primeiroBimestreDataInicial"
                    onChange={onChangeCamposData}
                  />
                </div>
                <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                  <CampoData
                    form={form}
                    placeholder="Início do Bimestre"
                    formatoData="DD/MM/YYYY"
                    label="Fim do Bimestre"
                    name="primeiroBimestreDataFinal"
                    onChange={onChangeCamposData}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-sm-4 col-md-4 col-lg-2 col-xl-2 mb-2">
                  <CaixaBimestre>
                    <BoxTextoBimetre>2 ° Bimestre</BoxTextoBimetre>
                  </CaixaBimestre>
                </div>
                <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                  <CampoData
                    form={form}
                    placeholder="Início do Bimestre"
                    formatoData="DD/MM/YYYY"
                    name="segundoBimestreDataInicial"
                    onChange={onChangeCamposData}
                  />
                </div>
                <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                  <CampoData
                    form={form}
                    placeholder="Início do Bimestre"
                    formatoData="DD/MM/YYYY"
                    name="segundoBimestreDataFinal"
                    onChange={onChangeCamposData}
                  />
                </div>
              </div>

              {isTipoCalendarioAnual ? (
                <div className="row">
                  <div className="col-sm-4 col-md-4 col-lg-2 col-xl-2 mb-2">
                    <CaixaBimestre>
                      <BoxTextoBimetre>3 ° Bimestre</BoxTextoBimetre>
                    </CaixaBimestre>
                  </div>
                  <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                    <CampoData
                      form={form}
                      placeholder="Início do Bimestre"
                      formatoData="DD/MM/YYYY"
                      name="terceiroBimestreDataInicial"
                      onChange={onChangeCamposData}
                    />
                  </div>
                  <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                    <CampoData
                      form={form}
                      placeholder="Início do Bimestre"
                      formatoData="DD/MM/YYYY"
                      name="terceiroBimestreDataFinal"
                      onChange={onChangeCamposData}
                    />
                  </div>
                </div>
              ) : (
                ''
              )}

              {isTipoCalendarioAnual ? (
                <div className="row">
                  <div className="col-sm-4 col-md-4 col-lg-2 col-xl-2 mb-2">
                    <CaixaBimestre>
                      <BoxTextoBimetre>4 ° Bimestre</BoxTextoBimetre>
                    </CaixaBimestre>
                  </div>
                  <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                    <CampoData
                      form={form}
                      placeholder="Início do Bimestre"
                      formatoData="DD/MM/YYYY"
                      name="quartoBimestreDataInicial"
                      onChange={onChangeCamposData}
                    />
                  </div>
                  <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                    <CampoData
                      form={form}
                      placeholder="Início do Bimestre"
                      formatoData="DD/MM/YYYY"
                      name="quartoBimestreDataFinal"
                      onChange={onChangeCamposData}
                    />
                  </div>
                </div>
              ) : (
                ''
              )}
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default PeriodosEscolares;
