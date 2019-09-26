import React, { useEffect, useState } from 'react';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import { Colors } from '~/componentes/colors';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import SelectComponent from '~/componentes/select';
import DataTable from '~/componentes/table/dataTable';
import { confirmar, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';

import Card from '../../../componentes/card';

export default function ReiniciarSenha() {
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [listaUsuario, setListaUsuario] = useState([]);

  const [listaDres, setListaDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState('');
  const [listaUes, setListaUes] = useState([]);
  const [ueSelecionada, setUeSelecionada] = useState([]);
  const [nomeUsuarioSelecionado, setNomeUsuarioSelecionado] = useState([]);
  const [rfSelecionado, setRfSelecionado] = useState([]);
  const [emailUsuarioSelecionado, setEmailUsuarioSelecionado] = useState('');
  const [exibirModalReiniciarSenha, setExibirModalReiniciarSenha] = useState(
    false
  );
  const [semEmailCadastrado, setSemEmailCadastrado] = useState(false);
  const [refForm, setRefForm] = useState();

  const [validacoes, setValidacoes] = useState(
    Yup.object({
      emailUsuario: Yup.string()
        .email('Digite um email válido.')
        .required('Campo obrigatório'),
    })
  );

  const colunas = [
    {
      title: 'Nome do usuário',
      dataIndex: 'nomeUsuario',
      width: '50%',
    },
    {
      title: 'Registro Funcional (RF)',
      dataIndex: 'registroFuncional',
      width: '40%',
    },
  ];

  useEffect(() => {
    async function carregarDres() {
      const dres = await api.get('v1/dres');
      setListaDres(dres.data);
    }
    carregarDres();

    // TODO - Mock
    setListaUsuario([
      { id: 1, nomeUsuario: 'Teste mock 01', registroFuncional: '001', emailUsuario:'' },
      { id: 2, nomeUsuario: 'Teste mock 02', registroFuncional: '002', emailUsuario:'' },
      { id: 3, nomeUsuario: 'Teste mock 03', registroFuncional: '003', emailUsuario:'' },
      { id: 4, nomeUsuario: 'Teste mock 04', registroFuncional: '004' },
      { id: 5, nomeUsuario: 'Teste mock 05', registroFuncional: '005' },
      { id: 6, nomeUsuario: 'Teste mock 06', registroFuncional: '006' },
      { id: 7, nomeUsuario: 'Teste mock 07', registroFuncional: '007' },
      { id: 8, nomeUsuario: 'Teste mock 08', registroFuncional: '008' },
    ]);
  }, []);

  function onClickVoltar() {
    console.log('voltar');
  }

  function onChangeDre(dre) {
    setDreSelecionada(dre);
    setUeSelecionada([]);
    setListaUes([]);
    if (dre) {
      carregarUes(dre);
    }
  }

  function onChangeUe(ue) {
    setUeSelecionada(ue);
  }

  function onChangeNomeUsuario(nomeUsuario) {
    setNomeUsuarioSelecionado(nomeUsuario.target.value);
  }

  function onChangeRf(rf) {
    setRfSelecionado(rf.target.value);
  }

  async function carregarUes(dre) {
    const ues = await api.get(`/v1/dres/${dre}/ues`);
    setListaUes(ues.data || []);
  }

  function onClickFiltrar() {
    const paramsQuery = {
      ue: ueSelecionada,
      nomeUsuario: nomeUsuarioSelecionado,
      dre: dreSelecionada,
      rf: rfSelecionado,
    };
    console.log(paramsQuery);
  }

  async function onClickReiniciar() {
    const confirmou = await confirmar(
      'Reiniciar Senha',
      '',
      'Deseja realmente reiniciar essa senha?',
      'Reiniciar',
      'Cancelar'
    );
    if (confirmou) {
      // TODO - MOCK simular sem e-mail cadastrado
      if (selectedRowKeys[0] == 3 || selectedRowKeys[0] == 5) {
        setEmailUsuarioSelecionado('');
        setSemEmailCadastrado(true);
        setExibirModalReiniciarSenha(true);
      } else {
        setSemEmailCadastrado(false);
      }
    }
  }

  function onSelectRow(row) {
    setSelectedRowKeys(row);
    const linhaSelecionada = listaUsuario.find(item => item.id == row[0])
    setEmailUsuarioSelecionado(linhaSelecionada.emailUsuario);
  }

  function onConfirmarReiniciarSenha(form) {
    console.log(form);
    alert(`Chamar endpoint para reiniciar senha ID: ${selectedRowKeys[0]} `);
    onCloseModalReiniciarSenha();
    sucesso(
      `Senha do usuário ${selectedRowKeys[0]} foi reiniciada com sucesso.`
    );
    refForm.resetForm();
  }

  function onCancelarReiniciarSenha() {
    onCloseModalReiniciarSenha();
  }

  function onCloseModalReiniciarSenha() {
    setExibirModalReiniciarSenha(false);
    setSemEmailCadastrado(false);
    refForm.resetForm();
  }

  const validaSeTemEmailCadastrado = () => {
    return semEmailCadastrado
      ? `Este usuário não tem e-mail cadastrado, para seguir com
     o processo de reinicio da senha é obrigatório informar um e-mail válido.`
      : null;
  };

  return (
    <>
      <Cabecalho pagina="Reiniciar senha" />
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickVoltar}
          />
        </div>
        <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
          <SelectComponent
            name="dre-reiniciar-senha"
            id="dre-reiniciar-senha"
            lista={listaDres}
            valueOption="id"
            valueText="nome"
            onChange={onChangeDre}
            valueSelect={dreSelecionada || []}
            label="Diretoria Regional de Educação (DRE)"
            placeholder="Diretoria Regional de Educação (DRE)"
          />
        </div>
        <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
          <SelectComponent
            name="ues-list"
            id="ues-list"
            lista={listaUes}
            valueOption="codigo"
            valueText="nome"
            onChange={onChangeUe}
            valueSelect={ueSelecionada || []}
            label="Unidade Escolar (UE)"
            placeholder="Unidade Escolar (UE)"
          />
        </div>
        <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 pb-3">
          <CampoTexto
            label="Nome do usuário"
            placeholder="Nome do usuário"
            onChange={onChangeNomeUsuario}
            value={nomeUsuarioSelecionado}
          />
        </div>
        <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 pb-3">
          <CampoTexto
            label="Registro Funcional (RF)"
            placeholder="Registro Funcional (RF)"
            onChange={onChangeRf}
            value={rfSelecionado}
          />
        </div>
        <div className="col-md-12">
          <Button
            label="Reiniciar"
            color={Colors.Roxo}
            border
            className="ml-2 text-center d-block mt-4 float-right"
            onClick={onClickReiniciar}
            disabled={selectedRowKeys && selectedRowKeys.length < 1}
          />
          <Button
            label="Aplicar Filtro"
            color={Colors.Azul}
            border
            className="text-center d-block mt-4 float-right"
            onClick={onClickFiltrar}
          />
        </div>

        <div className="col-md-12 pt-4">
          <DataTable
            selectedRowKeys={selectedRowKeys}
            onSelectRow={onSelectRow}
            columns={colunas}
            dataSource={listaUsuario}
          />
        </div>
      </Card>

      <Formik
        ref={(refFormik) => setRefForm(refFormik)}
        enableReinitialize
        initialValues={{
          emailUsuario: emailUsuarioSelecionado,
        }}
        validationSchema={validacoes}
        onSubmit={values => onConfirmarReiniciarSenha(values)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <ModalConteudoHtml
              key="reiniciarSenha"
              visivel={exibirModalReiniciarSenha}
              onConfirmacaoPrincipal={(e)=>{
                form.validateForm().then(() =>form.handleSubmit(e))
              }}
              onConfirmacaoSecundaria={onCancelarReiniciarSenha}
              onClose={onCloseModalReiniciarSenha}
              labelBotaoPrincipal="Cadastrar e reiniciar"
              tituloAtencao={semEmailCadastrado ? 'Atenção' : null}
              perguntaAtencao={validaSeTemEmailCadastrado()}
              labelBotaoSecundario="Cancelar"
              titulo="Reiniciar Senha"
              closable={false}
            >
              <b> Deseja realmente reininicar essa senha? </b>

              <CampoTexto
                label="E-mail"
                name="emailUsuario"
                form={form}
                maxlength="50"
              />
            </ModalConteudoHtml>
          </Form>
        )}
      </Formik>
    </>
  );
}
