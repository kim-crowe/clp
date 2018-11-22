import Vue from 'vue';
import VueResource from 'vue-resource';
import router from './router';
import auth from './auth';
import App from './App.vue';
import axios from 'axios';

Vue.use(VueResource);
Vue.use(auth);
Vue.config.productionTip = false;
axios.defaults.baseURL = "http://localhost:5000/api";

new Vue({
  router,
  render: h => h(App),
}).$mount('#app');
