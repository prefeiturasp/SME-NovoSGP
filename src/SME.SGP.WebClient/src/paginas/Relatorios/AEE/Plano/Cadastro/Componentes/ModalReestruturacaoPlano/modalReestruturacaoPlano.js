import moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import {
  Colors,
  Editor,
  ModalConteudoHtml,
  SelectComponent,
} from '~/componentes';
import {
  setAlteracaoDados,
  setAtualizarDados,
  setDadosModalReestruturacao,
} from '~/redux/modulos/planoAEE/actions';
import { confirmar, erros, sucesso } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';

const ModalReestruturacaoPlano = ({
  key,
  alternarModal,
  exibirModal,
  modoConsulta,
  dadosVisualizacao,
  semestre,
  match,
}) => {
  const [modoEdicao, setModoEdicao] = useState(false);
  const [versao, setVersao] = useState('');
  const [versaoId, setVersaoId] = useState();
  const [descricao, setDescricao] = useState();
  const [descricaoSimples, setDescricaoSimples] = useState();
  const [reestruturacaoId, setReestruturacaoId] = useState(0);
  const [listaVersao, setListaVersao] = useState([]);
  const ehVisualizacao =
    modoConsulta || !!Object.keys(dadosVisualizacao).length;

  const dispatch = useDispatch();
  const dadosModalReestruturacao = useSelector(
    store => store.planoAEE.dadosModalReestruturacao
  );

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas. Deseja realmente cancelar as alterações?'
    );
    return resposta;
  };

  const onConfirmarModal = async () => {
    const IdReestruturacao = reestruturacaoId || '';
    const resposta = await ServicoPlanoAEE.salvarReestruturacoes({
      planoAEEId: match?.params?.id,
      reestruturacaoId: IdReestruturacao,
      versaoId,
      semestre,
      descricao,
    }).catch(e => erros(e));

    if (resposta?.data) {
      const dadosSalvar = {
        id: resposta?.data,
        data: moment(),
        versao,
        versaoId,
        descricao,
        descricaoSimples,
        semestre,
        adicionando: !reestruturacaoId,
      };

      const palavarMsg = reestruturacaoId ? 'alterada' : 'registrada';
      sucesso(`Reestruturação ${palavarMsg} com sucesso.`);
      dispatch(setAtualizarDados(true));
      dispatch(setAlteracaoDados(dadosSalvar));
    }
    setModoEdicao(false);
    alternarModal();

    setVersao('');
    setVersaoId();
    setDescricao();
    setDescricaoSimples();
    setReestruturacaoId(0);
    setListaVersao([]);
    dispatch(setDadosModalReestruturacao({}));
  };

  const mudarVersao = useCallback(
    valor => {
      const acharVersao = listaVersao.find(
        item => String(item.id) === String(valor)
      );
      setModoEdicao(true);
      setVersaoId(valor);
      setVersao(acharVersao?.descricao);
      dispatch(
        setDadosModalReestruturacao({
          ...dadosModalReestruturacao,
          versaoId: valor,
        })
      );
    },
    [dispatch, dadosModalReestruturacao, listaVersao]
  );

  const removerFormatacao = texto => {
    return texto?.replace(/&nbsp;|<.*?>/g, '') || '';
  };

  const mudarDescricao = useCallback(
    texto => {
      const textoAlterado = ehVisualizacao
        ? texto.localeCompare(dadosVisualizacao.descricao)
        : false;
      if ((texto && !ehVisualizacao) || textoAlterado) {
        setModoEdicao(true);
      }
      const textoSimples = removerFormatacao(texto);

      setDescricao(texto);
      setDescricaoSimples(textoSimples);
      dispatch(
        setDadosModalReestruturacao({
          ...dadosModalReestruturacao,
          descricao: texto,
        })
      );
    },
    [
      dispatch,
      dadosModalReestruturacao,
      dadosVisualizacao.descricao,
      ehVisualizacao,
    ]
  );

  const fecharModal = async () => {
    alternarModal();
    if (modoEdicao) {
      const ehPraCancelar = await perguntarSalvarListaUsuario();
      if (ehPraCancelar) {
        setModoEdicao(false);
        dispatch(setDadosModalReestruturacao({}));
        return;
      }
      alternarModal(true);
    }
  };

  useEffect(() => {
    const dadosVersaoId = ehVisualizacao ? dadosVisualizacao.versaoId : '';
    const dadosVersao = ehVisualizacao ? dadosVisualizacao.versao : '';
    const dadosDescricao = ehVisualizacao ? dadosVisualizacao.descricao : '';
    const dadosDescricaoSimples = ehVisualizacao
      ? dadosVisualizacao.descricaoSimples
      : '';
    const dadosReestruturacaoId = ehVisualizacao ? dadosVisualizacao.id : 0;

    setVersaoId(String(dadosVersaoId));
    setVersao(dadosVersao);
    setDescricao(dadosDescricao);
    setDescricaoSimples(dadosDescricaoSimples);
    setReestruturacaoId(dadosReestruturacaoId);
  }, [dadosVisualizacao, ehVisualizacao]);

  const obterVersoes = useCallback(async () => {
    const resposta = await ServicoPlanoAEE.obterVersoes(
      match?.params?.id,
      reestruturacaoId
    );
    if (resposta?.data) {
      setListaVersao(resposta.data);
    }
  }, [match, reestruturacaoId]);

  useEffect(() => {
    if (
      (!reestruturacaoId && !ehVisualizacao) ||
      (reestruturacaoId && ehVisualizacao)
    ) {
      obterVersoes();
    }
  }, [obterVersoes, reestruturacaoId, ehVisualizacao]);

  useEffect(() => {
    if (!versaoId && dadosModalReestruturacao?.versaoId) {
      mudarVersao(dadosModalReestruturacao?.versaoId);
    }
    if (!descricao && dadosModalReestruturacao?.descricao) {
      mudarDescricao(dadosModalReestruturacao.descricao);
    }
  }, [
    dadosModalReestruturacao,
    descricao,
    versaoId,
    mudarVersao,
    mudarDescricao,
  ]);

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
  alternarModal: () => {},
  exibirModal: false,
  modoConsulta: false,
  dadosVisualizacao: {},
  match: {},
};

ModalReestruturacaoPlano.propTypes = {
  alternarModal: PropTypes.func,
  exibirModal: PropTypes.bool,
  key: PropTypes.string.isRequired,
  modoConsulta: PropTypes.bool,
  dadosVisualizacao: PropTypes.oneOfType([PropTypes.object]),
  semestre: PropTypes.number.isRequired,
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default ModalReestruturacaoPlano;
