<template>
    <CommonButton
        :html="'<span class=\'symbol_c_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .symbol_c_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/symbol_c_16.png');
}
.common-button:hover .symbol_c_button,
.common-button:active .symbol_c_button {
    background-image: url('/images/symbol_c_white_16.png');
}
/* .common-button.no-background:hover .symbol_c_button {
    background-image: url('/images/symbol_c_16_hover.png');
}
.common-button.no-background:active .symbol_c_button {
    background-image: url('/images/symbol_c_16_click.png');
} */
</style>


<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
export default Vue.extend({
    props: {
        noBackground: Boolean,
    },
    components: {
        CommonButton,
    },
});
</script>
