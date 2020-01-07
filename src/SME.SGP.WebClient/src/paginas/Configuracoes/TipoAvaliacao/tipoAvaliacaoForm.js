import * as Yup from 'yup';
import PropTypes from 'prop-types';
import { Form, Formik } from 'formik';
import { useSelector } from 'react-redux';
import React, { useState, useRef, useEffect } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import TextEditor from '~/componentes/textEditor';
import RadioGroupButton from '~/componentes/radioGroupButton';
import { Row } from './styles';

import {
  CampoTexto,
  Label,
  ButtonGroup,
  Card,
  Grid,
  momentSchema,
  Auditoria,
} from '~/componentes';
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import { erros, sucesso, confirmar, erro } from '~/servicos/alertas';
import servicoTipoAvaliaco from '~/servicos/Paginas/TipoAvaliacao'; // Redux
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';

import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

const TipoAvaliacaoForm = ({ match }) => {
  const listaSituacao = [
    {
      label: 'Ativo',
      value: true,
    },
    {
      label: 'Inativo',
      value: false,
    },
  ];
  const [descricao, setDescricao] = useState('');
  const [podeCancelar, setPodeCancelar] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoria, setAuditoria] = useState({});
  const [valoresCarregados, setValoresCarregados] = useState(null);
  const [valoresIniciais, setValoresIniciais] = useState({
    nome: '',
    descricao: '',
    situacao: true,
    possuiAvaliacao: false,
  });

  const [idTipoAvaliacao, setIdTipoAvaliacao] = useState('');

  const textEditorRef = useRef(null);

  const aoTrocarTextEditor = valor => {
    setDescricao(valor);
    setPodeCancelar(true);
  };

  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const validaFormulario = valores => {
    if (validaSeObjetoEhNuloOuVazio(valores)) return;
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };
  const onClickExcluir = async form => {
    if (validaSeObjetoEhNuloOuVazio(form.values)) return;
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir este registro?'
    );
    if (confirmado) {
      const excluir = await servicoTipoAvaliaco.deletarTipoAvaliacao([
        form.values.id,
      ]);
      if (excluir && excluir.status === 200) {
        sucesso(`Tipo Avaliação excluido com sucesso!`);
        history.push('/configuracoes/tipo-avaliacao');
      } else {
        erro(excluir);
      }
    }
  };

  const onClickCancelar = async form => {
    // if (!modoEdicao) return;
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );
    if (confirmou) {
      form.resetForm();
      setDescricao(form.values.descricao);
      setModoEdicao(false);
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
        history.push('/configuracoes/tipo-avaliacao');
      }
    } else {
      history.push('/configuracoes/tipo-avaliacao');
    }
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
    form.values.descricao = textEditorRef.current.state.value;
    validaAntesDoSubmit(form);
  };
  const validacoes = () => {
    return Yup.object({
      nome: momentSchema.required('Nome obrigatório'),
      // descricao: momentSchema.required('Descrição obrigatório'),
      situacao: Yup.bool()
        .typeError('Informar um booleano')
        .required('Campo obrigatório'),
    });
  };

  const onSubmitFormulario = async valores => {
    try {
      let cadastrado;
      if (!idTipoAvaliacao) {
        cadastrado = await servicoTipoAvaliaco.salvarTipoAvaliacao({
          ...valores,
        });
      } else {
        const tipoAvalicao = {
          nome: valores.nome,
          descricao: valores.descricao,
          situacao: valores.situacao,
        };
        cadastrado = await servicoTipoAvaliaco.atualizaTipoAvaliacao(
          idTipoAvaliacao,
          tipoAvalicao
        );
      }

      if (cadastrado && cadastrado.status === 200) {
        sucesso('Tipo de avaliação salvo com sucesso.');
        history.push('/configuracoes/tipo-avaliacao');
      }
    } catch (err) {
      if (err) {
        erro(err.response.data.mensagens[0]);
      }
    }
  };

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setBreadcrumbManual(
        match.url,
        'Atribuição',
        '/configuracoes/tipo-avaliacao'
      );
      buscarPorId(match.params.id);
    }
  }, []);

  const buscarPorId = async id => {
    try {
      const registro = await servicoTipoAvaliaco.buscarTipoAvaliacaoPorId(id);
      if (registro && registro.data) {
        setValoresIniciais({
          ...registro.data,
        });

        setDescricao(registro.data.descricao);
        setIdTipoAvaliacao(match.params.id);
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
        setPodeCancelar(false);
        //  dispatch(setLoaderSecao(false));
      }
    } catch (err) {
      erros(err);
    }
  };

  return (
    <>
      <Cabecalho pagina="Tipo de Avaliação" />
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
            <Form className="col-md-12 mb-4">
              <ButtonGroup
                form={form}
                permissoesTela={permissoesTela[RotasDto.TIPO_AVALIACAO]}
                novoRegistro={novoRegistro}
                labelBotaoPrincipal={idTipoAvaliacao ? 'Alterar' : 'Cadastrar'}
                onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                onClickCancelar={formulario => onClickCancelar(formulario)}
                onClickVoltar={() => onClickVoltar()}
                onClickExcluir={() => onClickExcluir(form)}
                modoEdicao={modoEdicao || podeCancelar}
              />
              <Row className="row">
                <Grid cols={8}>
                  <CampoTexto
                    form={form}
                    label="Nome da Atividade"
                    name="nome"
                    maxlength={100}
                    placeholder="Digite a descrição da avaliação"
                    type="input"
                    desabilitado={valoresIniciais.possuiAvaliacao}
                  />
                </Grid>
                <Grid cols={4}>
                  <RadioGroupButton
                    desabilitado={false}
                    id="situacao"
                    name="situacao"
                    label="Situação"
                    form={form}
                    opcoes={listaSituacao}
                    // onChange={e => {
                    //   setEhReposicao(e.target.value === 2);
                    //   setValoresIniciais({
                    //     ...valoresIniciais,
                    //     tipoAula: e.target.value,
                    //     recorrenciaAula: e.target.value === 2 ? 1 : '',
                    //   });
                    //   onChangeCampos();
                    //   montaValidacoes(0, e.target.value, form);
                    // }}
                  />
                </Grid>
              </Row>
              <Row className="row">
                <Grid cols={12}>
                  <Label text="Descrição" />
                  <TextEditor
                    className="form-control w-100"
                    ref={textEditorRef}
                    id="descricao"
                    alt="Descrição"
                    onBlur={aoTrocarTextEditor}
                    value={descricao}
                    maxlength={500}
                    toolbar={false}
                    disabled={valoresIniciais.possuiAvaliacao}
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
    </>
  );
};

TipoAvaliacaoForm.propTypes = {
  match: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
};

TipoAvaliacaoForm.defaultProps = {
  match: {},
};
export default TipoAvaliacaoForm;
