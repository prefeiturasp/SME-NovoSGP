import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import UploadImagens from '~/componentes-sgp/UploadImagens/uploadImagens';
import { erros, sucesso } from '~/servicos';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';

const FotosCriancaDados = props => {
  const dadosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
  );

  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [exibirLoader, setExibirLoader] = useState(false);
  const [listaInicialImagens, setListaInicialImagens] = useState([]);

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

  const obterImagensEstudante = () => {
    obterImagens({
      id: dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId,
    }).then(imagens => {
      setListaInicialImagens(imagens);
    });
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
        obterImagensEstudante();
        return true;
      }
    }
    return false;
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
    obterImagens,
    removerImagem,
    listaInicialImagens,
  };

  useEffect(() => {
    if (dadosAcompanhamentoAprendizagem?.acompanhamentoAlunoSemestreId) {
      obterImagensEstudante();
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
