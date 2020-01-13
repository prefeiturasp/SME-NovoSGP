import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import RadioGroupButton from '~/componentes/radioGroupButton';
import SelectComponent from '~/componentes/select';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import tipoFeriado from '~/dtos/tipoFeriado';
import { store } from '~/redux';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const TipoFeriadoForm = ({ match }) => {
  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [idTipoFeriadoEdicao, setIdTipoFeriadoEdicao] = useState(0);
  const [isTipoMovel, setIsTipoMovel] = useState(false);

  const { usuario } = store.getState();
  const permissoesTela = usuario.permissoes[RotasDto.TIPO_FERIADO];

  const valoresIniciaisForm = {
    nome: '',
    abrangencia: undefined,
    tipo: 1,
    dataFeriado: '',
    situacao: true,
  };
  const [valoresIniciais, setValoresIniciais] = useState(valoresIniciaisForm);

  const listaDropdownAbrangencia = [
    { id: 1, nome: 'Nacional' },
    { id: 2, nome: 'Estadual' },
    { id: 3, nome: 'Municipal' },
  ];

  const [validacoes] = useState(
    Yup.object({
      nome: Yup.string()
        .required('Nome obrigatório')
        .max(50, 'Máximo 50 caracteres'),
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

  const [possuiEventos, setPossuiEventos] = useState(false);

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);

    const consultaPorId = async () => {
      if (match && match.params && match.params.id) {
        setBreadcrumbManual(
          match.url,
          'Alterar Tipo de Feriado',
          '/calendario-escolar/tipo-feriado'
        );
        setIdTipoFeriadoEdicao(match.params.id);

        const cadastrado = await api
          .get(`v1/calendarios/feriados/${match.params.id}`)
          .catch(e => erros(e));

        if (cadastrado && cadastrado.data) {
          setIsTipoMovel(cadastrado.data.tipo == tipoFeriado.Movel);
          setValoresIniciais({
            abrangencia: String(cadastrado.data.abrangencia),
            nome: cadastrado.data.nome,
            tipo: cadastrado.data.tipo,
            dataFeriado: window.moment(cadastrado.data.dataFeriado),
            situacao: cadastrado.data.ativo,
          });
          setAuditoria({
            criadoPor: cadastrado.data.criadoPor,
            criadoRf: cadastrado.data.criadoRf,
            criadoEm: cadastrado.data.criadoEm,
            alteradoPor: cadastrado.data.alteradoPor,
            alteradoRf: cadastrado.data.alteradoRf,
            alteradoEm: cadastrado.data.alteradoEm,
          });
          setExibirAuditoria(true);
          setPossuiEventos(cadastrado.data.possuiEventos);
        }
        setNovoRegistro(false);
      }
    };

    consultaPorId();
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
    if (novoRegistro && !permissoesTela.podeIncluir) return;
    if (!novoRegistro && !permissoesTela.podeAlterar) return;

    let paramas = valoresForm;
    if (isTipoMovel) {
      paramas = valoresIniciais;
    }
    if (novoRegistro) {
      paramas.tipo = tipoFeriado.Fixo;
    }
    paramas.ativo = valoresForm.situacao;
    paramas.id = idTipoFeriadoEdicao;

    const cadastrado = await api
      .post('v1/calendarios/feriados', paramas)
      .catch(e => erros(e));

    if (cadastrado && cadastrado.status == 200) {
      if (idTipoFeriadoEdicao) {
        sucesso('Tipo de feriado alterado com sucesso.');
      } else {
        sucesso('Novo tipo de feriado criado com sucesso.');
      }
    }

    history.push('/calendario-escolar/tipo-feriado');
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onClickExcluir = async () => {
    if (!permissoesTela.podeExcluir) return;

    if (!novoRegistro) {
      const confirmado = await confirmar(
        'Excluir tipo de feriado',
        '',
        'Deseja realmente excluir este feriado?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const parametrosDelete = { data: [idTipoFeriadoEdicao] };
        const excluir = await api
          .delete('v1/calendarios/feriados', parametrosDelete)
          .catch(e => erros(e));

        if (excluir && excluir.status == 200) {
          sucesso('Tipo de feriado excluído com sucesso.');
          history.push('/calendario-escolar/tipo-feriado');
        }
      }
    }
  };

  const tipoCampoDataFeriado = form => {
    let formato = 'DD/MM/YYYY';
    if (form.values.tipo == tipoFeriado.Fixo) {
      formato = 'DD/MM';
    }
    return (
      <CampoData
        form={form}
        label="Data do feriado"
        placeholder="Data do feriado"
        formatoData={formato}
        name="dataFeriado"
        onChange={onChangeCampos}
        desabilitado={
          isTipoMovel ||
          (novoRegistro && !permissoesTela.podeIncluir) ||
          (!novoRegistro && !permissoesTela.podeAlterar) ||
          possuiEventos
        }
      />
    );
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
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
          idTipoFeriadoEdicao > 0 ? 'Alterar' : 'Cadastro de'
        } Tipo de Feriado`}
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
                    novoRegistro || !permissoesTela.podeExcluir || possuiEventos
                  }
                  onClick={onClickExcluir}
                />
                <Button
                  label={`${idTipoFeriadoEdicao > 0 ? 'Alterar' : 'Cadastrar'}`}
                  color={Colors.Roxo}
                  border
                  bold
                  disabled={
                    (novoRegistro && !permissoesTela.podeIncluir) ||
                    (!novoRegistro && !permissoesTela.podeAlterar)
                  }
                  className="mr-2"
                  onClick={() => validaAntesDoSubmit(form)}
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
                    desabilitado={
                      isTipoMovel ||
                      ((novoRegistro && !permissoesTela.podeIncluir) ||
                        (!novoRegistro && !permissoesTela.podeAlterar)) ||
                      possuiEventos
                    }
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
                    disabled={
                      isTipoMovel ||
                      (novoRegistro && !permissoesTela.podeIncluir) ||
                      (!novoRegistro && !permissoesTela.podeAlterar) ||
                      possuiEventos
                    }
                  />
                </div>

                <div className="col-sm-12 col-md-4 col-lg-3 col-xl-2 mb-2">
                  <RadioGroupButton
                    label="Tipo"
                    form={form}
                    opcoes={opcoesTipo}
                    name="tipo"
                    desabilitado
                  />
                </div>

                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-3">
                  {tipoCampoDataFeriado(form)}
                </div>

                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    label="Situação"
                    form={form}
                    opcoes={opcoesSituacao}
                    name="situacao"
                    valorInicial
                    desabilitado={
                      (novoRegistro && !permissoesTela.podeIncluir) ||
                      (!novoRegistro && !permissoesTela.podeAlterar)
                    }
                    onChange={onChangeCampos}
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
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
          />
        ) : (
          ''
        )}
      </Card>
    </>
  );
};

export default TipoFeriadoForm;
