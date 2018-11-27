import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/game/new',
      name: 'new-game',
      component: () => import('./views/NewGame.vue'),
    },
    {
      path: '/game/:gameId/cards/add',
      name: 'add-cards',
      component: () => import("./views/AddCards.vue"),
    },
    {
      path: '/game/:gameId/cards',
      name: 'cards',
      component: () => import('./views/Cards.vue'),
    }
  ]
});
