import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import { setLoaderSecao } from '~/redux/modulos/loader/actions';

// Serviços
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import { erros, erro, sucesso, confirmar } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

// Componentes SGP
import { Cabecalho, DreDropDown, UeDropDown } from '~/componentes-sgp';

// Componentes
import {
  Card,
  ButtonGroup,
  Grid,
  Localizador,
  Loader,
  Auditoria,
} from '~/componentes';
import ModalidadesDropDown from './componentes/ModalidadesDropDown';
import TurmasDropDown from './componentes/TurmasDropDown';
import Tabela from './componentes/Tabela';

// Styles
import { Row } from './styles';

// Funçoes
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

function AtribuicaoCJForm({ match }) {
  const dispatch = useDispatch();
  const carregando = useSelector(store => store.loader.loaderSecao);
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const filtroListagem = useSelector(
    store => store.atribuicaoEsporadica.filtro
  );
  const [dreId, setDreId] = useState('');
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoria, setAuditoria] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    professorRf: '',
    professorNome: '',
    dataInicio: '',
    dataFim: '',
  });

  const validacoes = () => {
    return Yup.object({});
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
      dispatch(setLoaderSecao(true));
      const cadastrado = await AtribuicaoEsporadicaServico.salvarAtribuicaoEsporadica(
        {
          ...filtroListagem,
          ...valores,
        }
      );
      if (cadastrado && cadastrado.status === 200) {
        dispatch(setLoaderSecao(false));
        sucesso('Atribuição esporádica salva com sucesso.');
        history.push('/gestao/atribuicao-esporadica');
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
      dispatch(setLoaderSecao(true));
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
        dispatch(setLoaderSecao(false));
      }
    } catch (err) {
      dispatch(setLoaderSecao(false));
      erros(err);
    }
  };

  const validaFormulario = valores => {
    if (validaSeObjetoEhNuloOuVazio(valores)) return;
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setBreadcrumbManual(match.url, 'Atribuição', '/gestao/atribuicao-cjs');
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
                  <Grid cols={6}>
                    <DreDropDown
                      label="Diretoria Regional de Educação (DRE)"
                      form={form}
                      onChange={valor => setDreId(valor)}
                    />
                  </Grid>
                  <Grid cols={6}>
                    <UeDropDown
                      label="Unidade Escolar (UE)"
                      dreId={dreId}
                      form={form}
                      onChange={() => null}
                    />
                  </Grid>
                </Row>
                <Row className="row">
                  <Grid cols={7}>
                    <Row className="row">
                      <Localizador
                        dreId={form.values.dreId}
                        anoLetivo="2019"
                        showLabel
                        form={form}
                        onChange={() => null}
                      />
                    </Row>
                  </Grid>
                  <Grid cols={3}>
                    <ModalidadesDropDown
                      label="Modalidade"
                      form={form}
                      onChange={value => value}
                    />
                  </Grid>
                  <Grid cols={2}>
                    <TurmasDropDown
                      label="Turma"
                      form={form}
                      onChange={value => value}
                    />
                  </Grid>
                </Row>
              </Form>
            )}
          </Formik>
          <Tabela />
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

AtribuicaoCJForm.propTypes = {
  match: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
};

AtribuicaoCJForm.defaultProps = {
  match: {},
};

export default AtribuicaoCJForm;
