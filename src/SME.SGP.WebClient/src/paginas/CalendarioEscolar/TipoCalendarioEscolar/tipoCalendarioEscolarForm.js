import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import RadioGroupButton from '~/componentes/radioGroupButton';

import { CaixaAno, CaixaTextoAno } from './tipoCalendarioEscolar.css';
import history from '~/servicos/history';
import { sucesso, confirmar } from '~/servicos/alertas';
import Auditoria from '~/componentes/auditoria';

const TipoCalendarioEscolarForm = ({ match }) => {
  const usuario = useSelector(store => store.usuario);

  const [refForm, setRefForm] = useState();
  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [anoLetivo, setAnoLetivo] = useState('');
  const [valoresIniciais, setValoresIniciais] = useState({
    situacao: true,
    nome: '',
    modalidade: '',
    periodo: '',
  });

  const [validacoes] = useState(
    Yup.object({
      nome: Yup.string().required('Nome obrigatório'),
      periodo: Yup.string().required('Período obrigatório'),
      modalidade: Yup.string().required('Modalidade obrigatória'),
      situacao: Yup.string().required('Situação obrigatória'),
    })
  );

  const opcoesPeriodo = [
    { label: 'Semestral', value: 1 },
    { label: 'Anual', value: 2 },
  ];

  const opcoesModalidade = [
    { label: 'Fundamental', value: 1 },
    { label: 'Médio', value: 2 },
    { label: 'EJA', value: 3 },
  ];

  const opcoesSituacao = [
    { label: 'Ativo', value: true },
    { label: 'Inativo', value: false },
  ];

  useEffect(() => {
    if (match && match.params && match.params.id) {
      // TODO Chamar endpoint consultar por ID
      // MOCK
      console.log(`Editando ID: ${match.params.id}`);
      setValoresIniciais({
        nome: '2019 - Calendário Escolar Educação Infantil',
        periodo: 1,
        situacao: true,
        modalidade: 1,
      });
      setAnoLetivo('2019');
      setNovoRegistro(false);
      setAuditoria({
        criadoPor: 'ELISANGELA DOS SANTOS ARRUDA',
        criadoRf: '1234567',
        criadoEm: new window.moment(),
        alteradoPor: 'JOÃO DA SILVA',
        alteradoRf: '7654321',
        alteradoEm: new window.moment(),
      });
    } else if (
      usuario.turmaSelecionada &&
      usuario.turmaSelecionada[0] &&
      usuario.turmaSelecionada[0].anoLetivo
    ) {
      setAnoLetivo(usuario.turmaSelecionada[0].anoLetivo);
      console.log(
        `Novo registro, ano letivo: ${usuario.turmaSelecionada[0].anoLetivo}`
      );
    }
  }, []);

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja voltar para tela de listagem agora?'
      );
      if (confirmado) {
        history.push('/calendario-escolar/tipo-calendario-escolar');
      }
    } else {
      history.push('/calendario-escolar/tipo-calendario-escolar');
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
    sucesso('Suas informações foram salvas com sucesso.');
    history.push('/calendario-escolar/tipo-calendario-escolar');
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      const confirmado = await confirmar(
        'Excluir tipo de calendário escolar',
        '',
        'Deseja realmente excluir este calendário?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        // TODO Chamar endpoint de excluir
        sucesso('Tipo de calendário excluído com sucesso.')
        history.push('/calendario-escolar/tipo-calendario-escolar');
      }
    }
  };

  return (
    <>
      <Cabecalho pagina="Cadastro do tipo de calendário escolar" />
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
                  label="Cadastrar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  type="submit"
                />
              </div>
              <div className="row">
                <div className="col-sm-4 col-md-2 col-lg-2 col-xl-1 mb-2">
                  <Label text="Ano" control="ano-letivo" />
                  <CaixaAno>
                    <CaixaTextoAno>{anoLetivo}</CaixaTextoAno>
                  </CaixaAno>
                </div>

                <div className="col-sm-12 col-md-10 col-lg-10 col-xl-8 mb-2">
                  <CampoTexto
                    form={form}
                    label="Nome do calendário"
                    placeholder="Nome do calendário"
                    name="nome"
                    onChange={onChangeCampos}
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    label="Situação"
                    form={form}
                    opcoes={opcoesSituacao}
                    name="situacao"
                    valorInicial
                    onChange={onChangeCampos}
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-4 mb-2">
                  <RadioGroupButton
                    label="Período"
                    form={form}
                    opcoes={opcoesPeriodo}
                    name="periodo"
                    onChange={onChangeCampos}
                  />
                </div>
                <div className="col-sm-12  col-md-12 col-lg-6 col-xl-5 mb-2">
                  <RadioGroupButton
                    label="Modalidade"
                    form={form}
                    opcoes={opcoesModalidade}
                    name="modalidade"
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

export default TipoCalendarioEscolarForm;
