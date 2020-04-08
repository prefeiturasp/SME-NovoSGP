import React, { useState, useCallback, useEffect } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
// import styled from 'styled-components';
// import shortid from 'shortid';

import { Form, Formik } from 'formik';
import * as Yup from 'yup';

import { Cabecalho } from '~/componentes-sgp';
import {
  Loader,
  Card,
  ButtonGroup,
  Grid,
  SelectComponent,
  CampoTexto,
  CampoData,
  Label,
} from '~/componentes';

import { Linha } from '~/componentes/EstilosGlobais';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const ComunicadosCadastro = ({ match }) => {
  const [loaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  useCallback(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const [idComunicado, setIdComunicado] = useState();

  useEffect(() => {
    if (match && match.params && match.params.id)
      setIdComunicado(match.params.id);
  }, [match]);

  const [refForm, setRefForm] = useState({});

  const gruposLista = [
    { Id: 1, Nome: 'EJA' },
    { Id: 2, Nome: 'Médio' },
    { Id: 3, Nome: 'Fundamental' },
    { Id: 4, Nome: 'EMEBS' },
    { Id: 5, Nome: 'CEI' },
    { Id: 6, Nome: 'EMEI' },
  ];

  const [valoresIniciais] = useState({
    grupoId: [],
    titulo: '',
    dataExpiracaoInicio: '',
    dataExpiracaoFim: '',
  });

  const validacoes = () => {
    return Yup.object({});
  };

  const validarAntesDeSalvar = valores => {
    const formContext = refForm && refForm.getFormikContext();
    if (formContext.isValid && Object.keys(formContext.errors).length === 0) {
      console.log(valores);
    }
  };

  const onClickExcluir = async () => {};

  const onClickVoltar = () => {
    history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
  };

  const onClickBotaoPrincipal = () => {};

  return (
    <>
      <Cabecalho pagina="Comunicação com pais ou responsáveis" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]}
            onClickExcluir={onClickExcluir}
            onClickVoltar={onClickVoltar}
            onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal={idComunicado ? 'Alterar' : 'Cadastrar'}
          />
          <Formik
            enableReinitialize
            initialValues={valoresIniciais}
            validationSchema={validacoes()}
            onSubmit={valores => validarAntesDeSalvar(valores)}
            ref={refFormik => setRefForm(refFormik)}
            validate={valores => validarAntesDeSalvar(valores)}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <Linha className="row mb-2">
                  <Grid cols={4}>
                    <Label control="grupoId" text="Grupo" />
                    <SelectComponent
                      form={form}
                      name="grupoId"
                      placeholder="Selecione um grupo"
                      value={form.values.gruposId}
                      multiple
                      lista={gruposLista}
                      valueOption="Id"
                      valueText="Nome"
                    />
                  </Grid>
                  <Grid cols={2}>
                    <Label control="dataEnvioInicio" text="Data de envio" />
                    <CampoData
                      form={form}
                      name="dataEnvioInicio"
                      placeholder="Data início"
                      formatoData="DD/MM/YYYY"
                      // onChange={data => validaDataInicio(data)}
                    />
                  </Grid>
                  <Grid cols={2}>
                    <Label
                      control="dataEnvioFim"
                      text="Data de envio"
                      className="text-white"
                    />
                    <CampoData
                      form={form}
                      name="dataEnvioFim"
                      placeholder="Data fim"
                      formatoData="DD/MM/YYYY"
                      // onChange={data => validaDataFim(data)}
                    />
                  </Grid>
                  <Grid cols={2}>
                    <Label
                      control="dataExpiracaoInicio"
                      text="Data de expiração"
                    />
                    <CampoData
                      form={form}
                      name="dataExpiracaoInicio"
                      placeholder="Data início"
                      formatoData="DD/MM/YYYY"
                      // onChange={data => validaDataInicio(data)}
                    />
                  </Grid>
                  <Grid cols={2}>
                    <Label
                      control="dataExpiracaoFim"
                      text="Data de expiração"
                      className="text-white"
                    />
                    <CampoData
                      form={form}
                      name="dataExpiracaoFim"
                      placeholder="Data fim"
                      formatoData="DD/MM/YYYY"
                      // onChange={data => validaDataFim(data)}
                    />
                  </Grid>
                </Linha>
                <Linha className="row">
                  <Grid cols={12}>
                    <Label control="titulo" text="Tíutulo" />
                    <CampoTexto
                      form={form}
                      name="titulo"
                      placeholder="Procure pelo título do comunicado"
                      value={form.values.titulo}
                    />
                  </Grid>
                </Linha>
              </Form>
            )}
          </Formik>
        </Card>
      </Loader>
    </>
  );
};

ComunicadosCadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

ComunicadosCadastro.defaultProps = {
  match: {},
};

export default ComunicadosCadastro;
