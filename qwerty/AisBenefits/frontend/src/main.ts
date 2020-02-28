import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import RequestSender from './requestSender';

Vue.config.productionTip = false;

const mainApp = new Vue({
  router,
  store,
  render: (h) => h(App),
});

router.beforeEach((to, from, next) => {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (!store.getters.isAuthorized) {
      next('/login'); // redirect query
      return;
    }
  } else if (to.matched.some((record) => record.name === 'logout')) {
    store.dispatch('log_out');
    next('/login');
    return;
  }
  next();
});

store.commit('setRequestSender', new RequestSender(store));

store.dispatch('loadUserDataByToken').then(() => {
  if (!store.getters.isAuthorized) {
    router.push('/login');
  }
});

mainApp.$mount('#app');
