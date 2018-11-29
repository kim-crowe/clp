export default {

  install(Vue, options) {

    let modalController = new Vue({
      data: { $modal: null, $modalTracker: null }
    });

    Vue.mixin({
      computed: {
        $modal: {
          get: function () { return modalController.$data.$modal }
        }
      },
      methods: {
        $showModal: function (modal) {
          modalController.$data.$modal = modal;
          return new Promise((resolve, reject) => {
            modalController.$data.$modal.resolve = resolve;
            modalController.$data.$modal.reject = reject;
          });
        },
        $cancelModal: function () {
          modalController.$data.$modal = null;
        }
      }
    })
  }
}
