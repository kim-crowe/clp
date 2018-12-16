import Vue from 'vue';
import VueResource from 'vue-resource';
import router from './router';
import auth from './auth';
import modal from './modal';
import App from './App.vue';
import axios from 'axios';

Vue.use(VueResource);
Vue.use(auth);
Vue.use(modal);

Vue.config.productionTip = false;
axios.defaults.baseURL = "http://ccgworks-alb-1137454900.eu-west-2.elb.amazonaws.com/api";
//axios.defaults.baseURL = "http://localhost:5000/api";

new Vue({
  router,
  render: h => h(App),
}).$mount('#app');
