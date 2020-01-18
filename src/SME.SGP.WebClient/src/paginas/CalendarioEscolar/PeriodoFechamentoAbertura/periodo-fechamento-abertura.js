import React, { useState, useEffect } from 'react';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
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
import { CampoData, Loader, momentSchema } from '~/componentes';
import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';

const PeriodoFechamentoAbertura = () => {
  const [listaTipoCalendarioEscolar, setListaTipoCalendarioEscolar] = useState(
    []
  );
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    ''
  );
  const [dreSelecionada, setDreSelecionada] = useState('');
  const [ueSelecionada, setUeSelecionada] = useState('');
  const [ehTipoCalendarioAnual, setEhTipoCalendarioAnual] = useState(true);
  const [desabilitaCampos, setDesabilitaCampos] = useState(false);

  const chaveBimestre = {
    primeiroInicio: 'primeiroBimestreDataInicial',
    primeiroFinal: 'primeiroBimestreDataFinal',
    segundoInicio: 'segundoBimestreDataInicial',
    segundoFinal: 'segundoBimestreDataFinal',
    terceiroInicio: 'terceiroBimestreDataInicial',
    terceiroFinal: 'terceiroBimestreDataFinal',
    quartoInicio: 'quartoBimestreDataInicial',
    quartoFinal: 'quartoBimestreDataFinal',
  };
  const valoresFormInicial = {
    [chaveBimestre.primeiroInicio]: '',
    [chaveBimestre.primeiroFinal]: '',
    [chaveBimestre.segundoInicio]: '',
    [chaveBimestre.segundoFinal]: '',
    [chaveBimestre.terceiroInicio]: '',
    [chaveBimestre.terceiroFinal]: '',
    [chaveBimestre.quartoInicio]: '',
    [chaveBimestre.quartoFinal]: '',
  };
  const [valoresIniciais, setValoresIniciais] = useState(valoresFormInicial);
  const [validacoes, setValidacoes] = useState();
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [desabilitarTipoCalendario, setDesabilitarTipoCalendario] = useState(
    false
  );
  const [desabilitaDre, setDesabilitaDre] = useState(false);
  const [desabilitaUe, setDesabilitaUe] = useState(false);
  const [anoLetivo, setAnoLetivo] = useState(new Date().getFullYear());

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
    const carregarDres = async () => {
      setCarregandoDres(true);
      const dres = await api.get(`v1/abrangencias/false/dres`).finally(() => {
        setCarregandoDres(false);
      });
      if (dres.data) {
        setListaDres(dres.data.sort(FiltroHelper.ordenarLista('nome')));
        if (dres.data.length === 1) {
          setDreSelecionada(dres.data[0].id);
          setDesabilitaDre(true);
        } else {
          setDesabilitaDre(false);
        }
      } else {
        setListaDres([]);
      }
    };
    carregarDres();
  }, []);

  const onChangeCamposData = valor => {};

  const validacaoAnoLetivo = () => {
    return momentSchema.test({
      name: 'teste',
      exclusive: true,
      message: 'Data inváçida',
      test: value => value.year().toString() === anoLetivo.toString(),
    });
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
    let periodos = {};
    if (ehTipoCalendarioAnual) {
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
  }, [ehTipoCalendarioAnual]);

  const onchangeTipoCalendarioEscolar = (id, form) => {
    const tipoSelecionado = listaTipoCalendarioEscolar.find(
      item => item.id == id
    );
    setAnoLetivo(tipoSelecionado.anoLetivo);

    if (tipoSelecionado && tipoSelecionado.periodo == periodo.Anual) {
      setEhTipoCalendarioAnual(true);
    } else {
      setEhTipoCalendarioAnual(false);
    }
    setTipoCalendarioSelecionado(id);
  };

  const onChangeDre = codigo => {
    setDreSelecionada(codigo);
    setUeSelecionada(null);
    carregarUes(codigo);
  };

  const carregarUes = async dre => {
    setCarregandoUes(true);
    const ues = await api
      .get(`/v1/abrangencias/false/dres/${dre}/ues`)
      .finally(() => {
        setCarregandoUes(false);
      });
    if (ues.data) {
      ues.data.forEach(
        ue => (ue.nome = `${tipoEscolaDTO[ue.tipoEscola]} ${ue.nome}`)
      );
      if (ues.data.length === 1) {
        setDesabilitaUe(true);
        setUeSelecionada(ues.data[0].codigo);
      } else {
        setDesabilitaUe(false);
      }
      setListaUes(ues.data.sort(FiltroHelper.ordenarLista('nome')));
    } else {
      setListaUes([]);
    }
  };

  const onChangeUe = codigo => {
    setUeSelecionada(codigo);
  };

  const onClickVoltar = () => {};

  const onClickCancelar = form => {};

  const validaAntesDoSubmit = form => {};

  const onSubmit = form => {};

  const criaBimestre = (form, descricao, chaveDataInicial, chaveDataFinal) => {
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
          />
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
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={values => onSubmit(values)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12">
              <div className="row">
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
                    onClick={() => onClickCancelar(form)}
                  />
                  <Button
                    label="Cadastrar"
                    color={Colors.Roxo}
                    border
                    bold
                    onClick={() => validaAntesDoSubmit(form)}
                  />
                </div>
                <div className="col-md-12 pb-2">
                  <Loader loading={carregandoTipos} tip="">
                    <div style={{ maxWidth: '300px' }}>
                      <SelectComponent
                        name="tipoCalendario"
                        id="tipoCalendario"
                        placeholder="Tipo de Calendário Escolar"
                        lista={listaTipoCalendarioEscolar}
                        valueOption="id"
                        valueText="descricaoTipoCalendario"
                        onChange={id => onchangeTipoCalendarioEscolar(id, form)}
                        valueSelect={tipoCalendarioSelecionado}
                        disabled={desabilitarTipoCalendario}
                      />
                    </div>
                  </Loader>
                </div>
                <br />
                <div className="col-md-6 pb-2">
                  <Loader loading={carregandoDres} tip="">
                    <SelectComponent
                      name="dre"
                      id="dre"
                      placeholder="Diretoria Regional de Educação (DRE)"
                      lista={listaDres}
                      valueOption="codigo"
                      valueText="nome"
                      onChange={codigo => onChangeDre(codigo)}
                      valueSelect={dreSelecionada}
                      disabled={desabilitaDre}
                    />
                  </Loader>
                </div>
                <div className="col-md-6 pb-2">
                  <Loader loading={carregandoUes} tip="">
                    <SelectComponent
                      name="ue"
                      id="ue"
                      placeholder="Unidade Escolar (UE)"
                      lista={listaUes}
                      valueOption="codigo"
                      valueText="nome"
                      onChange={codigo => onChangeUe(codigo)}
                      valueSelect={ueSelecionada}
                      disabled={desabilitaUe}
                    />
                  </Loader>
                </div>
              </div>
              {listaTipoCalendarioEscolar &&
              listaTipoCalendarioEscolar.length &&
              tipoCalendarioSelecionado ? (
                <>
                  {criaBimestre(
                    form,
                    '1 ° Bimestre',
                    chaveBimestre.primeiroInicio,
                    chaveBimestre.primeiroFinal
                  )}
                  {criaBimestre(
                    form,
                    '2 ° Bimestre',
                    chaveBimestre.segundoInicio,
                    chaveBimestre.segundoFinal
                  )}

                  {ehTipoCalendarioAnual ? (
                    <>
                      {criaBimestre(
                        form,
                        '3 ° Bimestre',
                        chaveBimestre.terceiroInicio,
                        chaveBimestre.terceiroFinal
                      )}
                      {criaBimestre(
                        form,
                        '4 ° Bimestre',
                        chaveBimestre.quartoInicio,
                        chaveBimestre.quartoFinal
                      )}
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

export default PeriodoFechamentoAbertura;
