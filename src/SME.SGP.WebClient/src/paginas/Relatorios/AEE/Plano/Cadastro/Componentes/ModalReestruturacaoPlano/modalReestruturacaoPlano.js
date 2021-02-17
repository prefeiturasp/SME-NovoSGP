import React, { useEffect, useState } from 'react';
import moment from 'moment';
import PropTypes from 'prop-types';
import shortid from 'shortid';

import {
  Colors,
  Editor,
  ModalConteudoHtml,
  SelectComponent,
} from '~/componentes';

import { confirmar } from '~/servicos';

const ModalReestruturacaoPlano = ({
  key,
  esconderModal,
  exibirModal,
  setListaDados,
  modoVisualizacao,
  dadosVisualizacao,
}) => {
  const [modoEdicao, setModoEdicao] = useState(false);
  const [listaVersao, setListaVersao] = useState([
    { id: 1, desc: 'v6 - 19/02/2020', valor: 'v6 - 19/02/2020' },
    { id: 2, desc: 'v7 - 19/02/2020', valor: 'v7 - 19/02/2020' },
    { id: 3, desc: 'v8 - 19/02/2020', valor: 'v8 - 19/02/2020' },
  ]);
  const [versaoSelecionada, setVersaoSelecionada] = useState();
  const [descricao, setDescricao] = useState();

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    const dadosSalvar = {
      key: shortid.generate(),
      data: moment().format('DD/MM/YYYY'),
      versaoPlano: versaoSelecionada,
      descricaoReestruturacao: descricao,
    };

    setListaDados(estadoAntigo => [...estadoAntigo, dadosSalvar]);
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

  const mudarVersao = valor => {
    setModoEdicao(true);
    setVersaoSelecionada(valor);
  };

  const removerFormatacao = texto => {
    return texto?.replace(/<.*?>/g, '') || '';
  };

  const mudarDescricao = texto => {
    const textoSimples = removerFormatacao(texto);
    setDescricao({ textoSimples, texto });
  };

  useEffect(() => {
    if (modoVisualizacao && dadosVisualizacao) {
      setVersaoSelecionada(dadosVisualizacao.versaoPlano);
      setDescricao(dadosVisualizacao.descricaoReestruturacao);
    }
  }, [dadosVisualizacao, modoVisualizacao]);

  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key={key}
      visivel={exibirModal}
      titulo="Reestruturação do plano"
      onClose={fecharModal}
      onConfirmacaoPrincipal={onConfirmarModal}
      onConfirmacaoSecundaria={fecharModal}
      labelBotaoPrincipal="Salvar"
      labelBotaoSecundario="Voltar"
      width="50%"
      closable
      paddingBottom="24"
      paddingRight={modoVisualizacao ? '40' : '48'}
      colorBotaoSecundario={Colors.Azul}
      esconderBotaoPrincipal={modoVisualizacao}
    >
      <div className="col-12 mb-3 p-0">
        <SelectComponent
          label="Selecione o plano que corresponde a reestruturação"
          lista={listaVersao}
          valueOption="valor"
          valueText="desc"
          name="diaSemana"
          onChange={mudarVersao}
          valueSelect={versaoSelecionada}
          disabled={modoVisualizacao}
        />
      </div>
      <div className="col-12 mb-3 p-0">
        <Editor
          label="Descreva as mudança que houveram na reestruturação "
          onChange={mudarDescricao}
          inicial={descricao?.texto || ''}
          desabilitar={modoVisualizacao}
          removerToolbar={modoVisualizacao}
        />
      </div>
    </ModalConteudoHtml>
  );
};

ModalReestruturacaoPlano.defaultProps = {
  esconderModal: () => {},
  exibirModal: false,
  setListaDados: () => {},
  modoVisualizacao: false,
  dadosVisualizacao: {},
};

ModalReestruturacaoPlano.propTypes = {
  esconderModal: PropTypes.func,
  exibirModal: PropTypes.bool,
  key: PropTypes.string.isRequired,
  setListaDados: PropTypes.func,
  modoVisualizacao: PropTypes.bool,
  dadosVisualizacao: PropTypes.oneOfType([PropTypes.object]),
};

export default ModalReestruturacaoPlano;
