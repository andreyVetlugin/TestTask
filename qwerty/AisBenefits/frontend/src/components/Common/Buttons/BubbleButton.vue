<template>
    <CommonButton
        :html="'<span class=\'bubble_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .bubble_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/bubble_16.png');
}
.common-button:hover .bubble_button {
    background-image: url('/images/bubble_16_hover.png');
}
.common-button:active .bubble_button {
    background-image: url('/images/bubble_16_click.png');
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
