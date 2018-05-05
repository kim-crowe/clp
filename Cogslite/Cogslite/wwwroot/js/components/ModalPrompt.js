Vue.component('modal-prompt',
    {
        props: ['title', 'message', 'placeholder', 'confirmText', 'cancelText'],
        data: function () {
            return {
                value: ''
            }
        },
        computed: {
            hasMessage: function () {
                return this.message;
            },
            confirmButtonText: function () {
                if (this.confirmText)
                    return this.confirmText;
                return "Ok";
            },
            cancelButtonText: function () {
                if (this.cancelText)
                    return this.cancelText;
                return "Cancel";
            }
        },
        methods: {
            confirm: function () {
                this.$emit('confirm', this.value);
                this.value = ''
            }
        },
        template: `<div class="modal" tabindex="-1" role="dialog">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">{{title}}</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <p v-if="hasMessage">{{message}}</p>
        <input type="text" class="form-control" v-bind:placeholder="placeholder" v-model="value"/>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" v-on:click="confirm" data-dismiss="modal">{{confirmButtonText}}</button>
        <button type="button" class="btn btn-secondary" data-dismiss="modal">{{cancelButtonText}}</button>
      </div>
    </div>
  </div>
</div>`
    })