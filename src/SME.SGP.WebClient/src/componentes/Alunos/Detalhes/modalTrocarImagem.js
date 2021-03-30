import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Base, ModalConteudoHtml } from '~/componentes';
import UploadImagens from '~/componentes-sgp/UploadImagens/uploadImagens';
import Loader from '~/componentes/loader';
import { erros, sucesso } from '~/servicos';
import ServicoArmazenamento from '~/servicos/Componentes/ServicoArmazenamento';
import ServicoImagemEstudante from '~/servicos/Componentes/ServicoImagemEstudante';
import { getBase64DataURL } from '~/utils';
import { ContainerModalUploadImagem } from './styles';

const ModalTrocarImagem = props => {
  const { exbirModal, onCloseModal, codigoAluno, dadosImagem } = props;

  const [exibirLoader, setExibirLoader] = useState(false);
  const [imagemAtual, setImagemAtual] = useState([]);
  const [exibirCarregarImagem, setExibirCarregarImagem] = useState(true);
  const [trocouImagem, setTrocouImagem] = useState(false);

  const confirmacao = useSelector(state => state.alertas.confirmacao);

  const atualizarDadosImagem = dados => {
    if (dados) {
      setExibirCarregarImagem(false);
      setImagemAtual(dados);
    } else {
      setImagemAtual([]);
      setTimeout(() => {
        setExibirCarregarImagem(true);
      }, 500);
    }
  };

  const obterImagem = async uid => {
    setExibirLoader(true);
    const resposta = await ServicoArmazenamento.obterArquivoParaDownload(uid)
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (resposta?.data) {
      const type = resposta.headers['content-type'];
      const urlImagem = await getBase64DataURL(resposta?.data, type || '');
      const dadosMapeados = { ...dadosImagem };
      dadosMapeados.url = urlImagem;
      atualizarDadosImagem([dadosMapeados]);
    } else {
      atualizarDadosImagem();
    }
  };

  useEffect(() => {
    if (exbirModal && dadosImagem?.uid) {
      obterImagem(dadosImagem.uid);
    }
  }, [dadosImagem, exbirModal]);

  const afterSuccessUpload = dados => {
    if (dados?.codigo) {
      obterImagem(dados?.codigo);
    } else {
      atualizarDadosImagem();
    }
    setTrocouImagem(true);
  };

  const removerImagem = async codigoFoto => {
    if (codigoFoto) {
      setExibirLoader(true);
      const resposta = await ServicoImagemEstudante.excluirImagemEstudante(
        codigoFoto
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data) {
        setTrocouImagem(true);
        sucesso('Imagem excluída com sucesso');
        atualizarDadosImagem();
      }
    }
  };

  const configUploadImagens = {
    servicoCustomRequest: ServicoImagemEstudante.uploadImagemEstudante,
    parametrosCustomRequest: [
      {
        nome: 'codigoAluno',
        valor: codigoAluno,
      },
    ],
    afterSuccessUpload,
    removerImagem,
    listaInicialImagens: imagemAtual,
    showUploadList: {
      showRemoveIcon: true,
      showPreviewIcon: false,
      showDownloadIcon: false,
    },
    exibirCarregarImagem,
  };

  return exbirModal ? (
    <ModalConteudoHtml
      id="modal-alterar-imagem"
      key="alterar-imagem"
      visivel={exbirModal && !confirmacao?.visivel}
      titulo="Alterar a imagem"
      onClose={() => onCloseModal(trocouImagem)}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={400}
      closable
    >
      <span>
        A imagem deve ter uma resolução mínima de 180 x 180 pixels e não deve
        ser maior que 5 MB.
      </span>
      <Loader loading={exibirLoader} tip="">
        <ContainerModalUploadImagem>
          <UploadImagens {...configUploadImagens} />
          {exibirCarregarImagem ? (
            <div style={{ color: Base.CinzaBotao, fontSize: '12px' }}>
              .jpeg, .jpg e .png
            </div>
          ) : (
            ''
          )}
        </ContainerModalUploadImagem>
      </Loader>
    </ModalConteudoHtml>
  ) : (
    ''
  );
};

ModalTrocarImagem.propTypes = {
  exbirModal: PropTypes.bool,
  onCloseModal: PropTypes.oneOfType([PropTypes.func]),
  codigoAluno: PropTypes.string,
  dadosImagem: PropTypes.oneOfType([PropTypes.any]),
};

ModalTrocarImagem.defaultProps = {
  exbirModal: false,
  onCloseModal: () => {},
  codigoAluno: '',
  dadosImagem: null,
};

export default ModalTrocarImagem;
