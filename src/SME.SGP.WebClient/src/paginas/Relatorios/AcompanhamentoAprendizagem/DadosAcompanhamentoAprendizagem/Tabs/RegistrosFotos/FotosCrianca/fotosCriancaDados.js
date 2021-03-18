import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import UploadImagens from '~/componentes-sgp/UploadImagens/uploadImagens';
import { erros } from '~/servicos';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';

const FotosCriancaDados = props => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const obterImagens = async dados => {
    setExibirLoader(true);

    let listaImagens = [];
    const resposta = await ServicoAcompanhamentoAprendizagem.obterFotos(
      dados?.id
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

  const removerImagem = () => {
    // TODO
    // const resposta = await ServicoAcompanhamentoAprendizagem.obterFotos(
    //   dados?.id
    // )
  }

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
    ],
    obterImagens,
    removerImagem,
  };

  return <UploadImagens {...configUploadImagens} />;
};

FotosCriancaDados.propTypes = {
  semestreSelecionado: PropTypes.string,
};

FotosCriancaDados.defaultProps = {
  semestreSelecionado: '',
};

export default FotosCriancaDados;
