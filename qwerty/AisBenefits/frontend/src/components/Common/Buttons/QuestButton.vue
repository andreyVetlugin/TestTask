<template>
    <CommonButton
        :html="'<span class=\'quest_button\'></span>'"
        :noBackground="noBackground"
        @click="$emit('click')">
    </CommonButton>
</template>

<style>
.common-button .quest_button {
    width: 16px;
    height: 16px;
    display: inline-block;
    background-image: url('/images/quest_16.png');
}
.common-button:hover .quest_button {
    background-image: url('/images/quest_16_hover.png');
}
.common-button:active .quest_button {
    background-image: url('/images/quest_16_click.png');
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
