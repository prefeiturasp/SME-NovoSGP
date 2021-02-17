import React, { useEffect, useState } from 'react';
import moment from 'moment';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { useDispatch } from 'react-redux';

import {
  Colors,
  Editor,
  ModalConteudoHtml,
  SelectComponent,
} from '~/componentes';

import { confirmar } from '~/servicos';

import { setReestruturacaoDados } from '~/redux/modulos/planoAEE/actions';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const ModalReestruturacaoPlano = ({
  key,
  esconderModal,
  exibirModal,
  modoVisualizacao,
  dadosVisualizacao,
  listaVersao,
  semestre,
  match,
}) => {
  const [modoEdicao, setModoEdicao] = useState(false);
  const [versaoId, setVersaoId] = useState();
  const [descricao, setDescricao] = useState();
  const [descricaoSimples, setDescricaoSimples] = useState();

  const dispatch = useDispatch();

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };
  const onConfirmarModal = async () => {
    const versaoTexto = listaVersao.find(
      item => String(item.id) === String(versaoId)
    );
    const dadosSalvar = {
      id: shortid.generate(),
      data: moment(),
      versao: versaoTexto.descricao,
      descricao,
      descricaoSimples,
      semestre,
    };

    const params = {
      planoAEEId: match?.params?.id,
      versaoId,
      semestre,
      descricao,
    };

    await ServicoPlanoAEE.salvarReestruturacoes(params);

    dispatch(setReestruturacaoDados(dadosSalvar));
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
    setVersaoId(valor);
  };

  const removerFormatacao = texto => {
    return texto?.replace(/<.*?>/g, '') || '';
  };

  const mudarDescricao = texto => {
    const textoSimples = removerFormatacao(texto);
    setDescricao(texto);
    setDescricaoSimples(textoSimples);
  };

  useEffect(() => {
    const ehVisualizacao = modoVisualizacao && dadosVisualizacao;
    const dadosVersao = ehVisualizacao ? dadosVisualizacao.versao : '';
    const dadosDescricao = ehVisualizacao ? dadosVisualizacao.descricao : '';
    const dadosDescricaoSimples = ehVisualizacao
      ? dadosVisualizacao.descricaoSimples
      : '';
    setVersaoId(dadosVersao);
    setDescricao(dadosDescricao);
    setDescricaoSimples(dadosDescricaoSimples);
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
          valueOption="id"
          valueText="descricao"
          name="versao"
          onChange={mudarVersao}
          valueSelect={versaoId}
          disabled={modoVisualizacao}
        />
      </div>
      <div className="col-12 mb-3 p-0">
        <Editor
          label="Descreva as mudança que houveram na reestruturação "
          onChange={mudarDescricao}
          inicial={descricao || ''}
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
  modoVisualizacao: false,
  dadosVisualizacao: {},
  listaVersao: {},
  match: {},
};

ModalReestruturacaoPlano.propTypes = {
  esconderModal: PropTypes.func,
  exibirModal: PropTypes.bool,
  key: PropTypes.string.isRequired,
  modoVisualizacao: PropTypes.bool,
  dadosVisualizacao: PropTypes.oneOfType([PropTypes.object]),
  listaVersao: PropTypes.oneOfType([PropTypes.object]),
  semestre: PropTypes.number.isRequired,
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default ModalReestruturacaoPlano;
