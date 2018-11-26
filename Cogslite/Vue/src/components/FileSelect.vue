<template>
    <div class="overflow-hidden relative w-64 mt-4 mb-4">
        <button class="bg-indigo hover:bg-indigo-dark text-white font-bold py-2 px-4 w-full inline-flex items-center rounded">
            <svg fill="#FFF" height="18" viewBox="0 0 24 24" width="18" xmlns="http://www.w3.org/2000/svg">
                <path d="M0 0h24v24H0z" fill="none"/>
                <path d="M9 16h6v-6h4l-7-7-7 7h4zm-4 2h14v2H5z"/>
            </svg>
            <span class="ml-2">{{text}}</span>
        </button>
        <input class="cursor-pointer absolute block opacity-0 pin-r pin-t" ref="file" type="file" @change="fileSelected">
    </div>
</template>

<script>
export default {
    props: ['text'],
    methods: {
        fileSelected : function() {

            var file = this.$refs.file.files[0];
            this.$emit("fileSelected", file);
            let reader = new FileReader();

            reader.addEventListener(
                "load",
                function() {                
                this.imagePreview = reader.result;
                this.$emit('previewReady', this.imagePreview);
                }.bind(this),
                false
            );

            if (file) {
                if (/\.(jpe?g|png|gif)$/i.test(file.name)) {
                reader.readAsDataURL(file);
                }
            }
        }
    }
}
</script>
