import { createStore, compose, applyMiddleware } from 'redux';

export default (reducers, middlewares) => {
  /* eslint-disable no-underscore-dangle */
  const enhancer =
    process.env.NODE_ENV === 'development'
      ? compose(
          applyMiddleware(...middlewares),
          window.__REDUX_DEVTOOLS_EXTENSION__ &&
            window.__REDUX_DEVTOOLS_EXTENSION__()
        )
      : applyMiddleware(...middlewares);
  /* eslint-enable */

  return createStore(reducers, enhancer);
};
