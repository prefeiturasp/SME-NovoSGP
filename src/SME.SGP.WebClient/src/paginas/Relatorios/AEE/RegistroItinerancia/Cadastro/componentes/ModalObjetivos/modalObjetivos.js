import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import * as Yup from 'yup';
import {
  CampoTexto,
  CheckboxComponent,
  ModalConteudoHtml,
} from '~/componentes';
import { setObjetivosItinerancia } from '~/redux/modulos/itinerancia/action';
import { aviso, confirmar } from '~/servicos';
import { TituloEstilizado } from './modalObjetivos.css';

const ModalObjetivos = ({
  modalVisivel,
  setModalVisivel,
  setObjetivosSelecionados,
  listaObjetivos,
  objetivosSelecionados,
  variasUesSelecionadas,
}) => {
  const dispatch = useDispatch();
  const NOME_CAMPO_TEXTO = 'campo-texto-';
  const [modoEdicao, setModoEdicao] = useState(false);
  const [valoresIniciais, setValoresIniciais] = useState({});
  const [validacoes, setValidacoes] = useState({});
  const [refForm, setRefForm] = useState({});

  const esconderModal = () => setModalVisivel(false);

  useEffect(() => {
    const valores = {};
    if (listaObjetivos?.length) {
      listaObjetivos.forEach(objetivo => {
        if (objetivo.temDescricao) {
          valores[NOME_CAMPO_TEXTO + objetivo.id] = '';
          setValoresIniciais(valores);
        }
        const objetivoSelecionado = objetivosSelecionados?.find(
          o => o.id === objetivo.id
        );
        if (objetivoSelecionado) {
          objetivo.checked = objetivoSelecionado.checked;
          objetivo.descricao = objetivoSelecionado.descricao;
        } else {
          objetivo.checked = false;
        }
      });
    }
  }, [listaObjetivos]);

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    const arraySelecionados = listaObjetivos.filter(item => item.checked);
    setObjetivosSelecionados(arraySelecionados);
    dispatch(setObjetivosItinerancia([...listaObjetivos]));
    setModoEdicao(false);
    esconderModal();
  };

  const fecharModal = async () => {
    esconderModal();
    if (modoEdicao) {
      const ehPraSalvar = await perguntarSalvarListaUsuario();
      if (ehPraSalvar) {
        onConfirmarModal();
      }
    }
  };

  const onChangeCheckbox = (item, form) => {
    form.resetForm();
    const objetivo = listaObjetivos.find(o => o.id === item.id);
    if (objetivo) {
      if (
        !objetivo.permiteVariasUes &&
        variasUesSelecionadas &&
        !objetivo.checked
      ) {
        aviso(
          'Este objetivo só pode ser selecionado quando o registro é de uma unidade apenas e você está' +
            ' com mais de uma unidade selecionada.'
        );
      } else {
        objetivo.checked = !objetivo.checked;
        setModoEdicao(true);
      }
    }
    const validacoesCamposComDescricao = {};
    listaObjetivos.forEach(objetivoItem => {
      if (objetivoItem.temDescricao && objetivoItem.checked) {
        validacoesCamposComDescricao[
          NOME_CAMPO_TEXTO + objetivoItem.id
        ] = Yup.string().required('Campo obrigatório');
      } else {
        objetivo.descricao = '';
      }
    });
    setValidacoes(
      Object.keys(validacoesCamposComDescricao).length
        ? Yup.object(validacoesCamposComDescricao)
        : Yup.object({})
    );
  };

  const onChangeCampoTexto = (evento, item) => {
    const texto = evento.target?.value;
    const objetivo = listaObjetivos.find(o => o.id === item.id);
    if (objetivo) {
      objetivo.descricao = texto;
    }
    setModoEdicao(true);
  };

  const validaAntesDoSubmit = form => {
    if (Object.keys(valoresIniciais).length) {
      const arrayCampos = Object.keys(valoresIniciais);
      arrayCampos.forEach(campo => {
        form.setFieldTouched(campo, true, true);
      });
      form.validateForm().then(() => {
        if (form.isValid || Object.keys(form.errors).length === 0) {
          form.submitForm(form);
        }
      });
    }
  };

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes}
      onSubmit={valores => onConfirmarModal(valores)}
      validateOnBlur
      validateOnChange
      ref={refFormik => setRefForm(refFormik)}
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <ModalConteudoHtml
            titulo="Objetivos da itinerância"
            visivel={modalVisivel}
            onClose={fecharModal}
            onConfirmacaoPrincipal={() => validaAntesDoSubmit(form)}
            onConfirmacaoSecundaria={fecharModal}
            labelBotaoPrincipal="Adicionar objetivos"
            labelBotaoSecundario="Cancelar"
            closable
            width="50%"
            fecharAoClicarFora
            fecharAoClicarEsc
          >
            <div className="col-md-12 mt-n2">
              <div className="row mb-3">
                <TituloEstilizado>Selecione os objetivos</TituloEstilizado>
              </div>
              {listaObjetivos?.length &&
                listaObjetivos.map(item => {
                  const textoUe = item.permiteVariasUes
                    ? '(uma ou mais unidades)'
                    : '(apenas uma unidade)';

                  return (
                    <React.Fragment key={item.id}>
                      <CheckboxComponent
                        key={item.id}
                        className="mb-3 ml-n2"
                        label={`${item.nome} ${textoUe}`}
                        name={`objetivo-${item.id}`}
                        onChangeCheckbox={() => onChangeCheckbox(item, form)}
                        disabled={false}
                        checked={item.checked}
                      />
                      {item.temDescricao && (
                        <div className="mb-3 pl-3 mr-n3">
                          <CampoTexto
                            name={NOME_CAMPO_TEXTO + item.id}
                            height="76"
                            onChange={evento =>
                              onChangeCampoTexto(evento, item)
                            }
                            form={form}
                            type="textarea"
                            value={item.descricao}
                            desabilitado={!item.checked}
                          />
                        </div>
                      )}
                    </React.Fragment>
                  );
                })}
            </div>
          </ModalConteudoHtml>
        </Form>
      )}
    </Formik>
  );
};

ModalObjetivos.defaultProps = {
  modalVisivel: false,
  setModalVisivel: () => {},
  setObjetivosSelecionados: () => {},
  variasUesSelecionadas: false,
  listaObjetivos: [],
  objetivosSelecionados: [],
};

ModalObjetivos.propTypes = {
  modalVisivel: PropTypes.bool,
  setModalVisivel: PropTypes.func,
  setObjetivosSelecionados: PropTypes.func,
  variasUesSelecionadas: PropTypes.bool,
  listaObjetivos: PropTypes.oneOfType([PropTypes.array]),
  objetivosSelecionados: PropTypes.oneOfType([PropTypes.array]),
};

export default ModalObjetivos;
