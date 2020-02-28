<template>
    <CommonButton
        :html="'<span class=\'arrowed_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .arrowed_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/arrow_16.png');
}
.common-button.no-background .arrowed_button {
    background-image: url('/images/arrow_dark_16.png');
}
.common-button:hover .arrowed_button {
    background-image: url('/images/arrow_16_hover.png');
}
.common-button:active .arrowed_button {
    background-image: url('/images/arrow_16_click.png');
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
