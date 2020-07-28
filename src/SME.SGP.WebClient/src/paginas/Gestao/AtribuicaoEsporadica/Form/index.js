import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import _ from 'lodash';

// Form
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

// Redux
import { useSelector, useDispatch } from 'react-redux';

// Serviços
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';
import { erros, erro, sucesso, confirmar } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

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
  Loader,
  Auditoria,
} from '~/componentes';
import DreDropDown from '../componentes/DreDropDown';
import UeDropDown from '../componentes/UeDropDown';
import AnoLetivoTag from '../componentes/AnoLetivoTag';

// Styles
import { Row } from './styles';

// Funçoes
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

function AtribuicaoEsporadicaForm({ match }) {
  const dispatch = useDispatch();
  const [carregando, setCarregando] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const somenteConsulta = verificaSomenteConsulta(
    permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]
  );
  const filtroListagem = useSelector(
    store => store.atribuicaoEsporadica.filtro
  );
  const [dreId, setDreId] = useState('');
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoria, setAuditoria] = useState({});
  const [valoresCarregados, setValoresCarregados] = useState(null);
  const [refForm, setRefForm] = useState({});

  const anoAtual = window.moment().format('YYYY');
  const [ehInfantil, setEhInfantil] = useState(false);

  const [valoresIniciais, setValoresIniciais] = useState({
    professorRf: '',
    professorNome: '',
    dataInicio: window.moment(),
    dataFim: '',
    ueId: '',
    dreId: '',
    anoLetivo: anoAtual,
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
      setCarregando(true);
      const cadastrado = await AtribuicaoEsporadicaServico.salvarAtribuicaoEsporadica(
        {
          ...filtroListagem,
          ...valores,
          ehInfantil,
        }
      );
      if (cadastrado && cadastrado.status === 200) {
        setCarregando(false);
        sucesso('Atribuição esporádica salva com sucesso.');
        history.push('/gestao/atribuicao-esporadica');
      }
    } catch (err) {
      if (err) {
        setCarregando(false);
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
        history.push('/gestao/atribuicao-esporadica');
      }
    } else {
      history.push('/gestao/atribuicao-esporadica');
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
      'Excluir atribuição',
      form.values.professorNome,
      `Deseja realmente excluir este item?`,
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      const excluir = await AtribuicaoEsporadicaServico.deletarAtribuicaoEsporadica(
        form.values.id
      );
      if (excluir) {
        sucesso(`Atribuição excluida com sucesso!`);
        history.push('/gestao/atribuicao-esporadica');
      }
    }
  };

  const buscarPorId = async id => {
    try {
      setCarregando(true);
      const registro = await AtribuicaoEsporadicaServico.buscarAtribuicaoEsporadica(
        id
      );
      if (registro && registro.data) {
        setValoresIniciais({
          ...registro.data,
          dataInicio: window.moment(registro.data.dataInicio),
          dataFim: window.moment(registro.data.dataFim),
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
        setCarregando(false);
      }
    } catch (err) {
      setCarregando(false);
      erros(err);
    }
  };

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
                  permissoesTela={
                    permissoesTela[RotasDto.ATRIBUICAO_ESPORADICA_LISTA]
                  }
                  novoRegistro={novoRegistro}
                  labelBotaoPrincipal="Cadastrar"
                  onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                  onClickCancelar={formulario => onClickCancelar(formulario)}
                  onClickVoltar={() => onClickVoltar()}
                  onClickExcluir={() => onClickExcluir(form)}
                  modoEdicao={modoEdicao}
                />
                <Row className="row">
                  <Grid cols={2}>
                    <AnoLetivoTag label="Ano Letivo" />
                  </Grid>
                  <Grid cols={5}>
                    <DreDropDown
                      label="Diretoria Regional de Educação (DRE)"
                      form={form}
                      onChange={valor => setDreId(valor)}
                      desabilitado={somenteConsulta}
                    />
                  </Grid>
                  <Grid cols={5}>
                    <UeDropDown
                      label="Unidade Escolar (UE)"
                      dreId={dreId}
                      form={form}
                      onChange={(v, infantil) => {
                        setEhInfantil(infantil);
                      }}
                      desabilitado={somenteConsulta}
                    />
                  </Grid>
                </Row>
                <Row className="row">
                  <Grid cols={8}>
                    <Row className="row">
                      <Localizador
                        dreId={form.values.dreId}
                        anoLetivo={form.values.anoLetivo}
                        showLabel
                        form={form}
                        onChange={() => null}
                        desabilitado={somenteConsulta || valoresIniciais.id}
                        incluirEmei
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
                      desabilitado={somenteConsulta}
                    />
                  </Grid>
                  <Grid cols={2}>
                    <CampoData
                      placeholder="Selecione"
                      label="Data Fim"
                      form={form}
                      name="dataFim"
                      formatoData="DD/MM/YYYY"
                      desabilitado={somenteConsulta}
                    />
                  </Grid>
                </Row>
              </Form>
            )}
          </Formik>
          {auditoria && (
            <Auditoria
              criadoEm={auditoria.criadoEm}
              criadoPor={auditoria.criadoPor}
              criadoRf={auditoria.criadoRf}
              alteradoPor={auditoria.alteradoPor}
              alteradoEm={auditoria.alteradoEm}
              alteradoRf={auditoria.alteradoRf}
            />
          )}
        </Card>
      </Loader>
    </>
  );
}

AtribuicaoEsporadicaForm.propTypes = {
  match: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
};

AtribuicaoEsporadicaForm.defaultProps = {
  match: {},
};

export default AtribuicaoEsporadicaForm;
