import React, { useState, useEffect } from 'react';

// Form
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

// Serviços
import { erros } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';

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

function AtribuicaoEsporadicaForm({ match }) {
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [auditoria, setAuditoria] = useState({});
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    professorRf: '',
    professorNome: '',
    dataInicio: '',
    dataFim: '',
  });

  const validacoes = () => {
    return Yup.object({
      dataInicio: momentSchema.required('Campo obrigatório'),
      dataFim: momentSchema.required('Campo obrigatório'),
      professorRf: Yup.number()
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

  const buscarPorId = async id => {
    try {
      // const registro = await Servico.get('xx/yy');
      console.log(id);
      const registro = await Promise.resolve({
        status: 200,
        data: {
          anoLetivo: 2019,
          dataFim: window.moment('2019-11-25T18:28:50.0331712+00:00'),
          dataInicio: window.moment('2019-11-18T18:28:50.0331756+00:00'),
          dreId: '1',
          excluido: false,
          id: 1,
          migrado: false,
          professorNome: 'Caíque Latorre 1',
          professorRf: '7777710',
          ueId: '1',
        },
      });
      if (registro && registro.data) {
        setValoresIniciais(registro.data);
      }
    } catch (error) {
      erros(error);
    }
  };

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setBreadcrumbManual(
        match.url,
        'Atribuição',
        '/gestao/atribuicao-esporadica'
      );
      buscarPorId(match.params.id);
    }
  }, []);

  useEffect(() => {
    if (Object.keys(refForm).length !== 0) {
      console.log(refForm);
    }
  }, [refForm]);

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
          ref={formik => setRefForm(formik)}
        >
          {form => (
            <Form>
              <ButtonGroup
                form={form}
                labelBotaoPrincipal="Cadastrar"
                onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                onClickCancelar={() => null}
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
