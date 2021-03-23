import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import UploadImagens from '~/componentes-sgp/UploadImagens/uploadImagens';
import { setDadosAcompanhamentoAprendizagem } from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import { erros, sucesso } from '~/servicos';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';

const FotosCriancaDados = props => {
  const dispatch = useDispatch();

  const dadosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
  );

  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const desabilitarCamposAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .desabilitarCamposAcompanhamentoAprendizagem
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [exibirLoader, setExibirLoader] = useState(false);
  const [listaInicialImagens, setListaInicialImagens] = useState([]);

  const obterImagens = async acompanhamentoAlunoSemestreId => {
    setExibirLoader(true);

    let listaImagens = [];
    const resposta = await ServicoAcompanhamentoAprendizagem.obterFotos(
      acompanhamentoAlunoSemestreId
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (resposta?.data) {
      const novoMap = resposta.data.map(item => {
        return {
          uid: item.codigo,
          fileBase64: item?.download?.item1,
          type: item?.download?.item2,
          name: item?.download?.item3,
        };
      });
      listaImagens = novoMap;
    }

    return listaImagens;
  };

  const obterListaInicialImagens = () => {
    obterImagens(
      dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId
    ).then(imagens => {
      setListaInicialImagens(imagens);
    });
  };

  const atualizarDados = dados => {
    const dadosAcompanhamentoAtual = dadosAcompanhamentoAprendizagem;
    dadosAcompanhamentoAtual.auditoria = dados;
    dispatch(setDadosAcompanhamentoAprendizagem(dadosAcompanhamentoAtual));
    obterListaInicialImagens();
  };

  const removerImagem = async codigoFoto => {
    if (
      codigoFoto &&
      dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId
    ) {
      setExibirLoader(true);
      const resposta = await ServicoAcompanhamentoAprendizagem.excluirFotos(
        dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId,
        codigoFoto
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data) {
        sucesso('Imagem excluída com sucesso');
        atualizarDados(resposta.data);
        return true;
      }
    }
    return false;
  };

  const afterSuccessUpload = dados => {
    if (!dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId) {
      ServicoAcompanhamentoAprendizagem.obterAcompanhamentoEstudante(
        turmaSelecionada?.id,
        codigoEOL,
        semestreSelecionado
      );
    } else {
      atualizarDados(dados);
    }
  };

  const configUploadImagens = {
    servicoCustomRequest: ServicoAcompanhamentoAprendizagem.uploadFoto,
    parametrosCustomRequest: [
      {
        nome: 'turmaId',
        valor: turmaSelecionada.id,
      },
      {
        nome: 'semestre',
        valor: semestreSelecionado,
      },
      {
        nome: 'alunoCodigo',
        valor: codigoEOL,
      },
      {
        nome: 'acompanhamentoAlunoSemestreId',
        valor: dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId,
      },
      {
        nome: 'acompanhamentoAlunoId',
        valor: dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoId,
      },
    ],
    afterSuccessUpload,
    removerImagem,
    listaInicialImagens,
    desabilitar: desabilitarCamposAcompanhamentoAprendizagem,
    quantidadeMaxima: dadosAcompanhamentoAprendizagem?.quantidadeFotos,
  };

  useEffect(() => {
    if (dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId) {
      obterListaInicialImagens();
    }
  }, [dadosAcompanhamentoAprendizagem]);

  return (
    <Loader loading={exibirLoader}>
      <UploadImagens {...configUploadImagens} />
    </Loader>
  );
};

FotosCriancaDados.propTypes = {
  semestreSelecionado: PropTypes.string,
};

FotosCriancaDados.defaultProps = {
  semestreSelecionado: '',
};

export default FotosCriancaDados;
