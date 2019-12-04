// Form
import * as Yup from 'yup';
import { Form, Formik } from 'formik';
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
} from '~/componentes';
import history from '~/servicos/history';
import { sucesso, confirmar } from '~/servicos/alertas';
import servicoTipoAvaliaco from '~/servicos/Paginas/TipoAvaliacao'; // Redux
import { useSelector } from 'react-redux';

// Funçoes
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

const TipoAvaliacaoForm = ({ match }) => {
  // const clicouBotaoVoltar = () => {
  //   history.push('/configuracoes/tipo-avaliacao');
  // };
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
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoria, setAuditoria] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    nome: '',
    descricao: '',
    situacao: true,
  });

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
      'Excluir atribuição',
      form.values.professorNome,
      `Deseja realmente excluir este item?`,
      'Excluir',
      'Cancelar'
    );
    if (confirmado) {
      const excluir = await servicoTipoAvaliaco.deletarTipoAvaliacao(
        form.values.id
      );
      if (excluir) {
        sucesso(`Tipo Avaliação excluido com sucesso!`);
        history.push('/configuracoes/tipo-avaliacao');
      }
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
  const onClickVoltar = async () => {
    history.push('/configuracoes/tipo-avaliacao');
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
  const validacoes = () => {
    return Yup.object({
      nome: momentSchema.required('Campo obrigatório'),
      descricao: momentSchema.required('Campo obrigatório'),
      situacao: Yup.number()
        .typeError('Informar um número inteiro')
        .required('Campo obrigatório'),
    });
  };

  const onSubmitFormulario = async valores => {
    try {

      const cadastrado = await  .salvarAtribuicaoEsporadica(
        {
          ...filtroListagem,
          ...valores,
        }
      );
      if (cadastrado && cadastrado.status === 200) {
        dispatch(setLoaderSecao(false));
        sucesso('Tipo de avaliação salvo com sucesso.');
        history.push('/configuracao/tipo-avaliacao');
      }
    } catch (err) {
      if (err) {
        dispatch(setLoaderSecao(false));
        erro(err.response.data.mensagens[0]);
      }
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
          >
          {form => (
            <Form className="col-md-12 mb-4">
              <ButtonGroup
                form={form}
                permissoesTela={permissoesTela[RotasDto.TipoAvaliacaoForm]}
                novoRegistro={novoRegistro}
                labelBotaoPrincipal="Cadastrar"
                onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                onClickCancelar={formulario => onClickCancelar(formulario)}
                onClickVoltar={() => onClickVoltar()}
                onClickExcluir={() => onClickExcluir(form)}
                modoEdicao={modoEdicao}
              />
              <Row>
                <Grid cols={8}>
                  <CampoTexto
                    form={form}
                    label="Nome da Atividade"
                    placeholder="Digite o nome da atividade"
                    name="nome"
                    maxlength={100}
                    placeholder="Digite a descrição da avaliação"
                    type="input"
                    // ref={campoNomeTipoEventoRef}
                    // onChange={aoDigitarDescricao}
                    desabilitado={false}
                  />
                </Grid>
                <Grid cols={2}>
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
              <Label text="Descrição" />
              <TextEditor
                className="form-control"
                //  ref={textEditorRef}
                id="textEditor"
                alt="Descrição"
                //  disabled={disabled}
                //   estadoAdicional={estadoAdicionalEditorTexto}
                // /  onClick={onClickTextEditor}
                //  value={bimestre.objetivo}
                //   onBlur={onBlurTextEditor}
              />
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};
export default TipoAvaliacaoForm;
