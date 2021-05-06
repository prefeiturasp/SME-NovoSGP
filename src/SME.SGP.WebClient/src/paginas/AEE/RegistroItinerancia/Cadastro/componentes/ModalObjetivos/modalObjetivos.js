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
import { clonarObjeto } from '~/utils';
import {
  NOME_CAMPO_OBJETIVO,
  NOME_CHECK_OBJETIVO,
} from '../ConstantesCamposDinâmicos';
import { TituloEstilizado } from './modalObjetivos.css';

const ModalObjetivos = ({
  modalVisivel,
  setModalVisivel,
  setObjetivosSelecionados,
  listaObjetivos,
  objetivosSelecionados,
  variasUesSelecionadas,
  setModoEdicaoItinerancia,
}) => {
  const dispatch = useDispatch();
  const [modoEdicao, setModoEdicao] = useState(false);
  const [valoresIniciais, setValoresIniciais] = useState({});
  const [validacoes, setValidacoes] = useState({});

  const esconderModal = () => setModalVisivel(false);

  useEffect(() => {
    const valores = {};
    if (listaObjetivos?.length) {
      const validacoesCamposComDescricao = {};
      listaObjetivos.forEach(objetivo => {
        if (objetivo.temDescricao) {
          valores[NOME_CAMPO_OBJETIVO + objetivo.itineranciaObjetivoBaseId] =
            '';
          setValoresIniciais(valores);
          validacoesCamposComDescricao[
            NOME_CAMPO_OBJETIVO + objetivo.itineranciaObjetivoBaseId
          ] = Yup.string().test(
            `validaQuestao-${objetivo.itineranciaObjetivoBaseId}`,
            'Campo obrigatório',
            function validar() {
              if (objetivo.checked && !objetivo.descricao) {
                return false;
              }
              return true;
            }
          );
        }
        const objetivoSelecionado = objetivosSelecionados?.find(
          o =>
            o.itineranciaObjetivoBaseId === objetivo.itineranciaObjetivoBaseId
        );
        if (objetivoSelecionado) {
          objetivo.checked = objetivoSelecionado.checked;
          objetivo.descricao = objetivoSelecionado.descricao;
        } else {
          objetivo.checked = false;
          objetivo.descricao = '';
        }
      });
      setValidacoes(Yup.object(validacoesCamposComDescricao));
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
    setObjetivosSelecionados(clonarObjeto(arraySelecionados));
    dispatch(setObjetivosItinerancia([...listaObjetivos]));
    setModoEdicao(false);
    setModoEdicaoItinerancia(true);
    esconderModal();
  };

  const validaCamposForm = async form => {
    if (Object.keys(valoresIniciais).length) {
      const arrayCampos = Object.keys(valoresIniciais);
      arrayCampos.forEach(campo => {
        form.setFieldTouched(campo, true, true);
      });
      await form.validateForm();
      return form.isValid || Object.keys(form.errors).length === 0;
    }
  };

  const fecharModal = async form => {
    const validacao = await validaCamposForm(form);
    if (validacao) {
      esconderModal();
      if (modoEdicao) {
        const ehPraSalvar = await perguntarSalvarListaUsuario();
        if (ehPraSalvar) {
          onConfirmarModal();
        }
      }
    }
  };

  const onChangeCheckbox = (item, form) => {
    const objetivo = listaObjetivos.find(
      o => o.itineranciaObjetivoBaseId === item.itineranciaObjetivoBaseId
    );
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
        form.setFieldValue(
          NOME_CHECK_OBJETIVO + objetivo.itineranciaObjetivoBaseId,
          !objetivo.checked
        );
        if (!objetivo.checked) {
          objetivo.descricao = '';
          form.setFieldValue(
            NOME_CAMPO_OBJETIVO + objetivo.itineranciaObjetivoBaseId,
            ''
          );
        }
        setModoEdicao(true);
      }
    }
  };

  const onChangeCampoTexto = (evento, item) => {
    const texto = evento.target?.value;
    const objetivo = listaObjetivos.find(
      o => o.itineranciaObjetivoBaseId === item.itineranciaObjetivoBaseId
    );
    if (objetivo) {
      objetivo.descricao = texto;
    }
    setModoEdicao(true);
  };

  const validaAntesDoSubmit = async form => {
    if (await validaCamposForm(form)) {
      form.submitForm(form);
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
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <ModalConteudoHtml
            titulo="Objetivos da itinerância"
            visivel={modalVisivel}
            onClose={() => fecharModal(form)}
            onConfirmacaoPrincipal={() => validaAntesDoSubmit(form)}
            onConfirmacaoSecundaria={() => fecharModal(form)}
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
              {listaObjetivos?.length > 0 &&
                listaObjetivos.map(item => {
                  const textoUe = item.permiteVariasUes
                    ? '(uma ou mais unidades)'
                    : '(apenas uma unidade)';

                  return (
                    <React.Fragment key={item.itineranciaObjetivoBaseId}>
                      <CheckboxComponent
                        key={item.itineranciaObjetivoBaseId}
                        className="mb-3 ml-n2"
                        label={`${item.nome} ${textoUe}`}
                        name={
                          NOME_CHECK_OBJETIVO + item.itineranciaObjetivoBaseId
                        }
                        onChangeCheckbox={() => onChangeCheckbox(item, form)}
                        disabled={false}
                        checked={item.checked}
                      />
                      {item.temDescricao && (
                        <div className="mb-3 pl-3 mr-n3">
                          <CampoTexto
                            name={
                              NOME_CAMPO_OBJETIVO +
                              item.itineranciaObjetivoBaseId
                            }
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
  setModoEdicaoItinerancia: () => {},
};

ModalObjetivos.propTypes = {
  modalVisivel: PropTypes.bool,
  setModalVisivel: PropTypes.func,
  setObjetivosSelecionados: PropTypes.func,
  variasUesSelecionadas: PropTypes.bool,
  listaObjetivos: PropTypes.oneOfType([PropTypes.array]),
  objetivosSelecionados: PropTypes.oneOfType([PropTypes.array]),
  setModoEdicaoItinerancia: PropTypes.func,
};

export default ModalObjetivos;
