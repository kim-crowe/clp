Vue.component('modal-form',
    {
        props: ['options'],
        data: function () {
            return {
                model: {}
            }
        },        
        watch: {
            "options": function (val) {
                this.initialise();
            }
        },
        computed: {            
            confirmButtonText: function () {
                if (this.options.confirmText)
                    return this.options.confirmText;
                return "Ok";
            },
            cancelButtonText: function () {
                if (this.options.cancelText)
                    return this.options.cancelText;
                return "Cancel";
            }
        },
        methods: {
            initialise: function () {
                this.model = {};                
                for (var i = 0; i < this.options.fields.length; i++) {
                    var option = this.options.fields[i];
                    if (typeof option.value !== 'undefined' && typeof option.name !== 'undefined')
                        this.model[option.name] = option.value;
                }
            },
            confirm: function () {               
                this.$emit('confirm', this.model);
                this.model = {}
            }
        },
        template: `<div class="modal" tabindex="-1" role="dialog">
  <div class="modal-dialog" role="document" v-if="options">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">{{options.title}}</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <data-form :fields="options.fields" :model="model"></data-form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" v-on:click="confirm" data-dismiss="modal">{{confirmButtonText}}</button>
        <button type="button" class="btn btn-secondary" data-dismiss="modal">{{cancelButtonText}}</button>
      </div>
    </div>
  </div>
</div>`
    })

Vue.component('data-form',
    {
        props: ['fields', 'model'],        
        methods: {
            componentFor: function (type) {
                return "data-form-" + type;
            },
            updateModel: function (dataElement, value) {
                this.model[dataElement] = value;
            }
        },
        template: `<div>
    <div v-for="field in fields">
        <component v-bind:is="componentFor(field.type)" v-bind:data="field" v-on:updated="updateModel(field['name'], $event)"></component>
    </div>
  </div>`

    })

Vue.component('data-form-image',
    {
        props: ['data'],
        template: `<div class="form-group"><img :src="data.value" class="mx-auto" style="display: block;" /></div>`
    })

Vue.component('data-form-text',
    {
        props: ['data'],
        watch: {
            "data": function (val) {
                if (typeof val.data.value !== 'undefined')
                    this.$emit('updated', val.data.value);
            }
        },
        computed: {
            dataJson: function () {
                return JSON.stringify(this.data);
            },
            isReadOnly: function () {
                if (this.data.readonly)
                    return this.data.readonly;
                return false;
            },
            initialValue: function () {
                if (typeof this.data.value !== 'undefined')
                    return this.data.value;
                return '';
            }
        },
        template: `<div class="form-group">
            <label v-if="data.label">{{data.label}}</label>
            <input type="text" class="form-control" :placeholder="data.placeholder" :value="initialValue" v-on:input="$emit('updated', $event.target.value)" :readonly="isReadOnly" />
            </div>`

    })

Vue.component('data-form-options',
    {
        props: ['data'],
        data: {
            selected: null
        },
        computed: {
            dataJson: function () {
                return JSON.stringify(this.data);
            }
        },
        template: `<div class="form-group">
        <label>{{data.label}}</label>
        <b-select :options="data.options" :select-size="5" v-model="selected" v-on:change="$emit('updated', $event)">
        </b-select>
    </div>`
    })

Vue.component('data-form-copytext',
    {
        props: ['data'],
        methods: {
            copyData: function () {
                $('#'+this.data.name).select();
                document.execCommand('copy');
            }
        },
        template: `<div class="form-group">
        <label>{{data.label}}</label>
        <div class="input-group">
            <input type="text" class="form-control" :id="data.name" :value="data.value" readonly />
            <div class="input-group-append">
                <a href="#" class="input-group-text" v-on:click="copyData"><i class="fa fa-copy"></i></a>                
            </div>
        </div>
    </div>`
    })

Vue.component('data-form-optiongroup',
    {
        props: ['data'],
        template: `<div class="form-group">
        <label>{{data.label}}</label>
        <div v-for="option in data.options">
            <input :id="data.data-id" type="radio" :value="option"/> {{option}}
        </div>
    </div>`
    })

Vue.component('data-form-range',
    {
        props: ['data'],
        computed: {
            dataJson: function () {
                return JSON.stringify(this.data);
            }
        },
        template: `<div class="form-group">
        <label>{{data.label}}</label>
        <div>Min: {{data.min}}</div>
        <div>Max: {{data.max}}</div>
    </div>`
    })

