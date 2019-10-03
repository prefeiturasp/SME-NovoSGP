import { Form, Formik } from 'formik';
import React, { useState } from 'react';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoData from '~/componentes/campoData';
import Card from '~/componentes/card';
import { Base } from '~/componentes/colors';

const PeriodosEscolares = () => {
  const [validacoes, setValidacoes] = useState(
    Yup.object({
      dataInicial: Yup.date().required('Data inicial obrigatória'),
      dataFinal: Yup.date().required('Data final obrigatória'),
    })
  );

  const [dataInicial, setDataInicial] = useState();
  const [dataFinal, setDataFinal] = useState();

  const onSubmit = data => {
    console.log(data);
  };

  return (
    <>
      <Cabecalho pagina="Cadastro do Período Escolar" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={{
            dataInicial: dataInicial || '',
            dataFinal: dataFinal || '',
          }}
          validationSchema={validacoes}
          onSubmit={values => onSubmit(values)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form>
              <div className="col-md-4">
                <Button
                  type="submit"
                  label="Teste"
                  color={Base.Roxo}
                  // onClick={e => {
                  //   form.validateForm().then(() => form.handleSubmit(e));
                  // }}
                />
              </div>
              <div className="col-md-4">
                <CampoData
                  form={form}
                  placeholder="Início do Bimestre"
                  formatoData="DD/MM/YYYY"
                  label="Início do Bimestre"
                  name="dataInicial"
                />
              </div>
              <div className="col-md-4">
                <CampoData
                  form={form}
                  placeholder="Início do Bimestre"
                  formatoData="DD/MM/YYYY"
                  label="Fim do Bimestre"
                  name="dataFinal"
                />
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default PeriodosEscolares;
