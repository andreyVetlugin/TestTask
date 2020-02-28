<template>
    <CommonButton
        :html="'<span class=\'plus_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .plus_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/plus_16.png');
}
.common-button:hover .plus_button,
.common-button:active .plus_button {
    background-image: url('/images/plus_white_16.png');
}
.common-button.no-background:hover .plus_button {
    background-image: url('/images/plus_16_hover.png');
}
.common-button.no-background:active .plus_button {
    background-image: url('/images/plus_16_click.png');
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
