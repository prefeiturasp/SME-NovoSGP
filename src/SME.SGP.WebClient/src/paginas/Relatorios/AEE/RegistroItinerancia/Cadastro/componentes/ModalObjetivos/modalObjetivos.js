import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';

import {
  CampoTexto,
  CheckboxComponent,
  ModalConteudoHtml,
} from '~/componentes';

import { aviso, confirmar, erros } from '~/servicos';
import ServicoRegistroItineranciaAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoRegistroItineranciaAEE';

import { TituloEstilizado } from './modalObjetivos.css';

const ModalObjetivos = ({
  modalVisivel,
  setModalVisivel,
  objetivosSelecionados,
  setObjetivosSelecionados,
  variasUesSelecionadas,
}) => {
  const [listaObjetivos, setListaObjetivos] = useState();
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
    setListaObjetivos(estadoAntigo =>
      estadoAntigo.map(objetivo => {
        if (!objetivo.permiteVariasUes && variasUesSelecionadas) {
          aviso(
            'Este objetivo só pode ser selecionado quando o registro é de uma unidade apenas e você está' +
              ' com mais de uma unidade selecionada.'
          );
          return objetivo;
        }

        if (objetivo.id === item.id) {
          return {
            ...objetivo,
            checked: !objetivo.checked,
          };
        }
        return objetivo;
      })
    );
    setModoEdicao(true);
  };

  const onChangeCampoTexto = (evento, item) => {
    const texto = evento.target?.value;
    setListaObjetivos(estadoAntigo =>
      estadoAntigo.map(objetivo => {
        if (objetivo.id === item.id) {
          return {
            ...objetivo,
            detalhamentoTexto: texto,
          };
        }
        return objetivo;
      })
    );
    setModoEdicao(true);
  };

  useEffect(() => {
    if (Object.keys(objetivosSelecionados).length) {
      objetivosSelecionados.map(objetivo =>
        setListaObjetivos(estadoAntigo =>
          estadoAntigo.map(estado => {
            if (estado.id === objetivo.id) {
              return objetivo;
            }
            return estado;
          })
        )
      );
    }
  }, [objetivosSelecionados]);

  const obterObjetivos = async () => {
    const retorno = await ServicoRegistroItineranciaAEE.obterObjetivos().catch(
      e => erros(e)
    );
    if (retorno?.data) {
      const dadosAlterados = retorno.data.map(item => ({
        ...item,
        key: item.id,
      }));
      setListaObjetivos(dadosAlterados);
    }
  };

  useEffect(() => {
    obterObjetivos();
  }, []);

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
        {listaObjetivos &&
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
                      value={item.detalhamentoTexto}
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
