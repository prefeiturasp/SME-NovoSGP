import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import {
  ModalConteudoHtml,
  Loader,
  Label,
  SelectComponent,
} from '~/componentes';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';

const CopiarConteudo = ({
  visivel,
  anoLetivo,
  codigoDisciplinaSelecionada,
  unidadeEscolar,
  turmaId,
  onConfirmarCopiarConteudo,
  onCancelarCopiarConteudo,
  onCloseCopiarConteudo,
  onChangeCopiarConteudo,
}) => {
  const [turmasSelecionadas, setTurmasSelecionadas] = useState([]);
  const [turmasComPlanoAnual, setTurmasComPlanoAnual] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [loader, setLoader] = useState(false);

  const turmasUsuario = useSelector(c => c.usuario.turmasUsuario);

  useEffect(() => {
    servicoPlanoAnual
      .obterTurmasParaCopia(
        turmasUsuario,
        anoLetivo,
        codigoDisciplinaSelecionada,
        unidadeEscolar,
        turmaId
      )
      .then(c => {
        setListaTurmas(c);
      })
      .catch(e => {
        debugger;
      });
  }, []);

  const modalCopiarConteudoAlertaVisivel = () => {
    return turmasSelecionadas.some(selecionada =>
      turmasComPlanoAnual.includes(selecionada)
    );
  };

  const modalCopiarConteudoAtencaoTexto = () => {
    const turmasReportar = turmasUsuario
      ? turmasUsuario
          .filter(
            turma =>
              turmasSelecionadas.includes(`${turma.valor}`) &&
              turmasComPlanoAnual.includes(turma.valor)
          )
          .map(turma => turma.desc)
      : [];

    return turmasReportar.length > 1
      ? `As turmas ${turmasReportar.join(
          ', '
        )} já possuem plano anual que serão sobrescritos ao realizar a cópia. Deseja continuar?`
      : `A turma ${turmasReportar[0]} já possui plano anual que será sobrescrito ao realizar a cópia. Deseja continuar?`;
  };

  return (
    <ModalConteudoHtml
      key="copiarConteudo"
      visivel={visivel}
      onConfirmacaoPrincipal={onConfirmarCopiarConteudo}
      onConfirmacaoSecundaria={onCancelarCopiarConteudo}
      onClose={onCloseCopiarConteudo}
      labelBotaoPrincipal="Copiar"
      tituloAtencao={modalCopiarConteudoAlertaVisivel() ? 'Atenção' : null}
      perguntaAtencao={
        modalCopiarConteudoAlertaVisivel()
          ? modalCopiarConteudoAtencaoTexto()
          : null
      }
      labelBotaoSecundario="Cancelar"
      titulo="Copiar Conteúdo"
      closable={false}
      loader={loader}
      desabilitarBotaoPrincipal={
        turmasSelecionadas && turmasSelecionadas.length < 1
      }
    >
      <Loader loading={loader}>
        <Label
          htmlFor="SelecaoTurma"
          alt="Selecione uma ou mais turmas de destino"
        >
          Copiar para a(s) turma(s)
        </Label>
        <SelectComponent
          id="SelecaoTurma"
          lista={listaTurmas}
          valueOption="valor"
          valueText="desc"
          onChange={onChangeCopiarConteudo}
          valueSelect={turmasSelecionadas}
          multiple
        />
      </Loader>
    </ModalConteudoHtml>
  );
};
export default CopiarConteudo;
