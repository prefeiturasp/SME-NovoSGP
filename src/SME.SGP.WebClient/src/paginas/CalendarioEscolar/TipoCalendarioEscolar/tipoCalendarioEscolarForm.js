import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import RadioGroupButton from '~/componentes/radioGroupButton';
import RotasDto from '~/dtos/rotasDto';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import SelectComponent from '~/componentes/select';

import { CaixaAno, CaixaTextoAno } from './tipoCalendarioEscolar.css';
import modalidadeTipoCalendario from '~/dtos/modalidadeTipoCalendario';

const TipoCalendarioEscolarForm = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.TIPO_CALENDARIO_ESCOLAR];

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);

  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);

  const anoAtual = window.moment().format('YYYY');

  const [anoLetivo, setAnoLetivo] = useState(anoAtual);
  const [idTipoCalendario, setIdTipoCalendario] = useState(0);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const valoresIniciaisForm = {
    situacao: true,
    nome: '',
    modalidade: '',
    periodo: '',
  };
  const [valoresIniciais, setValoresIniciais] = useState(valoresIniciaisForm);

  const [validacoes] = useState(
    Yup.object({
      nome: Yup.string()
        .required('Nome obrigatório')
        .max(50, 'Máximo 50 caracteres'),
      periodo: Yup.string().required('Período obrigatório'),
      modalidade: Yup.string().required('Modalidade obrigatória'),
      situacao: Yup.string().required('Situação obrigatória'),
    })
  );

  const opcoesPeriodo = [
    { label: 'Anual', value: 1 },
    { label: 'Semestral', value: 2 },
  ];

  const opcoesModalidade = [
    { label: 'Fundamental/Médio', value: modalidadeTipoCalendario.FUNDAMENTAL_MEDIO },
    { label: 'EJA', value: modalidadeTipoCalendario.EJA },
    { label: 'Infantil', value: modalidadeTipoCalendario.Infantil },
  ];

  const opcoesSituacao = [
    { label: 'Ativo', value: true },
    { label: 'Inativo', value: false },
  ];

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setBreadcrumbManual(
        match.url,
        'Alterar Tipo de Calendário Escolar',
        '/calendario-escolar/tipo-calendario-escolar'
      );
      setIdTipoCalendario(match.params.id);
      consultaPorId(match.params.id);
    } else if (usuario.turmaSelecionada && usuario.turmaSelecionada.anoLetivo) {
      setAnoLetivo(usuario.turmaSelecionada.anoLetivo);
    }
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  useEffect(() => {
    const desabilitar = novoRegistro
      ? somenteConsulta || !permissoesTela.podeIncluir
      : somenteConsulta || !permissoesTela.podeAlterar;
    setDesabilitarCampos(desabilitar);
  }, [somenteConsulta, novoRegistro]);

  const [possuiEventos, setPossuiEventos] = useState(false);

  const consultaPorId = async id => {
    const tipoCalendadio = await api
      .get(`v1/calendarios/tipos/${id}`)
      .catch(e => erros(e));

    if (tipoCalendadio && tipoCalendadio.data) {
      setValoresIniciais({
        nome: tipoCalendadio.data.nome,
        periodo: tipoCalendadio.data.periodo,
        situacao: tipoCalendadio.data.situacao,
        modalidade: String(tipoCalendadio.data.modalidade),
      });
      setAnoLetivo(tipoCalendadio.data.anoLetivo);
      setAuditoria({
        criadoPor: tipoCalendadio.data.criadoPor,
        criadoRf:
          tipoCalendadio.data.criadoRF > 0 ? tipoCalendadio.data.criadoRF : '',
        criadoEm: tipoCalendadio.data.criadoEm,
        alteradoPor: tipoCalendadio.data.alteradoPor,
        alteradoRf:
          tipoCalendadio.data.alteradoRF > 0
            ? tipoCalendadio.data.alteradoRF
            : '',
        alteradoEm: tipoCalendadio.data.alteradoEm,
      });
      setNovoRegistro(false);
      setExibirAuditoria(true);
      setPossuiEventos(tipoCalendadio.data.possuiEventos);
    }
  };

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

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        resetarTela(form);
      }
    }
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
  };

  const onClickCadastrar = async valoresForm => {
    valoresForm.id = idTipoCalendario || 0;
    valoresForm.anoLetivo = anoLetivo;
    var metodo = idTipoCalendario ? 'put' : 'post';
    var url = 'v1/calendarios/tipos';
    if (idTipoCalendario)
      url += '/' + idTipoCalendario;

    const cadastrado = await api[metodo]
      (url, valoresForm)
      .catch(e => erros(e));
    if (cadastrado) {
      sucesso('Suas informações foram salvas com sucesso.');
      history.push('/calendario-escolar/tipo-calendario-escolar');
    }
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
        const parametrosDelete = { data: [idTipoCalendario] };
        const excluir = await api
          .delete('v1/calendarios/tipos', parametrosDelete)
          .catch(e => erros(e));
        if (excluir) {
          sucesso('Tipo de calendário excluído com sucesso.');
          history.push('/calendario-escolar/tipo-calendario-escolar');
        }
      }
    }
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciaisForm);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length == 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  return (
    <>
      <Cabecalho
        pagina={`${
          idTipoCalendario > 0 ? 'Alterar' : 'Cadastro do'
          } Tipo de Calendário Escolar`}
      />
      <Card>
        <Formik
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
                  onClick={() => onClickCancelar(form)}
                  disabled={!modoEdicao}
                />
                <Button
                  label="Excluir"
                  color={Colors.Vermelho}
                  border
                  className="mr-2"
                  disabled={
                    somenteConsulta ||
                    !permissoesTela.podeExcluir ||
                    novoRegistro ||
                    possuiEventos
                  }
                  onClick={onClickExcluir}
                />
                <Button
                  label={idTipoCalendario > 0 ? 'Alterar' : 'Cadastrar'}
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={() => validaAntesDoSubmit(form)}
                  disabled={desabilitarCampos}
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
                    desabilitado={desabilitarCampos}
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
                    desabilitado={desabilitarCampos}
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-4 mb-2">
                  <RadioGroupButton
                    label="Período"
                    form={form}
                    opcoes={opcoesPeriodo}
                    name="periodo"
                    onChange={onChangeCampos}
                    desabilitado={desabilitarCampos || possuiEventos}
                  />
                </div>
                <div className="col-sm-12  col-md-12 col-lg-6 col-xl-5 mb-2">
                  <SelectComponent
                    id="modalidade"
                    name="modalidade"
                    lista={opcoesModalidade}
                    label="Modalidade"
                    valueOption="value"
                    valueText="label"
                    placeholder="Selecione uma modalidade"
                    form={form}
                    onChange={onChangeCampos}
                    disabled={desabilitarCampos || possuiEventos}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
        {exibirAuditoria ? (
          <Auditoria
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            criadoRf={auditoria.criadoRf}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
            alteradoRf={auditoria.alteradoRf}
          />
        ) : (
            ''
          )}
      </Card>
    </>
  );
};

export default TipoCalendarioEscolarForm;
