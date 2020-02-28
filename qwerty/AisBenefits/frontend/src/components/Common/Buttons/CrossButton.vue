<template>
    <CommonButton
        :html="'<span class=\'cross_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .cross_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/cross_16.png');
}
.common-button:active .cross_button,
.common-button:hover .cross_button {
    background-image: url('/images/cross_white_16_hover_click.png');
}

.common-button.no-background:hover .cross_button {
    background-image: url('/images/cross_16_hover.png');
}
.common-button.no-background:active .cross_button {
    background-image: url('/images/cross_16_click.png');
}
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
