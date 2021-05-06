import PropTypes from 'prop-types';
import React, { useState } from 'react';

import {
  Colors,
  ModalConteudoHtml,
  LocalizadorEstudantesAtivos,
} from '~/componentes';

import { confirmar } from '~/servicos';

import { BotaoEstilizado } from './modalAlunos.css';

const ModalAlunos = ({
  codigoUe,
  modalVisivel,
  setModalVisivel,
  alunosSelecionados,
  setAlunosSelecionados,
  questoes,
  setModoEdicaoItinerancia,
  dataVisita
}) => {
  const [alunosSelecionadosModal, setAlunosSelecionadosModal] = useState(
    alunosSelecionados
  );
  const [modoEdicao, setModoEdicao] = useState(false);

  const mudarLocalizador = aluno => {
    const questoesAluno = [];
    questoes.forEach(questao => {
      questoesAluno.push({ ...questao, resposta: '' });
    });
    if (aluno) {
      setAlunosSelecionadosModal(estadoAntigo => {
        const alunoEncontrado = estadoAntigo.find(
          item => item.alunoCodigo.toString() === aluno.alunoCodigo.toString()
        );
        if (alunoEncontrado) {
          return estadoAntigo;
        }
        return [
          ...estadoAntigo,
          {
            ...aluno,
            turmaId: aluno.codigoTurma,
            podeRemover: true,
            questoes: questoesAluno,
          },
        ];
      });
      setModoEdicao(true);
    }
  };

  const removerAlunos = alunoCodigo => {
    setAlunosSelecionadosModal(estadoAntigo =>
      estadoAntigo.filter(item => item.alunoCodigo !== alunoCodigo)
    );
    setModoEdicao(true);
  };

  const esconderModal = () => setModalVisivel(false);

  const perguntarSalvarListaAluno = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    setAlunosSelecionados(alunosSelecionadosModal);
    setModoEdicaoItinerancia(true);
    setModoEdicao(false);
    esconderModal();
  };

  const fecharModal = async () => {
    esconderModal();
    if (modoEdicao) {
      const ehPraSalvar = await perguntarSalvarListaAluno();
      if (ehPraSalvar) {
        onConfirmarModal();
      }
    }
  };

  return (
    <ModalConteudoHtml
      titulo="Selecione a(s) criança(s)/estudante(s) relacionados a visita"
      visivel={modalVisivel}
      esconderBotaoSecundario
      onClose={fecharModal}
      onConfirmacaoPrincipal={onConfirmarModal}
      labelBotaoPrincipal="Confirmar"
      closable
      width="50%"
      tamanhoFonteTitulo="16"
      fecharAoClicarFora
      fecharAoClicarEsc
    >
      <div className="col-md-12 d-flex mb-4 p-0">
        <LocalizadorEstudantesAtivos
          id="estudante"
          showLabel
          exibirCodigoEOL
          semMargin
          limparCamposAposPesquisa
          ueId={codigoUe}
          onChange={mudarLocalizador}
          dataReferencia={dataVisita}
        />
      </div>
      {alunosSelecionadosModal?.map(
        ({ alunoCodigo, alunoNome, podeRemover }) => (
          <div
            className="col-md-12 d-flex justify-content-between mb-4"
            key={`${alunoCodigo}`}
          >
            <span>{`${alunoNome} (${alunoCodigo})`} </span>
            {podeRemover && (
              <BotaoEstilizado
                id="btn-excluir"
                icon="trash-alt"
                iconType="far"
                color={Colors.CinzaBotao}
                onClick={() => removerAlunos(alunoCodigo)}
                height="13px"
                width="13px"
              />
            )}
          </div>
        )
      )}
    </ModalConteudoHtml>
  );
};

ModalAlunos.defaultProps = {
  codigoUe: '',
  alunosSelecionados: [],
  modalVisivel: false,
  setModalVisivel: () => {},
  setAlunosSelecionados: () => {},
  setModoEdicaoItinerancia: () => {},
  questoes: [],
  dataVisita: '',
};

ModalAlunos.propTypes = {
  codigoUe: PropTypes.string,
  alunosSelecionados: PropTypes.oneOfType([PropTypes.any]),
  modalVisivel: PropTypes.bool,
  setModoEdicaoItinerancia: PropTypes.func,
  setModalVisivel: PropTypes.func,
  setAlunosSelecionados: PropTypes.func,
  questoes: PropTypes.oneOfType([PropTypes.any]),
  dataVisita: PropTypes.oneOfType([PropTypes.any]),
};

export default ModalAlunos;
