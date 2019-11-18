import React, { useState } from 'react';

// Form
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import {
  Card,
  ButtonGroup,
  Grid,
  Localizador,
  CampoData,
  momentSchema,
} from '~/componentes';

// Styles
import { Row } from './styles';

function AtribuicaoEsporadicaForm() {
  const [valoresIniciais, setValoresIniciais] = useState({
    rfProfessor: null,
    dataInicio: '',
    dataFim: '',
  });
  const validacoes = () => {
    return Yup.object({
      dataInicio: momentSchema.required('Campo obrigatório'),
      dataFim: momentSchema.required('Campo obrigatório'),
      rfProfessor: Yup.number()
        .typeError('Informar um número inteiro')
        .required('Campo obrigatório'),
    });
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.submitForm(form);
      }
    });
  };

  const onClickBotaoPrincipal = form => {
    validaAntesDoSubmit(form);
  };

  return (
    <>
      <Cabecalho pagina="Atribuição" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => console.log(valores)}
          validateOnBlur
          validateOnChange
        >
          {form => (
            <Form>
              <ButtonGroup
                form={form}
                labelBotaoPrincipal="Cadastrar"
                onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                modoEdicao
              />
              <Row className="row">
                <Grid cols={8}>
                  <Row className="row">
                    <Localizador showLabel form={form} onChange={() => null} />
                  </Row>
                </Grid>
                <Grid cols={2}>
                  <CampoData
                    placeholder="Selecione"
                    label="Data Início"
                    form={form}
                    name="dataInicio"
                    formatoData="DD/MM/YYYY"
                  />
                </Grid>
                <Grid cols={2}>
                  <CampoData
                    placeholder="Selecione"
                    label="Data Fim"
                    form={form}
                    name="dataFim"
                    formatoData="DD/MM/YYYY"
                  />
                </Grid>
              </Row>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
}

export default AtribuicaoEsporadicaForm;
