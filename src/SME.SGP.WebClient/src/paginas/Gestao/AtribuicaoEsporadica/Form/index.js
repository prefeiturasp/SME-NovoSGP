import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

// Redux
import { useSelector } from 'react-redux';

// Serviços
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import { erros, erro, sucesso } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

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
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const filtroListagem = useSelector(
    store => store.atribuicaoEsporadica.filtro
  );
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

  const onSubmitFormulario = async valores => {
    try {
      const cadastrado = await AtribuicaoEsporadicaServico.salvarAtribuicaoEsporadica(
        {
          ...filtroListagem,
          ...valores,
        }
      );
      if (cadastrado && cadastrado.status === 200) {
        sucesso('Atribuição esporádica salva com sucesso.');
        history.push('/gestao/atribuicao-esporadica');
      }
    } catch (err) {
      if (err) {
        erro(err.response.data.mensagens[0]);
      }
    }
  };

  const onClickVoltar = () => history.push('/gestao/atribuicao-esporadica');

  const buscarPorId = async id => {
    try {
      const registro = await AtribuicaoEsporadicaServico.buscarAtribuicaoEsporadica(
        id
      );
      if (registro && registro.data) {
        setValoresIniciais({
          ...registro.data,
          dataInicio: window.moment(registro.data.dataInicio),
          dataFim: window.moment(registro.data.dataFim),
        });
      }
    } catch (err) {
      erros(err);
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

  return (
    <>
      <Cabecalho pagina="Atribuição" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => onSubmitFormulario(valores)}
          ref={formik => setRefForm(formik)}
          validateOnBlur
          validateOnChange
        >
          {form => (
            <Form>
              <ButtonGroup
                form={form}
                permissoesTela={
                  permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]
                }
                novoRegistro={novoRegistro}
                labelBotaoPrincipal="Cadastrar"
                onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                onClickCancelar={() => null}
                onClickVoltar={() => onClickVoltar()}
                modoEdicao
              />
              <Row className="row">
                <Grid cols={8}>
                  <Row className="row">
                    <Localizador
                      dreId={filtroListagem.dreId}
                      anoLetivo={filtroListagem.anoLetivo}
                      showLabel
                      form={form}
                      onChange={() => null}
                    />
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

AtribuicaoEsporadicaForm.propTypes = {
  match: PropTypes.objectOf(PropTypes.object),
};

AtribuicaoEsporadicaForm.defaultProps = {
  match: {},
};

export default AtribuicaoEsporadicaForm;
