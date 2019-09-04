import { combineReducers } from 'redux';

import notificacoes from './alertas/reducers';
import usuario from './usuario/reducers';
import menu from './menu/reducers'

export default combineReducers({
  notificacoes,
  usuario,
  menu
});
