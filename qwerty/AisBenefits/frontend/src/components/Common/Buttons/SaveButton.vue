<template>
    <CommonButton
        :html="'<span class=\'save_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .save_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/save_16.png');
}
.common-button:hover .save_button,
.common-button:active .save_button {
    background-image: url('/images/save_white_16.png');
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
