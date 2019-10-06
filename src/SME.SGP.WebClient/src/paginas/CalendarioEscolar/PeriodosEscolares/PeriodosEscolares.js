import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import SelectComponent from '~/componentes/select';

import { BoxTextoBimetre, CaixaBimestre } from './PeriodosEscoladres.css';


const PeriodosEscolares = () => {
  const [validacoes, setValidacoes] = useState();

  const [primeiroBimDataInicial, setPrimeiroBimDataInicial] = useState();
  const [primeiroBimDataFinal, setPrimeiroBimDataFinal] = useState();

  const [valoresDatas, setValoresDatas] = useState({
    primeiroBimDataInicial: '',
    primeiroBimDataFinal: '',
  });

  const [listaCalendarioEscolar, setListaCalendarioEscolar] = useState([]);
  const [
    calendarioEscolarSelecionado,
    setCalendarioEscolarSelecionado,
  ] = useState('1');

  useEffect(() => {
    montarMetodosYup();
    montarValidacoesYup();

    const lista = [
      { id: '1', nome: '2019 - Calendário escolar' },
      { id: '2', nome: '2018 - Calendário escolar' },
    ];
    setListaCalendarioEscolar(lista);
  }, []);

  // useEffect(() => {
  //   console.log(' A - useEffect - A');
  //   montarValidacoesYup();
  // }, [primeiroBimDataFinal, primeiroBimDataInicial]);

  const montarValidacoesYup = () => {

    const validacaoCampoData = Yup.object().shape({
      primeiroBimDataInicial: momentSchema.required('Data inicial obrigatória'),
      primeiroBimDataFinal: momentSchema
        .required('Data final obrigatória')
        .isMenor('primeiroBimDataInicial', 'Data inválida',valoresDatas)
    });
    setValidacoes(validacaoCampoData);
  };

  const montarMetodosYup = () => {
    Yup.addMethod(Yup.mixed, 'isMenor', function(
      nomeCampoComparacao,
      mensagem,
      valores
    ) {

      return this.test( 'isMenor', mensagem, function(dataCampo) {
        let dataValida = false;

        const dataComparacao = valores[nomeCampoComparacao];

        if (
          dataCampo &&
          dataComparacao &&
          dataCampo.isAfter(dataComparacao, 'date')
        ) {
          dataValida = true;
        }
        return dataValida;
      });
    });
  };

  const onSubmit = data => {
    console.log(data);
  };

  const onClickVoltar = () => {
    console.log('onClickVoltar');
  };

  const onClickCancelar = () => {
    console.log('onClickCancelar');
  };

  const onchangeCalendarioEscolar = valor => {
    console.log(valor);
    setCalendarioEscolarSelecionado(valor);
  };

  const onChangePrimeiroBimDataInicial = data => {
    const datas = valoresDatas;
    setValoresDatas({
      ...datas,
      primeiroBimDataInicial: data,
    });
    // setPrimeiroBimDataInicial(data);
  };

  const onChangePrimeiroBimDataFinal = data => {
    const datas = valoresDatas;
    setValoresDatas({
      ...datas,
      primeiroBimDataFinal: data,
    });
    // setPrimeiroBimDataFinal(data);
  };

  return (
    <>
      <Cabecalho pagina="Cadastro do período escolar" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={{
            primeiroBimDataInicial: primeiroBimDataInicial || '',
            primeiroBimDataFinal: primeiroBimDataFinal || '',
          }}
          validationSchema={validacoes}
          onSubmit={values => onSubmit(values)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form>
              <div className="row">
                <div className="col-sm-12 col-md-5 col-lg-3 col-xl-3 mb-4">
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
                <div className="col-sm-12 col-md-7 col-lg-9 col-xl-9 d-flex justify-content-end mb-4">
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
                    name="primeiroBimDataInicial"
                    onChange={onChangePrimeiroBimDataInicial}
                  />
                </div>
                <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
                  <CampoData
                    form={form}
                    placeholder="Início do Bimestre"
                    formatoData="DD/MM/YYYY"
                    label="Fim do Bimestre"
                    name="primeiroBimDataFinal"
                    onChange={onChangePrimeiroBimDataFinal}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default PeriodosEscolares;
