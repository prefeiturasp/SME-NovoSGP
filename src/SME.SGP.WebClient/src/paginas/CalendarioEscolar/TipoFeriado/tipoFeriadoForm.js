import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import RadioGroupButton from '~/componentes/radioGroupButton';

import history from '~/servicos/history';
import { sucesso, confirmar } from '~/servicos/alertas';
import Auditoria from '~/componentes/auditoria';
import SelectComponent from '~/componentes/select';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData.js';
import {setBreadcrumbManual} from '~/servicos/breadcrumb-services';

const TipoFeriadoForm = ({ match }) => {

  const [refForm, setRefForm] = useState();
  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [listaDropdownAbrangencia, setListaDropdownAbrangencia] = useState([]);
  const [idTipoFeriadoEdicao, setIdTipoFeriadoEdicao] = useState(0);

  const [valoresIniciais, setValoresIniciais] = useState({
    nome: '',
    abrangencia: undefined,
    tipo: '',
    dataFeriado: '',
    situacao: true,
  });

  const [validacoes] = useState(
    Yup.object({
      nome: Yup.string().required('Nome obrigatório').max(50, 'Máximo 50 caracteres'),
      abrangencia: Yup.string().required('Abrangência obrigatória'),
      tipo: Yup.string().required('Tipo obrigatória'),
      dataFeriado: momentSchema.required('Data obrigatória'),
      situacao: Yup.string().required('Situação obrigatória'),
    })
  );

  const opcoesTipo = [
    { label: 'Fixo', value: 1 },
    { label: 'Móvel', value: 2 },
  ];

  const opcoesSituacao = [
    { label: 'Ativo', value: true },
    { label: 'Inativo', value: false },
  ];

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setBreadcrumbManual(match.url,'Alterar Tipo de Feriado', '/calendario-escolar/tipo-feriado');
      // TODO Chamar endpoint consultar por ID
      // MOCK
      console.log(`Editando ID: ${match.params.id}`);
      setIdTipoFeriadoEdicao(match.params.id);
      setValoresIniciais({
        nome: 'Feriado teste mock',
        abrangencia: '1',
        tipo: 1,
        dataFeriado: new window.moment(),
        situacao: true,
      });
      setNovoRegistro(false);
      setAuditoria({
        criadoPor: 'ELISANGELA DOS SANTOS ARRUDA',
        criadoRf: '1234567',
        criadoEm: new window.moment(),
        alteradoPor: 'JOÃO DA SILVA',
        alteradoRf: '7654321',
        alteradoEm: new window.moment(),
      });
    } else {
      console.log('Novo registro');
    }
  }, []);

  useEffect(() => {
    // TODO - Mock - Chamar endpoint?
    setListaDropdownAbrangencia([
      { id: 1, nome: 'Nacional' },
      { id: 2, nome: 'Estadual' },
      { id: 3, nome: 'Municipal' },
    ]);
  }, []);

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja voltar para tela de listagem agora?'
      );
      if (confirmado) {
        history.push('/calendario-escolar/tipo-feriado');
      }
    } else {
      history.push('/calendario-escolar/tipo-feriado');
    }
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

  const resetarTela = () => {
    refForm.resetForm();
    setModoEdicao(false);
  };

  const onClickCadastrar = valoresForm => {
    console.log(valoresForm);
    if (idTipoFeriadoEdicao) {
      sucesso('Tipo de feriado alterado com sucesso.');
    } else {
      sucesso('Novo tipo de feriado criado com sucesso.');
    }
    history.push('/calendario-escolar/tipo-feriado');
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      const confirmado = await confirmar(
        'Excluir tipo de feriado',
        '',
        'Deseja realmente excluir este feriado?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        // TODO Chamar endpoint de excluir
        sucesso('Tipo de feriado excluído com sucesso.');
        history.push('/calendario-escolar/tipo-feriado');
      }
    }
  };

  return (
    <>
      <Cabecalho pagina={`${idTipoFeriadoEdicao > 0 ? 'Alterar' : 'Cadastro de'} Tipo de Feriado`} />
      <Card>
        <Formik
          ref={refFormik => setRefForm(refFormik)}
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => onClickCadastrar(valores)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
                <Button
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  className="mr-2"
                  onClick={onClickCancelar}
                  disabled={!modoEdicao}
                />
                <Button
                  label="Excluir"
                  color={Colors.Vermelho}
                  border
                  className="mr-2"
                  disabled={novoRegistro}
                  onClick={onClickExcluir}
                />
                <Button
                  label={`${idTipoFeriadoEdicao > 0 ? 'Alterar' : 'Cadastrar'}`}
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  type="submit"
                />
              </div>

              <div className="row">
                <div className="col-sm-12 col-md-8 col-lg-8 col-xl-8 mb-2">
                  <CampoTexto
                    form={form}
                    label="Nome do feriado"
                    placeholder="Meu novo feriado"
                    name="nome"
                    onChange={onChangeCampos}
                  />
                </div>

                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
                  <SelectComponent
                     form={form}
                    label="Abrangência"
                    name="abrangencia"
                    lista={listaDropdownAbrangencia}
                    valueOption="id"
                    valueText="nome"
                    onChange={onChangeCampos}
                    placeholder="Abrangência do feriado"
                  />
                </div>

                <div className="col-sm-12 col-md-4 col-lg-3 col-xl-2 mb-2">
                  <RadioGroupButton
                    label="Tipo"
                    form={form}
                    opcoes={opcoesTipo}
                    name="tipo"
                    onChange={onChangeCampos}
                  />
                </div>

                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-3">
                  <CampoData
                    form={form}
                    label="Data do feriado"
                    placeholder="Data do feriado"
                    formatoData="DD/MM/YYYY"
                    name="dataFeriado"
                    onChange={onChangeCampos}
                  />
                </div>

                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    label="Situação"
                    form={form}
                    opcoes={opcoesSituacao}
                    name="situacao"
                    valorInicial
                    onChange={onChangeCampos}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
        <Auditoria
          criadoEm={auditoria.criadoEm}
          criadoPor={auditoria.criadoPor}
          alteradoPor={auditoria.alteradoPor}
          alteradoEm={auditoria.alteradoEm}
        />
      </Card>
    </>
  );
};

export default TipoFeriadoForm;
