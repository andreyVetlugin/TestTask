<template>
    <CommonButton
        :html="'<span class=\'edit_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .edit_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/edit_16.png');
}
.common-button:hover .edit_button,
.common-button:active .edit_button {
    background-image: url('/images/edit_white_16.png');
}
.common-button.no-background:hover .edit_button {
    background-image: url('/images/edit_16_hover.png');
}
.common-button.no-background:active .edit_button {
    background-image: url('/images/edit_16_click.png');
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
