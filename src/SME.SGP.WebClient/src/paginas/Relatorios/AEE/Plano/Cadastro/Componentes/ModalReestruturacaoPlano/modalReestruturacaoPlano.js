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

import { confirmar, erros, sucesso } from '~/servicos';

import { setAlteracaoDados } from '~/redux/modulos/planoAEE/actions';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const ModalReestruturacaoPlano = ({
  key,
  esconderModal,
  exibirModal,
  modoConsulta,
  dadosVisualizacao,
  listaVersao,
  semestre,
  match,
}) => {
  const [modoEdicao, setModoEdicao] = useState(false);
  const [versaoId, setVersaoId] = useState('');
  const [descricao, setDescricao] = useState();
  const [descricaoSimples, setDescricaoSimples] = useState();
  const [reestruturacaoId, setReestruturacaoId] = useState(null);

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
    const resposta = await ServicoPlanoAEE.salvarReestruturacoes({
      planoAEEId: match?.params?.id,
      reestruturacaoId,
      versaoId,
      semestre,
      descricao,
    }).catch(e => erros(e));

    if (resposta?.data) {
      const dadosSalvar = {
        id: resposta?.data,
        data: moment(),
        versao: versaoTexto.descricao,
        descricao,
        descricaoSimples,
        semestre,
        adicionando: !reestruturacaoId,
      };

      sucesso(
        `Reestruturação ${
          reestruturacaoId ? 'alterada' : 'registrada'
        } com sucesso.`
      );

      dispatch(setAlteracaoDados(dadosSalvar));
    }
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
    return texto?.replace(/<.*?>/g, '').replace(/&nbsp;/g, ' ') || '';
  };

  const mudarDescricao = texto => {
    const textoSimples = removerFormatacao(texto);
    setDescricao(texto);
    setDescricaoSimples(textoSimples);
  };

  useEffect(() => {
    const ehVisualizacao =
      modoConsulta || Object.keys(dadosVisualizacao).length;
    const dadosVersaoId = ehVisualizacao ? dadosVisualizacao.versaoId : '';
    const dadosDescricao = ehVisualizacao ? dadosVisualizacao.descricao : '';
    const dadosDescricaoSimples = ehVisualizacao
      ? dadosVisualizacao.descricaoSimples
      : '';
    const dadosReestruturacaoId = ehVisualizacao ? dadosVisualizacao.id : null;

    setVersaoId(String(dadosVersaoId));
    setDescricao(dadosDescricao);
    setDescricaoSimples(dadosDescricaoSimples);
    setReestruturacaoId(dadosReestruturacaoId);
  }, [dadosVisualizacao, modoConsulta]);

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
      paddingRight={modoConsulta ? '40' : '48'}
      colorBotaoSecundario={Colors.Azul}
      esconderBotaoPrincipal={modoConsulta}
      desabilitarBotaoPrincipal={!versaoId || !descricao}
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
          disabled={modoConsulta}
        />
      </div>
      <div className="col-12 mb-3 p-0">
        <Editor
          label="Descreva as mudança que houveram na reestruturação "
          onChange={mudarDescricao}
          inicial={descricao || ''}
          desabilitar={modoConsulta}
          removerToolbar={modoConsulta}
        />
      </div>
    </ModalConteudoHtml>
  );
};

ModalReestruturacaoPlano.defaultProps = {
  esconderModal: () => {},
  exibirModal: false,
  modoConsulta: false,
  dadosVisualizacao: {},
  listaVersao: {},
  match: {},
};

ModalReestruturacaoPlano.propTypes = {
  esconderModal: PropTypes.func,
  exibirModal: PropTypes.bool,
  key: PropTypes.string.isRequired,
  modoConsulta: PropTypes.bool,
  dadosVisualizacao: PropTypes.oneOfType([PropTypes.object]),
  listaVersao: PropTypes.oneOfType([PropTypes.object]),
  semestre: PropTypes.number.isRequired,
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default ModalReestruturacaoPlano;
