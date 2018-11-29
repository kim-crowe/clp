export default {

  install(Vue, options) {

    let modalController = new Vue({
      data: { $modal: null }
    });

    Vue.mixin({
      computed: {
        $modal: {
          get: function () { return modalController.$data.$modal }
        },
      },
      methods: {
        $showModal(modal) {
          modalController.$data.$modal = modal;
        }
      }
    })
  }
}
