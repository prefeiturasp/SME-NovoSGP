import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import _ from 'lodash';

// Form
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import { setLoaderSecao } from '~/redux/modulos/loader/actions';

// Serviços
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import RegistroPOAServico from '~/servicos/Paginas/DiarioClasse/RegistroPOA';
import { erros, erro, sucesso, confirmar } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

// Componentes SGP
import { Cabecalho, DreDropDown, UeDropDown } from '~/componentes-sgp';

// Componentes
import {
  Card,
  ButtonGroup,
  Grid,
  CampoTexto,
  Localizador,
  Loader,
  Auditoria,
} from '~/componentes';
import MesesDropDown from '../componentes/MesesDropDown';

// Styles
import { Row } from './styles';

// Funçoes
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';

function RegistroPOAForm({ match }) {
  const dispatch = useDispatch();
  const carregando = useSelector(store => store.loader.loaderSecao);
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const anoLetivo =
    useSelector(store => store.usuario.turmaSelecionada.anoLetivo) ||
    window.moment().format('YYYY');

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoria, setAuditoria] = useState({});
  const [valoresCarregados, setValoresCarregados] = useState(null);
  const [refForm, setRefForm] = useState({});
  const ehEdicaoRegistro = match && match.params && match.params.id > 0;
  const [valoresIniciais, setValoresIniciais] = useState({
    bimestre: '',
    titulo: '',
    descricao: '',
    professorRf: '',
    professorNome: '',
    dreId: '',
    ueId: '',
  });

  useEffect(() => {
    const permissoes = permissoesTela[RotasDto.REGISTRO_POA];
    const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setSomenteConsulta(
      verificaSomenteConsulta(permissoes, naoSetarSomenteConsultaNoStore)
    );
    if (naoSetarSomenteConsultaNoStore && refForm.resetForm) {
      refForm.resetForm();
      setModoEdicao(false);
    }
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  const validacoes = () => {
    return Yup.object({
      descricao: Yup.string().required('Campo obrigatório!'),
      bimestre: Yup.number().required('Campo obrigatório!'),
      titulo: Yup.string().required('O campo "Título" é obrigatório!'),
      professorRf: Yup.number()
        .typeError('Informar um número inteiro!')
        .required('Campo obrigatório!'),
    });
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });

    if (!form.values.descricao.length) {
      erro('O campo "Descrição" é obrigatório!');
      return;
    }

    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.submitForm(form);
      }
    });
  };

  const onClickBotaoPrincipal = form => {
    const formComEditor = {
      ...form,
      values: {
        ...form.values,
        anoLetivo,
      },
    };
    validaAntesDoSubmit(formComEditor);
  };

  const onSubmitFormulario = async valores => {
    try {
      dispatch(setLoaderSecao(true));
      const cadastrado = await RegistroPOAServico.salvarRegistroPOA(
        {
          ...valores,
          codigoRf: valores.professorRf,
          nome: valores.professorNome,
          anoLetivo,
        },
        valores.id || null
      );
      if (cadastrado && cadastrado.status === 200) {
        dispatch(setLoaderSecao(false));
        sucesso('Registro salvo com sucesso.');
        history.push('/diario-classe/registro-poa');
      }
    } catch (err) {
      if (err) {
        dispatch(setLoaderSecao(false));
        erro(err.response.data.mensagens[0]);
      }
    }
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        history.push('/diario-classe/registro-poa');
      }
    } else {
      history.push('/diario-classe/registro-poa');
    }
  };

  const onClickCancelar = async form => {
    if (!modoEdicao) return;
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );
    if (confirmou) {
      form.resetForm();
      setModoEdicao(false);
    }
  };

  const onClickExcluir = async form => {
    if (validaSeObjetoEhNuloOuVazio(form.values)) return;

    const confirmado = await confirmar(
      'Excluir registro',
      form.values.titulo,
      `Deseja realmente excluir este item?`,
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      const excluir = await RegistroPOAServico.deletarRegistroPOA(
        form.values.id
      );
      if (excluir) {
        sucesso(`Registro excluido com sucesso!`);
        history.push('/diario-classe/registro-poa');
      }
    }
  };

  const buscarPorId = useCallback(async id => {
    try {
      const registro = await RegistroPOAServico.buscarRegistroPOA(id);
      if (registro && registro.data) {
        setValoresIniciais({
          ...registro.data,
          bimestre: String(registro.data.bimestre),
          professorRf: registro.data.codigoRf,
          professorNome: registro.data.nome,
          titulo: registro.data.titulo,
        });
        setAuditoria({
          criadoPor: registro.data.criadoPor,
          criadoRf: registro.data.criadoRF > 0 ? registro.data.criadoRF : '',
          criadoEm: registro.data.criadoEm,
          alteradoPor: registro.data.alteradoPor,
          alteradoRf:
            registro.data.alteradoRF > 0 ? registro.data.alteradoRF : '',
          alteradoEm: registro.data.alteradoEm,
        });
        setValoresCarregados(true);
      }
    } catch (err) {
      erros(err);
    }
  }, []);

  const validaFormulario = valores => {
    if (validaSeObjetoEhNuloOuVazio(valores)) return;
    if (
      (!modoEdicao &&
        valoresCarregados &&
        !_.isEqual(
          refForm.getFormikContext().initialValues,
          refForm.getFormikContext().values
        )) ||
      (!modoEdicao &&
        novoRegistro &&
        !_.isEqual(
          refForm.getFormikContext().initialValues,
          refForm.getFormikContext().values
        ))
    ) {
      setModoEdicao(true);
    }
  };

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setBreadcrumbManual(match.url, 'Registro', '/diario-classe/registro-poa');
      buscarPorId(match.params.id);
    }
  }, [buscarPorId, match]);

  return (
    <>
      <AlertaModalidadeInfantil />
      <Cabecalho pagina="Registro" />
      <Loader loading={carregando}>
        <Card>
          <Formik
            enableReinitialize
            initialValues={valoresIniciais}
            validationSchema={validacoes}
            onSubmit={valores => onSubmitFormulario(valores)}
            validate={valores => validaFormulario(valores)}
            ref={refFormik => setRefForm(refFormik)}
            validateOnBlur
            validateOnChange
          >
            {form => (
              <Form>
                <ButtonGroup
                  form={form}
                  permissoesTela={permissoesTela[RotasDto.REGISTRO_POA]}
                  novoRegistro={novoRegistro}
                  labelBotaoPrincipal={
                    ehEdicaoRegistro ? 'Alterar' : 'Cadastrar'
                  }
                  onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                  onClickCancelar={formulario => onClickCancelar(formulario)}
                  onClickVoltar={() => onClickVoltar()}
                  onClickExcluir={() => onClickExcluir(form)}
                  modoEdicao={modoEdicao}
                  desabilitarBotaoPrincipal={ehTurmaInfantil(
                    modalidadesFiltroPrincipal,
                    turmaSelecionada
                  )}
                />
                {!ehTurmaInfantil(
                  modalidadesFiltroPrincipal,
                  turmaSelecionada
                ) ? (
                  <>
                    <Row className="row mb-2">
                      <Grid cols={6}>
                        <DreDropDown
                          url="v1/dres/atribuicoes"
                          label="Diretoria Regional de Educação (DRE)"
                          form={form}
                          onChange={() => null}
                          desabilitado={somenteConsulta}
                        />
                      </Grid>
                      <Grid cols={6}>
                        <UeDropDown
                          dreId={form.values.dreId}
                          label="Unidade Escolar (UE)"
                          form={form}
                          url="v1/dres"
                          onChange={() => null}
                          desabilitado={somenteConsulta}
                        />
                      </Grid>
                    </Row>
                    <Row className="row mb-2">
                      <Localizador
                        dreId={form.values.dreId}
                        anoLetivo={anoLetivo}
                        form={form}
                        onChange={() => null}
                        showLabel
                        desabilitado={somenteConsulta}
                      />
                    </Row>
                    <Row className="row">
                      <Grid cols={2}>
                        <MesesDropDown
                          label="Bimestre"
                          name="bimestre"
                          form={form}
                          desabilitado={somenteConsulta}
                        />
                      </Grid>
                      <Grid cols={10}>
                        <CampoTexto
                          name="titulo"
                          id="titulo"
                          label="Título"
                          placeholder="Digite o título do registro"
                          form={form}
                          desabilitado={somenteConsulta}
                        />
                      </Grid>
                    </Row>
                    <Row className="row">
                      <Grid cols={12}>
                        <JoditEditor
                          label="Registro das atividades realizadas junto aos professores ao longo do bimestre, considerando a análise e o acompanhamento do planejamento docente"
                          form={form}
                          id="descricao"
                          alt="Registro das atividades realizadas junto aos professores ao longo do bimestre, considerando a análise e o acompanhamento do planejamento docente"
                          name="descricao"
                          value={valoresIniciais.descricao}
                          desabilitado={somenteConsulta}
                        />
                      </Grid>
                    </Row>
                  </>
                ) : (
                  ''
                )}
              </Form>
            )}
          </Formik>
          {auditoria &&
          !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
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
      </Loader>
    </>
  );
}

RegistroPOAForm.propTypes = {
  match: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
};

RegistroPOAForm.defaultProps = {
  match: {},
};

export default RegistroPOAForm;
