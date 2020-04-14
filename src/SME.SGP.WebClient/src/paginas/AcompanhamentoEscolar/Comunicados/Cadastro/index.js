import React, { useState, useCallback, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';

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
  TextEditor,
  momentSchema,
  Base,
} from '~/componentes';

import { Linha } from '~/componentes/EstilosGlobais';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';
import { confirmar, erro, sucesso } from '~/servicos/alertas';

const ComunicadosCadastro = ({ match }) => {
  const ErroValidacao = styled.span`
    color: ${Base.Vermelho};
  `;

  const InseridoAlterado = styled.div`
    color: ${Base.CinzaMako};
    font-weight: bold;
    font-style: normal;
    font-size: 10px;
    object-fit: contain;
    p {
      margin: 0;
    }
  `;

  const [loaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  useCallback(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao] = useState(false);

  const [idComunicado, setIdComunicado] = useState();

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setIdComunicado(match.params.id);
    }
  }, [match]);

  const [valoresIniciais, setValoresIniciais] = useState({
    id: 0,
    gruposId: [],
    dataEnvio: '',
    dataExpiracao: '',
    titulo: '',
    descricao: '',
  });

  const [descricaoComunicado, setDescricaoComunicado] = useState('');

  const [inseridoAlterado, setInseridoAlterado] = useState({
    alteradoEm: '',
    alteradoPor: '',
    alteradoRF: '',
    criadoEm: '',
    criadoPor: '',
    criadoRF: '',
  });

  useEffect(() => {
    async function obterPorId(id) {
      const comunicado = await ServicoComunicados.consultarPorId(id);
      if (comunicado && Object.entries(comunicado).length) {
        setValoresIniciais({
          id: comunicado.id,
          gruposId: [...comunicado.grupos.map(grupo => String(grupo.id))],
          dataEnvio: comunicado.dataEnvio
            ? window.moment(comunicado.dataEnvio)
            : '',
          dataExpiracao: comunicado.dataExpiracao
            ? window.moment(comunicado.dataExpiracao)
            : '',
          titulo: comunicado.titulo,
          descricao: comunicado.descricao,
        });

        setDescricaoComunicado(comunicado.descricao);

        setInseridoAlterado({
          alteradoEm: comunicado.alteradoEm,
          alteradoPor: comunicado.alteradoPor,
          alteradoRF: comunicado.alteradoRF,
          criadoEm: comunicado.criadoEm,
          criadoPor: comunicado.criadoPor,
          criadoRF: comunicado.criadoRF,
        });
      }
    }

    if (idComunicado) {
      obterPorId(idComunicado);
      setInseridoAlterado({});
      setDescricaoComunicado('');
      setNovoRegistro(false);
    }
  }, [idComunicado]);

  const [refForm, setRefForm] = useState({});
  const textEditorRef = useRef();

  const [gruposLista, setGruposLista] = useState([]);

  useEffect(() => {
    async function obterListaGrupos() {
      const lista = await ServicoComunicados.listarGrupos();
      setGruposLista(lista);
    }
    obterListaGrupos();
  }, []);

  const [validacoes] = useState(
    Yup.object({
      gruposId: Yup.string().required('Campo obrigatório'),
      dataEnvio: momentSchema.required('Campo obrigatório'),
      dataExpiracao: momentSchema.test(
        'validaDataMaiorQueEnvio',
        'Data de expiração deve ser maior que a data de envio',
        function validar() {
          const { dataEnvio } = this.parent;
          const { dataExpiracao } = this.parent;

          if (
            dataEnvio &&
            dataExpiracao &&
            window.moment(dataExpiracao) < window.moment(dataEnvio)
          ) {
            return false;
          }

          return true;
        }
      ),
      titulo: Yup.string()
        .required('Campo obrigatório')
        .min(10, 'Deve conter no mínimo 10 caracteres')
        .max(50, 'Deve conter no máximo 50 caracteres'),
    })
  );

  const [descricaoValida, setDescricaoValida] = useState(true);

  const validarAntesDeSalvar = form => {
    const arrayCampos = Object.keys(valoresIniciais);

    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });

    const descricao = textEditorRef.current.state.value.replace(
      '<p><br></p>',
      ''
    );

    form.validateForm().then(() => {
      setDescricaoValida(descricao.length);

      if (
        refForm &&
        (!Object.entries(refForm.state.errors).length || form.isValid) &&
        descricao.length
      ) {
        setDescricaoComunicado(descricao);
        form.handleSubmit(form);
      }
    });
  };

  const onClickExcluir = async () => {
    if (idComunicado) {
      const confirmado = await confirmar(
        'Atenção',
        'Você tem certeza que deseja excluir este registro?'
      );
      if (confirmado) {
        const exclusao = await ServicoComunicados.excluir([idComunicado]);
        if (exclusao && exclusao.status === 200) {
          history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
          sucesso('Registro excluído com sucesso');
        } else {
          erro(exclusao);
        }
      }
    }
  };

  const onClickVoltar = () => {
    history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
  };

  const onClickBotaoPrincipal = form => {
    validarAntesDeSalvar(form);
  };

  const onClickSalvar = async valores => {
    const dadosSalvar = {
      ...valores,
      descricao: textEditorRef.current.state.value,
    };
    const salvou = await ServicoComunicados.salvar(dadosSalvar);
    if (salvou && salvou.data) {
      history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
      sucesso('Registro salvo com sucesso');
    } else {
      erro(salvou);
    }
  };

  const onChangeGruposId = gruposId => {
    setValoresIniciais({ ...refForm.state.values, gruposId });
  };

  return (
    <>
      <Cabecalho pagina="Comunicação com pais ou responsáveis" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <Formik
            enableReinitialize
            initialValues={valoresIniciais}
            validationSchema={validacoes}
            ref={refFormik => setRefForm(refFormik)}
            onSubmit={valores => onClickSalvar(valores)}
            validateOnBlur
            validateOnChange
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <ButtonGroup
                  form={form}
                  novoRegistro={novoRegistro}
                  modoEdicao={modoEdicao}
                  somenteConsulta={somenteConsulta}
                  permissoesTela={
                    permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]
                  }
                  onClickExcluir={onClickExcluir}
                  onClickVoltar={onClickVoltar}
                  onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                  labelBotaoPrincipal={idComunicado ? 'Salvar' : 'Cadastrar'}
                />
                <Linha className="row mb-2">
                  <Grid cols={4}>
                    <Label control="gruposId" text="Grupo" />
                    <SelectComponent
                      form={form}
                      name="gruposId"
                      placeholder="Selecione um grupo"
                      valueSelect={form.values.gruposId}
                      multiple
                      onChange={onChangeGruposId}
                      lista={gruposLista}
                      valueOption="id"
                      valueText="nome"
                      disabled={somenteConsulta}
                    />
                  </Grid>
                  <Grid cols={4}>
                    <Label control="dataEnvio" text="Data de envio" />
                    <CampoData
                      form={form}
                      name="dataEnvio"
                      placeholder="Data início"
                      formatoData="DD/MM/YYYY"
                      disabled={somenteConsulta}
                    />
                  </Grid>
                  <Grid cols={4}>
                    <Label control="dataExpiracao" text="Data de expiração" />
                    <CampoData
                      form={form}
                      name="dataExpiracao"
                      placeholder="Data início"
                      formatoData="DD/MM/YYYY"
                      disabled={somenteConsulta}
                    />
                  </Grid>
                </Linha>
                <Linha className="row">
                  <Grid cols={12}>
                    <Label control="titulo" text="Tíutulo" />
                    <CampoTexto
                      form={form}
                      name="titulo"
                      placeholder="Título do comunicado"
                      value={form.values.titulo}
                      disabled={somenteConsulta}
                    />
                  </Grid>
                </Linha>
                <Linha className="row">
                  <Grid cols={12}>
                    <Label control="textEditor" text="Descrição" />
                    <TextEditor
                      ref={textEditorRef}
                      id="textEditor"
                      height="120px"
                      maxHeight="calc(100vh)"
                      className={`${!descricaoValida && 'is-invalid'}`}
                      value={descricaoComunicado}
                      disabled={somenteConsulta}
                    />
                    {!descricaoValida && (
                      <ErroValidacao>Campo obrigatório</ErroValidacao>
                    )}
                    <InseridoAlterado>
                      {inseridoAlterado &&
                      inseridoAlterado.criadoPor &&
                      inseridoAlterado.criadoPor.length ? (
                        <p className="pt-2">
                          INSERIDO por {inseridoAlterado.criadoPor} (
                          {inseridoAlterado.criadoRF}) em{' '}
                          {window
                            .moment(inseridoAlterado.criadoEm)
                            .format('DD/MM/YYYY HH:mm:ss')}
                        </p>
                      ) : (
                        ''
                      )}
                      {inseridoAlterado &&
                      inseridoAlterado.alteradoPor &&
                      inseridoAlterado.alteradoPor.length ? (
                        <p>
                          ALTERADO por {inseridoAlterado.alteradoPor} (
                          {inseridoAlterado.alteradoRF}) em{' '}
                          {window
                            .moment(inseridoAlterado.alteradoEm)
                            .format('DD/MM/YYYY HH:mm:ss')}
                        </p>
                      ) : (
                        ''
                      )}
                    </InseridoAlterado>
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
