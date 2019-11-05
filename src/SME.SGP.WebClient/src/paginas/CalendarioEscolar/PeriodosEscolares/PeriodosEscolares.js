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
import { sucesso, confirmar, erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import periodo from '~/dtos/periodo';
import { useSelector } from 'react-redux';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const PeriodosEscolares = () => {
  const [listaCalendarioEscolar, setListaCalendarioEscolar] = useState([]);
  const [
    calendarioEscolarSelecionado,
    setCalendarioEscolarSelecionado,
  ] = useState('');
  const [isTipoCalendarioAnual, setIsTipoCalendarioAnual] = useState(true);
  const [validacoes, setValidacoes] = useState();
  const [modoEdicao, setModoEdicao] = useState(false);
  const [periodoEscolarEdicao, setPeriodoEscolarEdicao] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    primeiroBimestreDataInicial: '',
    primeiroBimestreDataFinal: '',
    segundoBimestreDataInicial: '',
    segundoBimestreDataFinal: '',
    terceiroBimestreDataInicial: '',
    terceiroBimestreDataFinal: '',
    quartoBimestreDataInicial: '',
    quartoBimestreDataFinal: '',
  });
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.PERIODOS_ESCOLARES];
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [desabilitaCampos, setDesabilitaCampos] = useState(false);

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
    async function consultaTipos() {
      const listaTipo = await api.get('v1/calendarios/tipos');
      if (listaTipo && listaTipo.data && listaTipo.data.length) {
        listaTipo.data.map(item => {
          item.id = String(item.id);
          item.descricaoTipoCalendario = `${item.anoLetivo} - ${item.nome} - ${item.descricaoPeriodo}`;
        });
        setListaCalendarioEscolar(listaTipo.data);
      } else {
        setListaCalendarioEscolar([]);
      }
    }
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
    consultaTipos();
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

  const onSubmit = async dadosForm => {
    if (periodoEscolarEdicao) {
      periodoEscolarEdicao.periodos.forEach(item => {
        switch (item.bimestre) {
          case 1:
            item.periodoInicio = dadosForm.primeiroBimestreDataInicial.toDate();
            item.periodoFim = dadosForm.primeiroBimestreDataFinal.toDate();
            break;
          case 2:
            item.periodoInicio = dadosForm.segundoBimestreDataInicial.toDate();
            item.periodoFim = dadosForm.segundoBimestreDataFinal.toDate();
            break;
          case 3:
            item.periodoInicio = dadosForm.terceiroBimestreDataInicial.toDate();
            item.periodoFim = dadosForm.terceiroBimestreDataFinal.toDate();
            break;
          case 4:
            item.periodoInicio = dadosForm.quartoBimestreDataInicial.toDate();
            item.periodoFim = dadosForm.quartoBimestreDataFinal.toDate();
            break;
          default:
            break;
        }
      });
      const editado = await api
        .post('v1/periodo-escolar', periodoEscolarEdicao)
        .catch(e => erros(e));
      if (editado && editado.status == 200) {
        sucesso('Suas informações foram editadas com sucesso.');
      }
    } else {
      const calendarioParaCadastrar = listaCalendarioEscolar.find(item => {
        return item.id == calendarioEscolarSelecionado;
      });
      const paramsCadastrar = {
        periodos: [
          {
            id: 0,
            bimestre: 1,
            periodoInicio: dadosForm.primeiroBimestreDataInicial.toDate(),
            periodoFim: dadosForm.primeiroBimestreDataFinal.toDate(),
          },
          {
            id: 0,
            bimestre: 2,
            periodoInicio: dadosForm.segundoBimestreDataInicial.toDate(),
            periodoFim: dadosForm.segundoBimestreDataFinal.toDate(),
          },
        ],
        tipoCalendario: calendarioParaCadastrar.id,
        anoBase: calendarioParaCadastrar.anoLetivo,
      };

      if (isTipoCalendarioAnual) {
        paramsCadastrar.periodos.push(
          {
            id: 0,
            bimestre: 3,
            periodoInicio: dadosForm.terceiroBimestreDataInicial.toDate(),
            periodoFim: dadosForm.terceiroBimestreDataFinal.toDate(),
          },
          {
            id: 0,
            bimestre: 4,
            periodoInicio: dadosForm.quartoBimestreDataInicial.toDate(),
            periodoFim: dadosForm.quartoBimestreDataFinal.toDate(),
          }
        );
      }
      const cadastrado = await api
        .post('v1/periodo-escolar', paramsCadastrar)
        .catch(e => erros(e));
      if (cadastrado && cadastrado.status == 200) {
        sucesso('Suas informações foram salvas com sucesso.');
      }
    }
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
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

  const onchangeCalendarioEscolar = (id, form) => {
    const tipoSelecionado = listaCalendarioEscolar.find(item => item.id == id);

    if (tipoSelecionado && tipoSelecionado.periodo == periodo.Anual) {
      setIsTipoCalendarioAnual(true);
    } else {
      setIsTipoCalendarioAnual(false);
    }
    setCalendarioEscolarSelecionado(id);
    resetarTela(form);
    consultarPeriodoPorId(id);
  };

  const consultarPeriodoPorId = async id => {
    const periodoAtual = await api.get('v1/periodo-escolar', {
      params: { codigoTipoCalendario: id },
    });

    const bimestresValorInicial = {};
    if (
      periodoAtual.data &&
      periodoAtual.data.periodos &&
      periodoAtual.data.periodos.length
    ) {
      setDesabilitaCampos(!permissoesTela.podeAlterar || somenteConsulta);
      periodoAtual.data.periodos.forEach(item => {
        switch (item.bimestre) {
          case 1:
            bimestresValorInicial.primeiroBimestreDataInicial = window.moment(item.periodoInicio);
            bimestresValorInicial.primeiroBimestreDataFinal = window.moment(item.periodoFim);
            break;
          case 2:
            bimestresValorInicial.segundoBimestreDataInicial = window.moment(item.periodoInicio);
            bimestresValorInicial.segundoBimestreDataFinal = window.moment(item.periodoFim);
            break;
          case 3:
            bimestresValorInicial.terceiroBimestreDataInicial = window.moment(item.periodoInicio);
            bimestresValorInicial.terceiroBimestreDataFinal = window.moment(item.periodoFim);
            break;
          case 4:
            bimestresValorInicial.quartoBimestreDataInicial = window.moment(item.periodoInicio);
            bimestresValorInicial.quartoBimestreDataFinal = window.moment(item.periodoFim);
            break;
          default:
            break;
        }
      });
    }else{
      setDesabilitaCampos(!permissoesTela.podeIncluir || somenteConsulta);
    }
    setPeriodoEscolarEdicao(periodoAtual.data);
    setValoresIniciais(bimestresValorInicial);
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
    setValoresIniciais({});
  };

  const onChangeCamposData = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const primeiroBimestre = form => {
    return (
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
            desabilitado={desabilitaCampos}
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
            desabilitado={desabilitaCampos}
          />
        </div>
      </div>
    );
  };

  const segundoBimestre = form => {
    return (
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
            desabilitado={desabilitaCampos}
          />
        </div>
        <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
          <CampoData
            form={form}
            placeholder="Início do Bimestre"
            formatoData="DD/MM/YYYY"
            name="segundoBimestreDataFinal"
            onChange={onChangeCamposData}
            desabilitado={desabilitaCampos}
          />
        </div>
      </div>
    );
  };

  const terceiroBimestre = form => {
    return (
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
            desabilitado={desabilitaCampos}
          />
        </div>
        <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
          <CampoData
            form={form}
            placeholder="Início do Bimestre"
            formatoData="DD/MM/YYYY"
            name="terceiroBimestreDataFinal"
            onChange={onChangeCamposData}
            desabilitado={desabilitaCampos}
          />
        </div>
      </div>
    );
  };

  const quartoBimestre = form => {
    return (
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
            desabilitado={desabilitaCampos}
          />
        </div>
        <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
          <CampoData
            form={form}
            placeholder="Início do Bimestre"
            formatoData="DD/MM/YYYY"
            name="quartoBimestreDataFinal"
            onChange={onChangeCamposData}
            desabilitado={desabilitaCampos}
          />
        </div>
      </div>
    );
  };

  return (
    <>
      <Cabecalho pagina="Cadastro do período escolar" />
      <Card>
        <Formik
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
                    valueText="descricaoTipoCalendario"
                    onChange={ id => onchangeCalendarioEscolar(id, form)}
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
                    onClick={() => onClickCancelar(form)}
                    disabled={!modoEdicao || desabilitaCampos}
                  />
                  <Button
                    label="Cadastrar"
                    color={Colors.Roxo}
                    border
                    bold
                    type="submit"
                    disabled={!calendarioEscolarSelecionado || desabilitaCampos}
                  />
                </div>
              </div>
              {listaCalendarioEscolar &&
              listaCalendarioEscolar.length &&
              calendarioEscolarSelecionado ? (
                <>
                  {primeiroBimestre(form)}
                  {segundoBimestre(form)}

                  {isTipoCalendarioAnual ? (
                    <>
                      {terceiroBimestre(form)}
                      {quartoBimestre(form)}
                    </>
                  ) : (
                    ''
                  )}
                </>
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
