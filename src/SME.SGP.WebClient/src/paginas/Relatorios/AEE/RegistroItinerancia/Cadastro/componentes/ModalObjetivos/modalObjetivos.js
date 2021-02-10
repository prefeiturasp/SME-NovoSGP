import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
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
  objetivosSelecionados,
  setObjetivosSelecionados,
  variasUesSelecionadas,
}) => {
  const dispatch = useDispatch();
  const listaObjetivos = useSelector(store => store.itinerancia.objetivos);
  const [modoEdicao, setModoEdicao] = useState(false);

  const esconderModal = () => setModalVisivel(false);

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

  const onChangeCheckbox = item => {
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
      }
    }
    dispatch(setObjetivosItinerancia([...listaObjetivos]));
    setModoEdicao(true);
  };

  const onChangeCampoTexto = (evento, item) => {
    const texto = evento.target?.value;
    const objetivo = listaObjetivos.find(o => o.id === item.id);
    if (objetivo) {
      objetivo.descricao = texto;
    }
    dispatch(setObjetivosItinerancia([...listaObjetivos]));
    setModoEdicao(true);
  };

  useEffect(() => {}, [objetivosSelecionados]);

  return (
    <ModalConteudoHtml
      titulo="Objetivos da itinerância"
      visivel={modalVisivel}
      onClose={fecharModal}
      onConfirmacaoPrincipal={onConfirmarModal}
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
                  onChangeCheckbox={() => onChangeCheckbox(item)}
                  disabled={false}
                  checked={item.checked}
                />
                {item.temDescricao && (
                  <div className="mb-3 pl-3 mr-n3">
                    <CampoTexto
                      height="76"
                      onChange={evento => onChangeCampoTexto(evento, item)}
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
  );
};

ModalObjetivos.defaultProps = {
  modalVisivel: false,
  objetivosSelecionados: [],
  setModalVisivel: () => {},
  setObjetivosSelecionados: () => {},
  variasUesSelecionadas: false,
};

ModalObjetivos.propTypes = {
  modalVisivel: PropTypes.bool,
  objetivosSelecionados: PropTypes.oneOfType([PropTypes.any]),
  setModalVisivel: PropTypes.func,
  setObjetivosSelecionados: PropTypes.func,
  variasUesSelecionadas: PropTypes.bool,
};

export default ModalObjetivos;
